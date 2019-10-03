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
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : PageModel
    {
        public INotificationService service;
        public int PageIndex;
        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;

        public IndexModel(INotificationService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {

            var pageSize = 50;

            var draftStatusList = new List<NotificationStatus>() {NotificationStatus.Draft};
            var nonDraftStatusList = new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified};
            IQueryable<Notification> draftsIQ = service.GetBaseQueryableNotificationByStatus(draftStatusList);
            IQueryable<Notification> nonDraftsIQ = service.GetBaseQueryableNotificationByStatus(nonDraftStatusList);

            var notificationsIQ = OrderQueryable(draftsIQ).Union(OrderQueryable(nonDraftsIQ));

            var notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResults = notifications.SelectItems(NotificationBannerModel.WithLink);

            SetPaginationDetails();

            return Page();
        }

        public IQueryable<Notification> OrderQueryable(IQueryable<Notification> query) {
            return query.OrderByDescending(n => n.CreationDate)
                .OrderByDescending(n => n.SubmissionDate)
                .OrderBy(n => n.NotificationStatus);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return Page();
        }

        public void SetPaginationDetails() {
            var queryString = Request.Query;
            var previousPageQueryString = new Dictionary<string, string>();
            foreach(var key in queryString.Keys) {
                previousPageQueryString[key] = queryString[key].ToString();
            }
            var nextPageQueryString = previousPageQueryString;
            if(SearchResults.HasPreviousPage) 
            {
                PreviousPageText = "Page " + (SearchResults.PageIndex - 1) + " of " + (SearchResults.TotalPages);
                previousPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex - 1);
                PreviousPageUrl = @Url.Action("Search", previousPageQueryString);
            }
            if(SearchResults.HasNextPage)
            {
                NextPageText = "Page " + (SearchResults.PageIndex + 1) + " of " + (SearchResults.TotalPages);
                nextPageQueryString["pageIndex"] = "" + (SearchResults.PageIndex + 1);
                NextPageUrl = QueryHelpers.AddQueryString("/Search", nextPageQueryString);
            }
        }
    }
}
