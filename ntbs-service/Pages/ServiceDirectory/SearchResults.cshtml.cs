using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Pages.Search;
using ntbs_service.Services;

namespace ntbs_service.Pages.ServiceDirectory
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SearchResults : ServiceDirectorySearchBase
    {
        private readonly IUserSearchService _userSearchService;
        private readonly IServiceDirectoryService _serviceDirectoryService;
        private IReferenceDataRepository _referenceDataRepository;
        private PaginationParametersBase _paginationParameters;
        public PaginatedList<ServiceDirectoryItemWrapper> DirectorySearchResults;
        public IList<PHEC> AllPhecs;
        public string NextPageUrl;
        public string PreviousPageUrl;

        public SearchResults(
            IUserSearchService userSearchService, 
            IReferenceDataRepository referenceDataRepository, 
            IServiceDirectoryService serviceDirectoryService)
        {
            _userSearchService = userSearchService;
            _referenceDataRepository = referenceDataRepository;
            _serviceDirectoryService = serviceDirectoryService;
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
            
            var searchKeywords = SearchStringHelper.GetSearchKeywords(SearchKeyword);
            
            var usersToDisplay = await _userSearchService.OrderQueryableAsync(searchKeywords);
            var regionsToDisplay = await _referenceDataRepository.GetPhecsBySearchKeywords(searchKeywords);
            var tbServicesToDisplay = await _referenceDataRepository.GetActiveTBServicesBySearchKeywords(searchKeywords);
            var hospitalsToDisplay = await _referenceDataRepository.GetActiveHospitalsBySearchKeywords(searchKeywords);

            var (paginatedResults, count) = 
                _serviceDirectoryService.GetPaginatedItems(
                    regionsToDisplay,
                    usersToDisplay,
                    tbServicesToDisplay,
                    hospitalsToDisplay,
                    _paginationParameters);
            
            DirectorySearchResults = new PaginatedList<ServiceDirectoryItemWrapper>(paginatedResults, count, _paginationParameters);

            AllPhecs = await _referenceDataRepository.GetAllPhecs();

            if (DirectorySearchResults.HasNextPage)
            {
                NextPageUrl = QueryHelpers.AddQueryString("/ServiceDirectory/SearchResults",
                    new Dictionary<string, string>
                    {
                        {"SearchKeyword", SearchKeyword},
                        {"pageIndex", (_paginationParameters.PageIndex + 1).ToString()},
                        {"offset", (_paginationParameters.Offset + paginatedResults.Count).ToString()}
                    });
            }

            if (DirectorySearchResults.HasPreviousPage)
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
