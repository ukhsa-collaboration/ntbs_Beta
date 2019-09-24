using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace ntbs_service.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationService service;

        public IndexModel(INotificationService service)
        {
            this.service = service;
        }

        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }

        public async Task OnGetAsync()
        {
            DraftNotifications = await service.GetDraftNotificationsAsync();
            RecentNotifications = await service.GetRecentNotificationsAsync();
        }
    }
}
