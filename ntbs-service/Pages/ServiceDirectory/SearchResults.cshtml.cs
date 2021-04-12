using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Pages.Search;
using ntbs_service.Services;

namespace ntbs_service.Pages.ServiceDirectory
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SearchResults : ServiceDirectorySearchBase
    {
        private readonly ICaseManagerSearchService _caseManagerSearchService;
        private IReferenceDataRepository _referenceDataRepository;
        private PaginationParametersBase _paginationParameters;
        public PaginatedList<User> CaseManagersSearchResults;
        public IList<PHEC> AllPhecs;
        public string NextPageUrl;
        public string PreviousPageUrl;

        public SearchResults(ICaseManagerSearchService caseManagerSearchService, IReferenceDataRepository referenceDataRepository)
        {
            _caseManagerSearchService = caseManagerSearchService;
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex = null, int? offset = null)
        {
            if (!ModelState.IsValid || SearchKeyword.IsNullOrEmpty())
            {
                return Page();
            }

            _paginationParameters = new PaginationParametersBase
            {
                PageSize = 20,
                PageIndex = pageIndex ?? 1,
                Offset = offset ?? 0
            };

            var (caseManagersToDisplay, count) =
                await _caseManagerSearchService.OrderAndPaginateQueryableAsync(SearchKeyword, _paginationParameters);

            CaseManagersSearchResults = new PaginatedList<User>(caseManagersToDisplay, count, _paginationParameters);

            AllPhecs = await _referenceDataRepository.GetAllPhecs();

            if (CaseManagersSearchResults.HasNextPage)
            {
                NextPageUrl = QueryHelpers.AddQueryString("/ServiceDirectory/SearchResults",
                    new Dictionary<string, string>
                    {
                        {"SearchKeyword", SearchKeyword},
                        {"pageIndex", (_paginationParameters.PageIndex + 1).ToString()},
                        {"offset", (_paginationParameters.Offset + caseManagersToDisplay.Count).ToString()}
                    });
            }

            if (CaseManagersSearchResults.HasPreviousPage)
            {
                PreviousPageUrl = QueryHelpers.AddQueryString("/ServiceDirectory/SearchResults",
                    new Dictionary<string, string>
                    {
                        {"SearchKeyword", SearchKeyword},
                        {"pageIndex", (_paginationParameters.PageIndex - 1).ToString()},
                        {"offset", (_paginationParameters.Offset - _paginationParameters.PageSize).ToString()}
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
