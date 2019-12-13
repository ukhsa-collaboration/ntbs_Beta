using System;
using System.Collections.Generic;
using ntbs_service.Models;

namespace ntbs_service.Pages.Search
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int NumberOfResults { get; private set; }

        public PaginatedList(IEnumerable<T> items, int count, PaginationParameters paginationParameters)
        {
            PageIndex = paginationParameters.PageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)paginationParameters.PageSize);
            NumberOfResults = count;

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public string PreviousPageText => HasPreviousPage ? "Page " + (PageIndex - 1) + " of " + (TotalPages) : null;
        public string NextPageText => HasNextPage ? "Page " + (PageIndex + 1) + " of " + (TotalPages) : null;
    }
}
