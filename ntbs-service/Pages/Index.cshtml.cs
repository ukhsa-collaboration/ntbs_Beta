using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.DataAccess;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public int AlertId { get; set; }
        public IList<Alert> Alerts { get; set; }
        public IList<Notification> DraftNotifications { get;set; }
        public IList<Notification> RecentNotifications { get;set; }
        public SelectList TbServices { get; set; }

        public async Task OnGetAsync()
        {
            await SetUserNotificationsAsync();
            await SetUserAlertsAsync();
        }

        private async Task SetUserNotificationsAsync()
        {
            var draftNotificationsQueryable = notificationRepository.GetDraftNotificationsIQueryable();
            var recentNotificationsQueryable = notificationRepository.GetRecentNotificationsIQueryable();
            DraftNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, draftNotificationsQueryable)).Take(10).ToList();
            RecentNotifications = (await authorizationService.FilterNotificationsByUserAsync(User, recentNotificationsQueryable)).Take(10).ToList();
        }

        private async Task SetUserAlertsAsync()
        {
            var services = await userService.GetTbServicesAsync(User);
            var tbServiceCodes = services.Select(s => s.Code);
            TbServices = new SelectList(services, nameof(TBService.Code), nameof(TBService.Name));
            Alerts = await alertRepository.GetAlertsByTbServiceCodesAsync(tbServiceCodes);
        }
    }
}
