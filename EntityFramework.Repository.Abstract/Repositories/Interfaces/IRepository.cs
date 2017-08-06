using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EntityFramework.Repository.Abstract.Models;
using EntityFramework.Repository.Abstract.Models.Interfaces;

namespace EntityFramework.Repository.Abstract.Repositories.Interfaces
{
    public interface IRepository
    {
        Task<int> TotalCount();

        Task SaveChanges();
    }
    
    public interface IRepository<TEntity, in TKey> : IRepository where TEntity : class, IId<TKey>
    {
        Task<TEntity> GetById(TKey id, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null);

        Task<TEntity> GetByKey(object[] keyValues);
        
        Task<ICollection<TEntity>> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null);

        Task<PagedResult<TEntity>> GetPaged(int page, int itemsPerPage, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryExtra = null);

        Task Add(TEntity entity);

        Task Remove(TEntity entity);

        Task Remove(params TEntity[] entity);

        Task Remove(ICollection<TEntity> entity);
    }
}