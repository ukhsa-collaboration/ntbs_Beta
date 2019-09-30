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
        public PaginatedList<Notification> Notifications { get; set; }
        public IList<NotificationBannerModel> SearchResultsBannerDisplay;

        public IndexModel(INotificationService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {

            var pageSize = 50;

            PageIndex = pageIndex ?? 1;

            IQueryable<Notification> notificationsIQ = service.GetBaseNotificationIQueryable();

            Notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResultsBannerDisplay = new List<NotificationBannerModel>();
            foreach(Notification result in Notifications) {
                SearchResultsBannerDisplay.Add(new NotificationBannerModel(result, true));
            }

            return Page();
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
