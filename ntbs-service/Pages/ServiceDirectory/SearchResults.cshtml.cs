using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Pages.Search;
using ntbs_service.Services;

namespace ntbs_service.Pages.ServiceDirectory
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SearchResults : ServiceDirectorySearchBase
    {
        private ICaseManagerSearchService _caseManagerSearchService;
        private PaginationParametersBase PaginationParameters;
        public PaginatedList<User> CaseManagersSearchResults;
        public string NextPageUrl;
        public string PreviousPageUrl;

        public SearchResults(ICaseManagerSearchService caseManagerSearchService)
        {
            _caseManagerSearchService = caseManagerSearchService;
        }
        
        public async Task<IActionResult> OnGetAsync(int? pageIndex = null, int? offset = null)
        {
            if (!ModelState.IsValid || SearchKeyword.IsNullOrEmpty())
            {
                return Page();
            }

            PaginationParameters = new PaginationParametersBase()
            {
                PageSize = 20, PageIndex = pageIndex ?? 1, Offset = offset ?? 0
            };

            var (caseManagersToDisplay, count) =
                await _caseManagerSearchService.OrderAndPaginateQueryableAsync(SearchKeyword, PaginationParameters);

            CaseManagersSearchResults = new PaginatedList<User>(caseManagersToDisplay, count, PaginationParameters);

            if (CaseManagersSearchResults.HasNextPage)
            {
                NextPageUrl = QueryHelpers.AddQueryString($"/ServiceDirectory/SearchResults",
                    new Dictionary<string, string>
                    {
                        {"SearchKeyword", SearchKeyword},
                        {"pageIndex", (PaginationParameters.PageIndex + 1).ToString()},
                        {"offset", (PaginationParameters.Offset + caseManagersToDisplay.Count).ToString()}
                    });
            }

            if (CaseManagersSearchResults.HasPreviousPage)
            {
                PreviousPageUrl = QueryHelpers.AddQueryString($"/ServiceDirectory/SearchResults",
                    new Dictionary<string, string>
                    {
                        {"SearchKeyword", SearchKeyword},
                        {"pageIndex", (PaginationParameters.PageIndex - 1).ToString()},
                        {"offset", (PaginationParameters.Offset - PaginationParameters.PageSize).ToString()}
                    });
            }

            PrepareBreadcrumbs();

            return Page();
        }
        
        private void PrepareBreadcrumbs()
        {
            var breadcrumbs = new List<Breadcrumb>
            {
                new Breadcrumb {Label = "Service Directory", Url = "/ServiceDirectory"},
                new Breadcrumb {Label = "Search results", Url = Request.GetDisplayUrl()}
            };

            ViewData["Breadcrumbs"] = breadcrumbs;
        }
    }
}
