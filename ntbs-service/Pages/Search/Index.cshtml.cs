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
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Search;
using ntbs_service.Services;

namespace ntbs_service.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ISearchService _searchService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ILegacySearchService _legacySearchService;

        public ValidationService ValidationService;

        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;
        public int Count;
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
            this._authorizationService = authorizationService;
            this._searchService = searchService;
            this._notificationRepository = notificationRepository;
            this._legacySearchService = legacySearchService;

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

        public async Task<IActionResult> OnGetAsync(int? pageIndex, int? legacyOffset, int? ntbsOffset, int? previousLegacyOffset, int? previousNtbsOffset)
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


            var draftStatusList = new List<NotificationStatus>() { NotificationStatus.Draft };
            var nonDraftStatusList = new List<NotificationStatus>() { NotificationStatus.Notified, NotificationStatus.Denotified };
            var draftsQueryable = _notificationRepository.GetBaseQueryableNotificationByStatus(draftStatusList);
            var nonDraftsQueryable = _notificationRepository.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            var draftSearchBuilder = new NotificationSearchBuilder(draftsQueryable);
            var nonDraftsSearchBuilder = new NotificationSearchBuilder(nonDraftsQueryable);

            var filteredDrafts = draftSearchBuilder
                .FilterById(SearchParameters.IdFilter)
                .FilterByFamilyName(SearchParameters.FamilyName)
                .FilterByGivenName(SearchParameters.GivenName)
                .FilterByPartialDob(SearchParameters.PartialDob)
                .FilterByPostcode(SearchParameters.Postcode)
                .FilterByPartialNotificationDate(SearchParameters.PartialNotificationDate)
                .FilterBySex(SearchParameters.SexId)
                .FilterByBirthCountry(SearchParameters.CountryId)
                .FilterByTBService(SearchParameters.TBServiceCode)
                .GetResult();

            var filteredNonDrafts = nonDraftsSearchBuilder
                .FilterById(SearchParameters.IdFilter)
                .FilterByFamilyName(SearchParameters.FamilyName)
                .FilterByGivenName(SearchParameters.GivenName)
                .FilterByPartialDob(SearchParameters.PartialDob)
                .FilterByPostcode(SearchParameters.Postcode)
                .FilterByPartialNotificationDate(SearchParameters.PartialNotificationDate)
                .FilterBySex(SearchParameters.SexId)
                .FilterByBirthCountry(SearchParameters.CountryId)
                .FilterByTBService(SearchParameters.TBServiceCode)
                .GetResult();

            IEnumerable<NotificationBannerModel> notificationsToDisplay;

            if(PaginationParameters.LegacyOffset == null && PaginationParameters.NtbsOffset == null)
            {
                notificationsToDisplay = await SearchWithoutOffsets(filteredDrafts, filteredNonDrafts);
            }
            else 
            {
                notificationsToDisplay = await SearchWithOffsets(filteredDrafts, filteredNonDrafts);
            }

            int? nextLegacyOffset, nextNtbsOffset;
            if(ntbsOffset != null && legacyOffset != null || pageIndex == 1)
            {
                var legacyNotificationsDisplayed = notificationsToDisplay.Count(n => n.NotificationStatus == NotificationStatus.Legacy);
                var ntbsNotificationsDisplayed = PaginationParameters.PageSize - legacyNotificationsDisplayed;
                nextNtbsOffset = ntbsNotificationsDisplayed + ntbsOffset ?? 0;
                nextLegacyOffset = legacyNotificationsDisplayed + legacyOffset ?? 0;
            }
            else
            {
                nextNtbsOffset = null;
                nextLegacyOffset = null;
            }

            // TODO NTBS-263 AUTHORISE banners properly
            SearchResults = new PaginatedList<NotificationBannerModel>(notificationsToDisplay, Count, PaginationParameters);

            SetPaginationDetails(nextNtbsOffset, nextLegacyOffset, previousNtbsOffset, previousLegacyOffset, ntbsOffset, legacyOffset);

            return Page();
        }

        public async Task<IEnumerable<NotificationBannerModel>> SearchWithoutOffsets(IQueryable<Notification> filteredDrafts, IQueryable<Notification> filteredNonDrafts)
        {
            var numberOfNotificationsToFetch = PaginationParameters.PageSize * PaginationParameters.PageIndex;
            var (orderedNotificationIds, count) = await _searchService.OrderAndPaginateQueryables(filteredDrafts, filteredNonDrafts, PaginationParameters);
            var ntbsNotifications = await _notificationRepository.GetNotificationBannerModelsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.Search(0, numberOfNotificationsToFetch);
            Count = legacyCount + count;
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var notifications = allPossibleNotifications
                .OrderByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.SortByDate)
                .ThenByDescending(n => n.NotificationId)
                .Skip(numberOfNotificationsToFetch - PaginationParameters.PageSize)
                .Take(PaginationParameters.PageSize);
            return notifications;
        }

        public async Task<IEnumerable<NotificationBannerModel>> SearchWithOffsets(IQueryable<Notification> filteredDrafts, IQueryable<Notification> filteredNonDrafts)
        {
            var (orderedNotificationIds, count) = await _searchService.OrderAndPaginateQueryables(filteredDrafts, filteredNonDrafts, PaginationParameters);
            var ntbsNotifications = await _notificationRepository.GetNotificationBannerModelsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.Search((int)PaginationParameters.LegacyOffset, PaginationParameters.PageSize);
            Count = legacyCount + count;
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var notifications = allPossibleNotifications
                .OrderByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.SortByDate)
                .ThenByDescending(n => n.NotificationId)
                .Take(PaginationParameters.PageSize);
            return notifications;  
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidationService.ValidateProperty(this, key, value);
        }

        public void SetPaginationDetails(int? nextNtbsOffset,
                                         int? nextLegacyOffset,
                                         int? previousNtbsOffset,
                                         int? previousLegacyOffset,
                                         int? ntbsOffset,
                                         int? legacyOffset)
        {
            if (SearchResults.HasPreviousPage)
            {
                var previousPageParameters = CreateSearchPageParameters(SearchResults.PageIndex - 1, previousNtbsOffset, previousLegacyOffset);
                // TODO move these to the SearchResults model
                PreviousPageText = "Page " + (SearchResults.PageIndex - 1) + " of " + (SearchResults.TotalPages);
                PreviousPageUrl = QueryHelpers.AddQueryString("/Search", previousPageParameters);
            }
            if (SearchResults.HasNextPage)
            {
                var nextPageParameters = CreateSearchPageParameters(SearchResults.PageIndex + 1, nextNtbsOffset, nextLegacyOffset, ntbsOffset, legacyOffset);
                NextPageText = "Page " + (SearchResults.PageIndex + 1) + " of " + (SearchResults.TotalPages);
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageParameters);
            }
        }

        private Dictionary<string, string> CreateSearchPageParameters(int pageIndex,
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
                if(!key.Contains("Offset"))
                {
                    searchParameterDictionary[key] = queryString[key].ToString();
                }
            }
            return searchParameterDictionary;
        }
    }
}
