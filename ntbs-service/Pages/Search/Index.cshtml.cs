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

            IEnumerable<Notification> notificationsToDisplay;

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


            var notificationBannerModels = notificationsToDisplay.CreateNotificationBanners(User, _authorizationService);
            SearchResults = new PaginatedList<NotificationBannerModel>(notificationBannerModels, Count, PaginationParameters);

            SetPaginationDetails(nextNtbsOffset, nextLegacyOffset, previousNtbsOffset, previousLegacyOffset, ntbsOffset, legacyOffset);

            return Page();
        }

        public async Task<IEnumerable<Notification>> SearchWithoutOffsets(IQueryable<Notification> filteredDrafts, IQueryable<Notification> filteredNonDrafts)
        {
            var numberOfNotificationsToFetch = PaginationParameters.PageSize * PaginationParameters.PageIndex;
            var (orderedNotificationIds, count) = await _searchService.OrderAndPaginateQueryables(filteredDrafts, filteredNonDrafts, PaginationParameters);
            var ntbsNotifications = await _notificationRepository.GetNotificationsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.Search(0, numberOfNotificationsToFetch);
            Count = legacyCount + count;
            var allPossibleNotifications = ntbsNotifications.Concat(legacyNotifications);
            var notifications = allPossibleNotifications
                .OrderByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.NotificationDate ?? n.CreationDate)
                .ThenByDescending(n => n.NotificationId)
                .ThenBy(n => n.LTBRID ?? n.ETSID)
                .Skip(numberOfNotificationsToFetch - PaginationParameters.PageSize)
                .Take(PaginationParameters.PageSize);
            return notifications;
        }

        public async Task<IEnumerable<Notification>> SearchWithOffsets(IQueryable<Notification> filteredDrafts, IQueryable<Notification> filteredNonDrafts)
        {
            var (orderedNotificationIds, count) = await _searchService.OrderAndPaginateQueryables(filteredDrafts, filteredNonDrafts, PaginationParameters);
            var notifications = await _notificationRepository.GetNotificationsByIdsAsync(orderedNotificationIds);
            var (legacyNotifications, legacyCount) = await _legacySearchService.Search((int)PaginationParameters.LegacyOffset, PaginationParameters.PageSize);
            Count = legacyCount + count;
            var allPossibleNotifications = notifications.Concat(legacyNotifications);
            var notificationss = allPossibleNotifications
                .OrderByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.NotificationDate ?? n.CreationDate)
                .ThenByDescending(n => n.NotificationId)
                .ThenBy(n => n.LTBRID ?? n.ETSID)
                .Take(PaginationParameters.PageSize);
            return notificationss;  
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidationService.ValidateProperty(this, key, value);
        }

        public void SetPaginationDetails(int? nextNtbsOffset, int? nextLegacyOffset, 
            int? previousNtbsOffset, int? previousLegacyOffset, int? ntbsOffset, int? legacyOffset)
        {
            var queryString = Request.Query;
            var previousPageQueryString = new Dictionary<string, string>();
            foreach (var key in queryString.Keys)
            {
                // Copy full query string over apart from any offset values
                if(!key.Contains("Offset"))
                {
                    previousPageQueryString[key] = queryString[key].ToString();
                }
            }
            var nextPageQueryString = previousPageQueryString;
            if (SearchResults?.HasPreviousPage ?? false)
            {
                PreviousPageText = "Page " + (SearchResults.PageIndex - 1) + " of " + (SearchResults.TotalPages);
                previousPageQueryString["pageIndex"] = (SearchResults.PageIndex - 1).ToString();
                if (previousNtbsOffset != null && previousLegacyOffset != null) 
                {
                    previousPageQueryString["ntbsOffset"] = previousNtbsOffset.ToString();
                    previousPageQueryString["legacyOffset"] = previousLegacyOffset.ToString();
                }
                PreviousPageUrl = QueryHelpers.AddQueryString("/Search", previousPageQueryString);
            }
            if (SearchResults?.HasNextPage ?? false)
            {
                NextPageText = "Page " + (SearchResults.PageIndex + 1) + " of " + (SearchResults.TotalPages);
                nextPageQueryString["pageIndex"] = (SearchResults.PageIndex + 1).ToString();
                if(nextNtbsOffset != null && nextLegacyOffset != null)
                {
                    nextPageQueryString["legacyOffset"] = nextLegacyOffset.ToString();
                    nextPageQueryString["ntbsOffset"] = nextNtbsOffset.ToString();
                }
                if (ntbsOffset != null && legacyOffset != null) 
                {
                    nextPageQueryString["previousNtbsOffset"] = ntbsOffset.ToString();
                    nextPageQueryString["previousLegacyOffset"] = legacyOffset.ToString();
                }
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageQueryString);
            }
        }
    }
}
