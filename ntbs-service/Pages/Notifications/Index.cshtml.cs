using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Models;
using ntbs_service.DataAccess;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
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
