using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.Models;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : PageModel
    {
        public INotificationService service;
        public int PageIndex;
        public string CurrentFilter { get; set; }
        public PaginatedList<NotificationBannerModel> SearchResultsBannerDisplay;

        public IndexModel(INotificationService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {

            var pageSize = 50;

            IQueryable<Notification> notificationsIQ = service.GetBaseNotificationIQueryable();
            // IQueryable<Notification> draftsIQ = service.GetBaseNotificationIQueryable(Drafts);

            notificationsIQ = OrderQueryable(notificationsIQ);

            var notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResultsBannerDisplay = notifications.SelectItems(NotificationBannerModel.WithLink);

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
    }
}
