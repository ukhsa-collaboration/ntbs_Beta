using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace ntbs_service.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationService notificationService;
        private readonly IUserService userService;

        public IndexModel(INotificationService notificationService, IUserService userService)
        {
            this.notificationService = notificationService;
            this.userService = userService;
        }

        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }

        public async Task OnGetAsync()
        {
            var tbServices = await userService.GetTbServicesAsync(User);
            DraftNotifications = await notificationService.GetDraftNotificationsAsync(tbServices);
            RecentNotifications = await notificationService.GetRecentNotificationsAsync(tbServices);
        }
    }
}
