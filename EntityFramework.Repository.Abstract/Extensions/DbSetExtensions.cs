using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework.Repository.Abstract.Extensions
{
    public static class DbSetExtensions
    {
        public static Task RemoveAsync<T>(this DbSet<T> dbSet, T entity) where T : class
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;
        }
    }
}