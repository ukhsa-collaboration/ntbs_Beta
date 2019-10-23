using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
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

            await AuthorizeAndSetBannerAsync();

            NotificationId = Notification.NotificationId;
            await GetLinkedNotifications();

            if (Group == null)
            {
                return NotFound();
            }

            LinkedNotifications = Group.Notifications.Where(n => n.NotificationId != NotificationId)
                .Select(async n => NotificationBannerModel.WithLink(n, await authorizationService.CanEdit(User, n)))
                                                        .Select(t => t.Result).ToList();

            return Page();
        }
    }
}