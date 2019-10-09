using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.Pages_Search
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
        private PaginatedList(IEnumerable<T> items)
        {
            this.AddRange(items);
        }

        public PaginatedList<TResult> SelectItems<TResult>(Func<T, TResult> selector) {
            var newItems = this.Select(selector);
            return new PaginatedList<TResult>(newItems) {
                PageIndex = this.PageIndex,
                TotalPages = this.TotalPages,
                NumberOfResults = this.NumberOfResults
            };
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
    }
}