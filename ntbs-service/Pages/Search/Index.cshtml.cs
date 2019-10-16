using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.Models;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : PageModel
    {
        private readonly NtbsContext context;
        public ValidationService validationService;
        public INotificationService notificationService;
        public ISearchService searchService;
        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;
        public List<Sex> Sexes { get; set; }
        public SelectList Countries { get; set; }
        public SelectList TBServices { get; set; }
        public PaginationParameters PaginationParameters;

        [BindProperty(SupportsGet = true)]
        public SearchParameters SearchParameters { get; set; }

        public IndexModel(INotificationService notificationService, ISearchService searchService, NtbsContext context)
        {
            this.context = context;
            this.searchService = searchService;
            this.notificationService = notificationService;
            validationService = new ValidationService(this);
            Sexes = context.GetAllSexesAsync().Result.ToList();
            TBServices = new SelectList(context.GetAllTbServicesAsync().Result,
                                        nameof(TBService.Code),
                                        nameof(TBService.Name));
            Countries = new SelectList(context.GetAllCountriesAsync().Result,
                                        nameof(Country.CountryId),
                                        nameof(Country.Name));
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            if(!ModelState.IsValid) 
            {
                return Page();
            }

            PaginationParameters = new PaginationParameters() {PageSize = 50, PageIndex = pageIndex ?? 1};


            var draftStatusList = new List<NotificationStatus>() {NotificationStatus.Draft};
            var nonDraftStatusList = new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified};
            IQueryable<Notification> draftsQueryable = notificationService.GetBaseQueryableNotificationByStatus(draftStatusList);
            IQueryable<Notification> nonDraftsQueryable = notificationService.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            NotificationSearchBuilder draftSearchBuilder = new NotificationSearchBuilder(draftsQueryable);
            NotificationSearchBuilder nonDraftsSearchBuilder = new NotificationSearchBuilder(nonDraftsQueryable);

            var filteredDrafts = draftSearchBuilder
                .FilterById(SearchParameters.IdFilter)
                .FilterByFamilyName(SearchParameters.FamilyName)
                .FilterByGivenName(SearchParameters.GivenName)
                .FilterByPartialDob(SearchParameters.PartialDob)
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
                .FilterByPartialNotificationDate(SearchParameters.PartialNotificationDate)
                .FilterBySex(SearchParameters.SexId)
                .FilterByBirthCountry(SearchParameters.CountryId)
                .FilterByTBService(SearchParameters.TBServiceCode)
                .GetResult();

            Tuple<IList<int>, int> notificationIdsAndCount = await searchService.UnionAndPaginateQueryables(filteredDrafts, filteredNonDrafts, PaginationParameters);
            var notificationIds = notificationIdsAndCount.Item1;
            var count = notificationIdsAndCount.Item2;
            
            IEnumerable<Notification> notifications = await notificationService.GetNotificationsByIdAsync(notificationIds);
            var notificationBannerModels = notifications.Select(NotificationBannerModel.WithLink);
            SearchResults = new PaginatedList<NotificationBannerModel>(notificationBannerModels, count, PaginationParameters);

            SetPaginationDetails();

            return Page();
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return validationService.ValidateProperty(this, key, value);
        }

        public void SetPaginationDetails() {
            var queryString = Request.Query;
            var previousPageQueryString = new Dictionary<string, string>();
            foreach(var key in queryString.Keys) {
                previousPageQueryString[key] = queryString[key].ToString();
            }
            var nextPageQueryString = previousPageQueryString;
            if(SearchResults?.HasPreviousPage ?? false) 
            {
                PreviousPageText = "Page " + (SearchResults.PageIndex - 1) + " of " + (SearchResults.TotalPages);
                previousPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex - 1);
                PreviousPageUrl = QueryHelpers.AddQueryString("/Search", previousPageQueryString);
            }
            if(SearchResults?.HasNextPage ?? false)
            {
                NextPageText = "Page " + (SearchResults.PageIndex + 1) + " of " + (SearchResults.TotalPages);
                nextPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex + 1);
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageQueryString);
            }
        }
    }
}
