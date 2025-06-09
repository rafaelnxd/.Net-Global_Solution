using System.Collections.Generic;

namespace HeatWatch.API.Helpers
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; }
        public int TotalCount { get; }
        public int Page { get; }
        public int PageSize { get; }

        public PagedResult(IEnumerable<T> items, int total, int page, int size)
        {
            Items = items;
            TotalCount = total;
            Page = page;
            PageSize = size;
        }
    }
}
