using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.DataAccess;
using System.Linq;

namespace ntbs_service.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationRepository notificationRepository;
        private readonly IAuthorizationService authorizationService;

        public IndexModel(INotificationRepository notificationRepository, IAuthorizationService authorizationService)
        {
            this.notificationRepository = notificationRepository;
            this.authorizationService = authorizationService;
        }

        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }

        public async Task OnGetAsync()
        {
            var draftNotificationsQueryable = await notificationRepository.GetDraftNotificationsAsync();
            var recentNotifications = await notificationRepository.GetRecentNotificationsAsync();
            DraftNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, draftNotificationsQueryable)).Take(10).ToList();
            RecentNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, recentNotifications)).Take(10).ToList();
        }
    }
}
