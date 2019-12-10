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

        public ValidationService ValidationService;

        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;
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
            IReferenceDataRepository referenceDataRepository)
        {
            this._authorizationService = authorizationService;
            this._searchService = searchService;
            this._notificationRepository = notificationRepository;

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

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PaginationParameters = new PaginationParameters() { PageSize = 50, PageIndex = pageIndex ?? 1 };


            var draftStatusList = new List<NotificationStatus>() { NotificationStatus.Draft };
            var nonDraftStatusList = new List<NotificationStatus>() { NotificationStatus.Notified, NotificationStatus.Denotified };
            var draftsQueryable = _notificationRepository.GetQueryableNotificationByStatus(draftStatusList);
            var nonDraftsQueryable = _notificationRepository.GetQueryableNotificationByStatus(nonDraftStatusList);

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

            var (orderedNotificationIds, count) = await _searchService.OrderAndPaginateQueryables(filteredDrafts, filteredNonDrafts, PaginationParameters);
            var notifications = await _notificationRepository.GetNotificationsByIdsAsync(orderedNotificationIds);
            var orderedNotifications = notifications.OrderBy(d => orderedNotificationIds.IndexOf(d.NotificationId)).ToList();
            var notificationBannerModels = orderedNotifications.CreateNotificationBanners(User, _authorizationService);
            SearchResults = new PaginatedList<NotificationBannerModel>(notificationBannerModels, count, PaginationParameters);

            SetPaginationDetails();

            return Page();
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidationService.ValidateProperty(this, key, value);
        }

        public void SetPaginationDetails()
        {
            var queryString = Request.Query;
            var previousPageQueryString = new Dictionary<string, string>();
            foreach (var key in queryString.Keys)
            {
                previousPageQueryString[key] = queryString[key].ToString();
            }
            var nextPageQueryString = previousPageQueryString;
            if (SearchResults?.HasPreviousPage ?? false)
            {
                PreviousPageText = "Page " + (SearchResults.PageIndex - 1) + " of " + (SearchResults.TotalPages);
                previousPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex - 1);
                PreviousPageUrl = QueryHelpers.AddQueryString("/Search", previousPageQueryString);
            }
            if (SearchResults?.HasNextPage ?? false)
            {
                NextPageText = "Page " + (SearchResults.PageIndex + 1) + " of " + (SearchResults.TotalPages);
                nextPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex + 1);
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageQueryString);
            }
        }
    }
}
