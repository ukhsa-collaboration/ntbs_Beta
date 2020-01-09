using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ISearchService _searchService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILegacySearchService _legacySearchService;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public ValidationService ValidationService;

        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string PreviousPageUrl;
        public PaginationParameters PaginationParameters;

        public List<Sex> Sexes { get; set; }
        public SelectList Countries { get; set; }
        public SelectList TbServices { get; set; }

        [BindProperty(SupportsGet = true)]
        public SearchParameters SearchParameters { get; set; }

        public IndexModel(
            INotificationRepository notificationRepository,
            ISearchService searchService,
            IAuthorizationService authorizationService,
            IReferenceDataRepository referenceDataRepository,
            ILegacySearchService legacySearchService)
        {
            _authorizationService = authorizationService;
            _searchService = searchService;
            _notificationRepository = notificationRepository;
            _legacySearchService = legacySearchService;
            _referenceDataRepository = referenceDataRepository;

            ValidationService = new ValidationService(this);

            Sexes = referenceDataRepository.GetAllSexesAsync().Result.ToList();
            TbServices = new SelectList(
                referenceDataRepository.GetAllTbServicesAsync().Result,
                nameof(TBService.Code),
                nameof(TBService.Name));
            Countries = new SelectList(
                referenceDataRepository.GetAllCountriesAsync().Result,
                nameof(Country.CountryId),
                nameof(Country.Name));
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex = null, int? legacyOffset = null, int? ntbsOffset = null, int? previousLegacyOffset = null, int? previousNtbsOffset = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PaginationParameters = new PaginationParameters()
            {
                PageSize = 50,
                PageIndex = pageIndex ?? 1,
                LegacyOffset = legacyOffset,
                NtbsOffset = ntbsOffset
            };

            var draftsQueryable = _notificationRepository.GetQueryableNotificationByStatus(new List<NotificationStatus>() {
                NotificationStatus.Draft });
            var nonDraftsQueryable = _notificationRepository.GetQueryableNotificationByStatus(new List<NotificationStatus>() {
                NotificationStatus.Notified,
                NotificationStatus.Denotified });

            var ntbsFilteredDraftsBuilder = (INtbsSearchBuilder)FilterBySearchParameters(new NtbsSearchBuilder(draftsQueryable));
            var ntbsFilteredNonDraftsBuilder = (INtbsSearchBuilder)FilterBySearchParameters(new NtbsSearchBuilder(nonDraftsQueryable));
            var legacyFilteredSearchBuilder = (ILegacySearchBuilder)FilterBySearchParameters(new LegacySearchBuilder(_referenceDataRepository));

            var (notificationsToDisplay, count) = await SearchAsync(ntbsFilteredDraftsBuilder, ntbsFilteredNonDraftsBuilder, legacyFilteredSearchBuilder);
            var authorisedNotificationsToDisplay = _authorizationService.SetFullAccessOnNotificationBanners(notificationsToDisplay, User);
            SearchResults = new PaginatedList<NotificationBannerModel>(authorisedNotificationsToDisplay, count, PaginationParameters);
            var (nextNtbsOffset, nextLegacyOffset) = CalculateNextOffsets(PaginationParameters.PageIndex, legacyOffset, ntbsOffset, notificationsToDisplay);
            SetPaginationDetails(nextNtbsOffset, nextLegacyOffset, previousNtbsOffset, previousLegacyOffset, ntbsOffset, legacyOffset);

            return Page();
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidationService.GetPropertyValidationResult(this, key, value);
        }

        private ISearchBuilder FilterBySearchParameters(ISearchBuilder searchBuilder)
        {
            return searchBuilder
                .FilterById(SearchParameters.IdFilter)
                .FilterByFamilyName(SearchParameters.FamilyName)
                .FilterByGivenName(SearchParameters.GivenName)
                .FilterByPartialDob(SearchParameters.PartialDob)
                .FilterByPostcode(SearchParameters.Postcode)
                .FilterByPartialNotificationDate(SearchParameters.PartialNotificationDate)
                .FilterBySex(SearchParameters.SexId)
                .FilterByBirthCountry(SearchParameters.CountryId)
                .FilterByTBService(SearchParameters.TBServiceCode);
        }
        
        private async Task<(IEnumerable<NotificationBannerModel> results, int count)> SearchAsync(
            INtbsSearchBuilder filteredDrafts,
            INtbsSearchBuilder filteredNonDrafts,
            ILegacySearchBuilder legacySqlQuery)
        {
            if (PaginationParameters.LegacyOffset == null && PaginationParameters.NtbsOffset == null)
            {
                return await SearchWithoutOffsetsAsync(filteredDrafts, filteredNonDrafts, legacySqlQuery);
            }
            else
            {
                return await SearchWithOffsetsAsync(filteredDrafts, filteredNonDrafts, legacySqlQuery);
            }
        }

        // Given no offsets from the previous page perform a search without using skip and take in SQL queries
        private async Task<(IEnumerable<NotificationBannerModel> results, int count)> SearchWithoutOffsetsAsync(
            INtbsSearchBuilder filteredDrafts,
            INtbsSearchBuilder filteredNonDrafts,
            ILegacySearchBuilder legacySqlQuery)
        {
            var numberOfNotificationsToFetch = PaginationParameters.PageSize * PaginationParameters.PageIndex;
            var (orderedNotificationIds, ntbsCount) = await _searchService.OrderAndPaginateQueryablesAsync(
                filteredDrafts,
                filteredNonDrafts,
                PaginationParameters);
            var ntbsNotifications = await _notificationRepository.GetNotificationBannerModelsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.SearchAsync(
                legacySqlQuery,
                0,
                numberOfNotificationsToFetch);
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var notifications = allPossibleNotifications
                .OrderByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.SortByDate)
                .ThenByDescending(n => n.NotificationId)
                .Skip(numberOfNotificationsToFetch - PaginationParameters.PageSize)
                .Take(PaginationParameters.PageSize);
            return (notifications, ntbsCount + legacyCount);
        }

        // Given offsets from the previous page perform a search with the correct skip and take to be efficient
        private async Task<(IEnumerable<NotificationBannerModel> results, int count)> SearchWithOffsetsAsync(
            INtbsSearchBuilder filteredDrafts,
            INtbsSearchBuilder filteredNonDrafts,
            ILegacySearchBuilder legacySqlQuery)
        {
            var (orderedNotificationIds, ntbsCount) = await _searchService.OrderAndPaginateQueryablesAsync(
                filteredDrafts,
                filteredNonDrafts,
                PaginationParameters);
            var ntbsNotifications = await _notificationRepository.GetNotificationBannerModelsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.SearchAsync(
                legacySqlQuery,
                (int)PaginationParameters.LegacyOffset,
                PaginationParameters.PageSize);
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var notifications = allPossibleNotifications
                .OrderByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.SortByDate)
                .ThenByDescending(n => n.NotificationId)
                .Take(PaginationParameters.PageSize);
            return (notifications, ntbsCount + legacyCount);
        }

        private (int? nextNtbsOffset, int? nextLegacyOffset) CalculateNextOffsets(
            int pageIndex,
            int? currentLegacyOffset,
            int? currentNtbsOffset,
            IEnumerable<NotificationBannerModel> notificationsToDisplay)
        {
            if (pageIndex == 1)
            {
                currentNtbsOffset = 0;
                currentLegacyOffset = 0;
            }

            if (currentNtbsOffset != null && currentLegacyOffset != null)
            {
                var legacyCount = notificationsToDisplay.Count(n => n.NotificationStatus == NotificationStatus.Legacy);
                var ntbsCount = notificationsToDisplay.Count(n => n.NotificationStatus != NotificationStatus.Legacy);
                var nextNtbsOffset = ntbsCount + currentNtbsOffset;
                var nextLegacyOffset = legacyCount + currentLegacyOffset;
                return (nextNtbsOffset, nextLegacyOffset);
            }
            return (null, null);
        }

        private void SetPaginationDetails(
            int? nextNtbsOffset,
            int? nextLegacyOffset,
            int? previousNtbsOffset,
            int? previousLegacyOffset,
            int? ntbsOffset,
            int? legacyOffset)
        {
            if (SearchResults.HasPreviousPage)
            {
                var previousPageParameters = CreateSearchPageParameters(
                    SearchResults.PageIndex - 1,
                    previousNtbsOffset,
                    previousLegacyOffset);
                PreviousPageUrl = QueryHelpers.AddQueryString("/Search", previousPageParameters);
            }
            if (SearchResults.HasNextPage)
            {
                var nextPageParameters = CreateSearchPageParameters(
                    SearchResults.PageIndex + 1,
                    nextNtbsOffset,
                    nextLegacyOffset,
                    ntbsOffset,
                    legacyOffset);
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageParameters);
            }
        }

        private Dictionary<string, string> CreateSearchPageParameters(
            int pageIndex,
            int? ntbsOffset,
            int? legacyOffset,
            int? previousNtbsOffset = null,
            int? previousLegacyOffset = null)
        {
            var queryStringDictionary = GetCurrentSearchParameters();
            queryStringDictionary["pageIndex"] = pageIndex.ToString();
            if (ntbsOffset != null && legacyOffset != null)
            {
                queryStringDictionary["ntbsOffset"] = ntbsOffset.ToString();
                queryStringDictionary["legacyOffset"] = legacyOffset.ToString();
            }
            if (previousNtbsOffset != null && previousLegacyOffset != null)
            {
                queryStringDictionary["previousNtbsOffset"] = previousNtbsOffset.ToString();
                queryStringDictionary["previousLegacyOffset"] = previousLegacyOffset.ToString();
            }
            return queryStringDictionary;
        }

        private Dictionary<string, string> GetCurrentSearchParameters()
        {
            var queryString = Request.Query;
            var searchParameterDictionary = new Dictionary<string, string>();
            foreach (var key in queryString.Keys)
            {
                // Copy full query string over apart from any offset values
                if (!key.Contains("Offset"))
                {
                    searchParameterDictionary[key] = queryString[key].ToString();
                }
            }
            return searchParameterDictionary;
        }
    }
}
