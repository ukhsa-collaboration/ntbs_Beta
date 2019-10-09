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
        public ValidationService validationService;
        public INotificationService service;
        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;
        public PaginationParameters PaginationParameters;

        [BindProperty(SupportsGet = true)]
        public SearchParameters SearchParameters { get; set; }
        public bool? SearchParamsExist { get; set; }

        public IndexModel(INotificationService service)
        {
            this.service = service;
            validationService = new ValidationService(this);
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
            IQueryable<Notification> draftsQueryable = service.GetBaseQueryableNotificationByStatus(draftStatusList);
            IQueryable<Notification> nonDraftsQueryable = service.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            if (!String.IsNullOrEmpty(SearchParameters.IdFilter))
            {
                SearchParamsExist = true;
                draftsQueryable = service.FilterById(draftsQueryable, SearchParameters.IdFilter);
                nonDraftsQueryable = service.FilterById(nonDraftsQueryable, SearchParameters.IdFilter);
            }

            IQueryable<Notification> notificationIdsQueryable = service.OrderQueryableByNotificationDate(draftsQueryable)
                                                                .Union(service.OrderQueryableByNotificationDate(nonDraftsQueryable));

            var notificationIds = await service.GetPaginatedIdsAsync(notificationIdsQueryable.Select(n => n.NotificationId), PaginationParameters);
            var count = await notificationIdsQueryable.CountAsync();
            IEnumerable<Notification> notifications = await service.GetNotificationsByIdAsync(notificationIds);
            SearchResults = new PaginatedList<NotificationBannerModel>(notifications.Select(n => NotificationBannerModel.WithLink(n)), count, PaginationParameters);

            SetPaginationDetails();

            return Page();
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return validationService.ValidateProperty(new IndexModel(service), key, value);
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
