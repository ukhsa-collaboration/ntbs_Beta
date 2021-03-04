
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
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            var hasLinkedNotifications = await TryGetLinkedNotificationsAsync();
            if (!hasLinkedNotifications)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();

            // Deleted notifications should have their group ID removed so they should not appear here
            LinkedNotifications = Notification.Group.Notifications
                .Where(n => n.NotificationId != NotificationId)
                .CreateNotificationBanners(User, _authorizationService).ToList();

            PrepareBreadcrumbs();

            return Page();
        }
    }
}
