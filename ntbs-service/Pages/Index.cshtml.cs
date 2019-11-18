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
        private readonly IAlertRepository alertRepository;
        private readonly IAuthorizationService authorizationService;
        private readonly IUserService userService;

        public IndexModel(INotificationRepository notificationRepository, IAlertRepository alertRepository, 
            IAuthorizationService authorizationService, IUserService userService)
        {
            this.notificationRepository = notificationRepository;
            this.authorizationService = authorizationService;
            this.alertRepository = alertRepository;
            this.userService = userService;
        }

        public IList<Alert> Alerts { get; set; }
        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }

        public async Task OnGetAsync()
        {
            var draftNotificationsQueryable = await notificationRepository.GetDraftNotificationsAsync();
            var recentNotifications = await notificationRepository.GetRecentNotificationsAsync();
            DraftNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, draftNotificationsQueryable)).Take(10).ToList();
            RecentNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, recentNotifications)).Take(10).ToList();
            var services = await userService.GetTbServicesAsync(User);
            Alerts = await alertRepository.GetAlertsByTbServices(services.Select(x => x.Code));
        }
    }
}
