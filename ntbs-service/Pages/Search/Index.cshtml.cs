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
        public PaginationParameters PaginationParameters;

        [BindProperty(SupportsGet = true)]
        public SearchParameters SearchParameters { get; set; }
        
        public bool? SearchParamsExist { get; set; }

        public IndexModel(INotificationService notificationService, ISearchService searchService, NtbsContext context)
        {
            this.context = context;
            this.searchService = searchService;
            this.notificationService = notificationService;
            validationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            Sexes = context.GetAllSexesAsync().Result.ToList();
            if(!ModelState.IsValid) 
            {
                return Page();
            }

            PaginationParameters = new PaginationParameters() {PageSize = 50, PageIndex = pageIndex ?? 1};


            var draftStatusList = new List<NotificationStatus>() {NotificationStatus.Draft};
            var nonDraftStatusList = new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified};
            IQueryable<Notification> draftsQueryable = notificationService.GetBaseQueryableNotificationByStatus(draftStatusList);
            IQueryable<Notification> nonDraftsQueryable = notificationService.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            if (!string.IsNullOrEmpty(SearchParameters.IdFilter))
            {
                SearchParamsExist = true;
                draftsQueryable = searchService.FilterById(draftsQueryable, SearchParameters.IdFilter);
                nonDraftsQueryable = searchService.FilterById(nonDraftsQueryable, SearchParameters.IdFilter);
            }

            if (SearchParameters.SexId != null)
            {
                SearchParamsExist = true;
                draftsQueryable = searchService.FilterBySex(draftsQueryable, (int) SearchParameters.SexId);
                nonDraftsQueryable = searchService.FilterBySex(nonDraftsQueryable, (int) SearchParameters.SexId);
            }

            if (SearchParameters.PartialDob != null && SearchParameters.PartialDob.Year != null)
            {
                SearchParamsExist = true;
                SearchParameters.PartialDob.TryConvertToDateTimeRange(out DateTime? start, out DateTime? end);

                draftsQueryable = searchService.FilterByPartialDate(draftsQueryable, SearchParameters.PartialDob);
                nonDraftsQueryable = searchService.FilterByPartialDate(nonDraftsQueryable, SearchParameters.PartialDob);
            }

            IQueryable<Notification> notificationIdsQueryable = searchService.OrderQueryableByNotificationDate(draftsQueryable)
                                                                .Union(searchService.OrderQueryableByNotificationDate(nonDraftsQueryable));

            var notificationIds = await searchService.GetPaginatedItemsAsync(notificationIdsQueryable.Select(n => n.NotificationId), PaginationParameters);
            var count = await notificationIdsQueryable.CountAsync();
            IEnumerable<Notification> notifications = await notificationService.GetNotificationsByIdAsync(notificationIds);
            var notificationBannerModels = notifications.Select(NotificationBannerModel.WithLink);
            SearchResults = new PaginatedList<NotificationBannerModel>(notificationBannerModels, count, PaginationParameters);

            SetPaginationDetails();

            return Page();
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return validationService.ValidateProperty(new IndexModel(notificationService, searchService, context), key, value);
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
