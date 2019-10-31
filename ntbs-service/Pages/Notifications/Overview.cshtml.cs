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
        public OverviewModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService) {}

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await service.GetNotificationWithAllInfoAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;
            await GetLinkedNotifications();

            await AuthorizeAndSetBannerAsync();
            if (!HasEditPermission)
            {
                return Partial("./UnauthorizedWarning", this);
            }

            // This check has to happen after authorization as otherwise patient will redirect to overview and we'd be stuck in a loop.
            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                return RedirectToPage("./Edit/Patient", new { id = NotificationId });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateLinkAsync()
        {
            var notification = await service.GetNotificationAsync(NotificationId);
            var linkedNotification = await service.CreateLinkedNotificationAsync(notification, User);

            return RedirectToPage("/Notifications/Edit/Patient", new { id = linkedNotification.NotificationId });
        }
    }
}