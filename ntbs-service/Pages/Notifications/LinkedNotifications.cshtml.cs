using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class LinkedNotificationsModel : NotificationModelBase
    {
        public List<NotificationBannerModel> LinkedNotifications { get; set; }

        public LinkedNotificationsModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService) {}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await service.GetNotificationAsync(id);
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

            LinkedNotifications = Group.Notifications
                .Where(n => n.NotificationId != NotificationId)
                .CreateNotificationBanners(User, authorizationService).ToList();

            return Page();
        }
    }
}
