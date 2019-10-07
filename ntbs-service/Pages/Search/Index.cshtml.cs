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

            var pageSize = 50;

            var draftStatusList = new List<NotificationStatus>() {NotificationStatus.Draft};
            var nonDraftStatusList = new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified};
            IQueryable<Notification> draftsIQ = service.GetBaseQueryableNotificationByStatus(draftStatusList);
            IQueryable<Notification> nonDraftsIQ = service.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            if (!String.IsNullOrEmpty(SearchParameters.IdFilter))
            {
                SearchParamsExist = true;
                draftsIQ = service.FilterById(draftsIQ, SearchParameters.IdFilter);
                nonDraftsIQ = service.FilterById(nonDraftsIQ, SearchParameters.IdFilter);
            }

            IQueryable<Notification> notificationsIQ = service.OrderQueryableByNotificationDate(draftsIQ).Union(service.OrderQueryableByNotificationDate(nonDraftsIQ));

            var notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResults = notifications.SelectItems(NotificationBannerModel.WithLink);

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
