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

namespace ntbs_service.Pages_Search
{
    public class IndexModel : ValidationModel
    {
        public INotificationService service;
        public int PageIndex;
        public PaginatedList<NotificationBannerModel> SearchResultsBannerDisplay;

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

            IQueryable<Notification> draftsIQ = service.GetBaseNotificationIQueryableByNotificationStatus(new List<NotificationStatus>() {NotificationStatus.Draft});
            IQueryable<Notification> nonDraftsIQ = service.GetBaseNotificationIQueryableByNotificationStatus(new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified});

            if (!String.IsNullOrEmpty(IdFilter))
            {
                DisplayCreateNotification = true;
                draftsIQ = FilterById(draftsIQ, IdFilter);
                nonDraftsIQ = FilterById(nonDraftsIQ, IdFilter);
            }

            IQueryable<Notification> notificationsIQ = OrderQueryable(draftsIQ).Union(OrderQueryable(nonDraftsIQ));

            var notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResultsBannerDisplay = notifications.SelectItems(NotificationBannerModel.WithLink);

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
    }
}
