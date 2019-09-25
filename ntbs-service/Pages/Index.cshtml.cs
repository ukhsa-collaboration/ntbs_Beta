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
            // Using a dummy TB service here while we don't have user profiles
            DraftNotifications = await service.GetDraftNotificationsAsync(new List<string> {"Ashford Hospital"});
            RecentNotifications = await service.GetRecentNotificationsAsync(new List<string> {"Ashford Hospital"});
        }
    }
}
