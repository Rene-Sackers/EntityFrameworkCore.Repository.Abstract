using System;
using System.Linq;

namespace EntityFrameworkCore.Repository.Abstract.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Optional<T>(this IQueryable<T> query, Func<IQueryable<T>, IQueryable<T>> optionalQuery) =>
            optionalQuery == null ? query : optionalQuery(query);
    }
}