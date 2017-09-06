using System;
using System.Collections.Generic;

namespace EntityFrameworkCore.Repository.Abstract.Models
{
    public class PagedResult<T>
    {
        public ICollection<T> Items { get; }

        public int CurrentPage { get; }

        public int PageSize { get; }

        public int TotalItems { get; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public PagedResult(int currentPage, int pageSize, int totalItems, ICollection<T> items)
        {
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            Items = items;
        }
    }
}