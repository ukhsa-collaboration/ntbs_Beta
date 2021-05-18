using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
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
        private readonly IUserService _userService;
        private readonly IUserHelper _userHelper;

        public ValidationService ValidationService;

        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string PreviousPageUrl;
        public PaginationParameters PaginationParameters;

        public List<Sex> Sexes { get; }
        public SelectList Countries { get; }
        public SelectList TbServices { get; }

        [BindProperty(SupportsGet = true)]
        public SearchParameters SearchParameters { get; set; }

        public IndexModel(
            INotificationRepository notificationRepository,
            ISearchService searchService,
            IAuthorizationService authorizationService,
            IReferenceDataRepository referenceDataRepository,
            ILegacySearchService legacySearchService,
            IUserService userService,
            IUserHelper userHelper)
        {
            _authorizationService = authorizationService;
            _searchService = searchService;
            _notificationRepository = notificationRepository;
            _legacySearchService = legacySearchService;
            _referenceDataRepository = referenceDataRepository;
            _userService = userService;
            _userHelper = userHelper;

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
            HttpContext.Session.SetTopLevelBreadcrumb("Search", HttpContext.Request.GetEncodedPathAndQuery());

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

            var ntbsQueryable = _notificationRepository.GetBaseNotificationsIQueryable();

            var ntbsFilteredSearchBuilder = (INtbsSearchBuilder)FilterBySearchParameters(new NtbsSearchBuilder(ntbsQueryable));
            var legacyFilteredSearchBuilder = (ILegacySearchBuilder)FilterBySearchParameters(new LegacySearchBuilder(_referenceDataRepository));

            var (notificationsToDisplay, count) = await SearchAsync(ntbsFilteredSearchBuilder, legacyFilteredSearchBuilder);
            await _authorizationService.SetFullAccessOnNotificationBannersAsync(notificationsToDisplay, User);
            SearchResults = new PaginatedList<NotificationBannerModel>(notificationsToDisplay, count, PaginationParameters);
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

        private async Task<(IList<NotificationBannerModel> results, int count)> SearchAsync(
            INtbsSearchBuilder ntbsQueryable,
            ILegacySearchBuilder legacySqlQuery)
        {
            IEnumerable<NotificationBannerModel> notificationsBannerModels;
            int count;
            if (PaginationParameters.LegacyOffset == null && PaginationParameters.NtbsOffset == null)
            {
                (notificationsBannerModels, count) = await SearchWithoutOffsetsAsync(ntbsQueryable, legacySqlQuery);
            }
            else
            {
                (notificationsBannerModels, count) = await SearchWithOffsetsAsync(ntbsQueryable, legacySqlQuery);
            }
            // notificationsToDisplay is ToList() to enumerate it so that the dynamic/notificationBannerModels from the migration database are mapped correctly
            // and we can successfully update properties on the models
            return (notificationsBannerModels.ToList(), count);
        }

        // Given no offsets from the previous page perform a search without using skip and take in SQL queries
        private async Task<(IEnumerable<NotificationBannerModel> results, int count)> SearchWithoutOffsetsAsync(
            INtbsSearchBuilder ntbsQueryable,
            ILegacySearchBuilder legacySqlQuery)
        {
            var numberOfNotificationsToFetch = PaginationParameters.PageSize * PaginationParameters.PageIndex;
            var (orderedNotificationIds, ntbsCount) = await _searchService.OrderAndPaginateQueryableAsync(
                ntbsQueryable,
                PaginationParameters,
                User);
            var ntbsNotifications = await _notificationRepository.GetNotificationBannerModelsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.SearchAsync(
                legacySqlQuery,
                0,
                numberOfNotificationsToFetch,
                User);
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var permittedTbServiceCodes = (await _userService.GetTbServicesAsync(User)).Select(s => s.Code);

            var notifications = allPossibleNotifications
                .OrderByDescending(n => permittedTbServiceCodes.Contains(n.TbServiceCode))
                .ThenByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.SortByDate)
                .ThenByDescending(n => n.NotificationId)
                .Skip(numberOfNotificationsToFetch - PaginationParameters.PageSize)
                .Take(PaginationParameters.PageSize);
            return (notifications, ntbsCount + legacyCount);
        }

        // Given offsets from the previous page perform a search with the correct skip and take to be efficient
        private async Task<(IEnumerable<NotificationBannerModel> results, int count)> SearchWithOffsetsAsync(
            INtbsSearchBuilder ntbsQueryable,
            ILegacySearchBuilder legacySqlQuery)
        {
            var (orderedNotificationIds, ntbsCount) = await _searchService.OrderAndPaginateQueryableAsync(
                ntbsQueryable,
                PaginationParameters,
                User);
            var ntbsNotifications = await _notificationRepository.GetNotificationBannerModelsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.SearchAsync(
                legacySqlQuery,
                (int)PaginationParameters.LegacyOffset,
                PaginationParameters.PageSize,
                User);
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var permittedTbServiceCodes = (await _userService.GetTbServicesAsync(User)).Select(s => s.Code);

            var notifications = allPossibleNotifications
                .OrderByDescending(n => permittedTbServiceCodes.Contains(n.TbServiceCode))
                .ThenByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
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

        public bool userIsReadOnly()
        {
            return _userHelper.CurrentUserIsReadOnly(HttpContext);
        }
    }
}
