using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class OverviewModel : NotificationModelBase
    {
        public OverviewModel(INotificationService service) : base(service)
        {
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await service.GetNotificationWithAllInfoAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;
            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                return RedirectToPage("./Edit/Patient", new { id = NotificationId });
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);

            await GetLinkedNotifications();

            return Page();
        }

        public async Task<IActionResult> OnPostCreateLinkAsync()
        {
            var notification = await service.GetNotificationAsync(NotificationId);
            var linkedNotification = await service.CreateLinkedNotificationAsync(notification);

            return RedirectToPage("/Notifications/Edit/Patient", new { id = linkedNotification.NotificationId });
        }
    }
}