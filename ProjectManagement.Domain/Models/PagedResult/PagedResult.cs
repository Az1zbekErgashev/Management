﻿using System.Text.Json.Serialization;

namespace ProjectManagement.Domain.Models.PagedResult
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public long TotalItems { get; }
        public int ItemsPerPage { get; }
        public long CurrentItemCount { get; protected set; }
        public int PageIndex { get; }
        public int TotalPages { get; }

        [JsonConstructor]
        protected PagedResult(
        IEnumerable<T> items,
        long totalItems, int itemsPerPage,
        long currentItemCount,
        int pageIndex,
        int totalPages
        )
        {
            Items = items;
            TotalItems = totalItems;
            ItemsPerPage = itemsPerPage;
            CurrentItemCount = currentItemCount;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }

        public static PagedResult<T> Create(
            IEnumerable<T> items,
            long totalItems,
            int itemsPerPage,
            long currentItemCount,
            int pageIndex,
            int totalPages

        )
        {
            return new PagedResult<T>(items, totalItems, itemsPerPage, currentItemCount, pageIndex, totalPages);
        }
    }

    public static class PagedResult
    {
        public static PagedResult<T> Create<T>(
            IList<T> items,
            long totalItems,
            int itemsPerPage,
            int pageIndex
        )
        {
            int totalPages = (int)((totalItems % itemsPerPage == 0) ? (totalItems / itemsPerPage) : (1 + totalItems / itemsPerPage));
            return PagedResult<T>.Create(items, totalItems, itemsPerPage, items.Count, pageIndex, totalPages);
        }
    }
}
