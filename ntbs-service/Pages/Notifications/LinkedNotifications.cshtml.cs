using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class LinkedNotificationsModel : NotificationModelBase
    {
        public List<NotificationBannerModel> LinkedNotifications { get; set; }

        public LinkedNotificationsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository) : base(service, authorizationService, alertRepository, notificationRepository)
        {
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await NotificationRepository.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;

            var hasLinkedNotifications = await TryGetLinkedNotifications();
            if (!hasLinkedNotifications)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();

            // Deleted notifications should have their group ID removed so they should not appear here
            LinkedNotifications = Group.Notifications
                .Where(n => n.NotificationId != NotificationId)
                .CreateNotificationBanners(User, AuthorizationService).ToList();

            return Page();
        }
    }
}
