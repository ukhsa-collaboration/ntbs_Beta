using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Pages.Search;
using ntbs_service.Services;

namespace ntbs_service.Pages.ServiceDirectory
{
    public class SearchResults : ServiceDirectorySearchBase
    {
        public ICaseManagerSearchService _caseManagerSearchService;
        public PaginationParametersBase PaginationParameters;
        public PaginatedList<User> CaseManagersSearchResults;
        public string NextPageUrl;
        public string PreviousPageUrl;

        public SearchResults(ICaseManagerSearchService caseManagerSearchService)
        {
            _caseManagerSearchService = caseManagerSearchService;
        }
        
        public async Task<IActionResult> OnGetAsync(int? pageIndex = null, int? offset = null)
        {
            PaginationParameters = new PaginationParametersBase()
            {
                PageSize = 20, 
                PageIndex = pageIndex ?? 1, 
                Offset = offset ?? 0
            };

            var (caseManagersToDisplay, count) =
                await _caseManagerSearchService.OrderAndPaginateQueryableAsync(SearchKeyword, PaginationParameters);
            
            CaseManagersSearchResults = new PaginatedList<User>(caseManagersToDisplay, count, PaginationParameters);

            if (CaseManagersSearchResults.HasNextPage)
            {
                NextPageUrl = QueryHelpers.AddQueryString($"/ServiceDirectory/SearchResults", new Dictionary<string, string>
                {
                    {"SearchKeyword", SearchKeyword},
                    {"pageIndex", (PaginationParameters.PageIndex + 1).ToString()},
                    {"offset", (PaginationParameters.Offset + caseManagersToDisplay.Count()).ToString()}
                });
            }
            
            if (CaseManagersSearchResults.HasPreviousPage)
            {
                PreviousPageUrl = QueryHelpers.AddQueryString($"/ServiceDirectory/SearchResults", new Dictionary<string, string>
                {
                    {"SearchKeyword", SearchKeyword},
                    {"pageIndex", (PaginationParameters.PageIndex - 1).ToString()},
                    {"offset", (PaginationParameters.Offset - PaginationParameters.PageSize).ToString()}
                });
            }

            return Page();
        }
    }
}
