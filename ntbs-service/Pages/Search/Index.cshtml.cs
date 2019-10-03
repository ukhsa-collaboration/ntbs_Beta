using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;
using ntbs_service.Pages;
using ntbs_service.Models.Enums;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : ValidationModel
    {
        public INotificationService service;
        public int PageIndex;
        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResults;
        public string NextPageUrl;
        public string NextPageText;
        public string PreviousPageUrl;
        public string PreviousPageText;

        [RegularExpression(@"[0-9]+", ErrorMessage = "This can only contain digits 0-9")]
        [BindProperty(SupportsGet = true)]
        public string IdFilter { get; set; }
        public bool? DisplayCreateNotification { get; set; }

        public IndexModel(INotificationService service)
        {
            this.service = service;
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

            if (!String.IsNullOrEmpty(IdFilter))
            {
                DisplayCreateNotification = true;
                draftsIQ = FilterById(draftsIQ, IdFilter);
                nonDraftsIQ = FilterById(nonDraftsIQ, IdFilter);
            }

            IQueryable<Notification> notificationsIQ = OrderQueryable(draftsIQ).Union(OrderQueryable(nonDraftsIQ));

            var notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResults = notifications.SelectItems(NotificationBannerModel.WithLink);

            SetPaginationDetails();

            return Page();
        }

        public IQueryable<Notification> OrderQueryable(IQueryable<Notification> query) {
            return query.OrderByDescending(n => n.CreationDate)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public IQueryable<Notification> FilterById(IQueryable<Notification> IQ, string IdFilter) {
            return IQ.Where(s => s.NotificationId.Equals(Int32.Parse(IdFilter)) 
                    || s.ETSID.Equals(IdFilter) || s.LTBRID.Equals(IdFilter) || s.PatientDetails.NhsNumber.Equals(IdFilter));
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidateProperty(new IndexModel(service), key, value);
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
                PreviousPageUrl = @Url.Action("Search", previousPageQueryString);
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
