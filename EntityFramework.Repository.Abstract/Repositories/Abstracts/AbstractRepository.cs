using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Repository.Abstract.Extensions;
using EntityFramework.Repository.Abstract.Models;
using EntityFramework.Repository.Abstract.Models.Interfaces;
using EntityFramework.Repository.Abstract.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Repository.Abstract.Repositories.Abstracts
{
    public abstract class AbstractRepository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IId<TKey>
    {
        private readonly DbContext _context;

        protected readonly DbSet<TEntity> DbSet;

        protected AbstractRepository(DbContext context)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
        }

        public virtual Task<TEntity> GetById(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null) =>
            DbSet.Optional(queryExtra).SingleOrDefaultAsync(entity => Equals(entity.Id, id));

        public virtual Task<TEntity> GetByKey(object[] keyValues) =>
            DbSet.FindAsync(keyValues);

        public virtual async Task<ICollection<TEntity>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null) =>
            await DbSet.Optional(queryExtra).ToListAsync();

        public virtual async Task<PagedResult<TEntity>> GetPaged(int page, int itemsPerPage, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null)
        {
            if (page < 1)
                throw new IndexOutOfRangeException($"Page number {page} is below 1.");

            var startIndex = (page - 1) * itemsPerPage;

            var finalQuery = DbSet.Optional(queryExtra);

            var resultCount = await finalQuery.CountAsync();
            var items = await finalQuery.Skip(startIndex).Take(itemsPerPage).ToListAsync();

            return new PagedResult<TEntity>(page, itemsPerPage, resultCount, items);
        }

        public Task<int> TotalCount() => DbSet.CountAsync();

        public virtual Task Add(TEntity entity) => DbSet.AddAsync(entity);

        public virtual Task Remove(TEntity entity) => DbSet.RemoveAsync(entity);

        public Task Remove(params TEntity[] entity) => Remove((ICollection<TEntity>)entity);

        public Task Remove(ICollection<TEntity> entity) => Task.WhenAll(entity.ToList().Select(Remove));

        public virtual Task SaveChanges() => _context.SaveChangesAsync();
    }
}