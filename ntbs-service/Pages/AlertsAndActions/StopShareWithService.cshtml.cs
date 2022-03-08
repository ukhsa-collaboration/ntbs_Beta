using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.AlertsAndActions
{
    public class StopShareWithServiceModel : NotificationModelBase
    {
        public StopShareWithServiceModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IUserHelper userHelper)
            : base(notificationService, authorizationService, userHelper, notificationRepository)
        {
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await SetNotificationAndAuthorize();
            // Check edit permission and redirect if not allowed
            if (PermissionLevel != PermissionLevel.Edit)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await SetNotificationAndAuthorize();
            if (PermissionLevel != PermissionLevel.Edit)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            await Service.StopSharingNotificationWithService(Notification);

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        private async Task SetNotificationAndAuthorize()
        {
            Notification = await GetNotificationAsync(NotificationId);
            await AuthorizeAndSetBannerAsync();
        }
    }
}
