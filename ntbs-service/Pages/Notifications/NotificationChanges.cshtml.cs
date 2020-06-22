using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotificationChangesModel : NotificationModelBase
    {
        public NotificationChangesModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository)
            : base(service, authorizationService, notificationRepository)
        {
        }

        // ReSharper disable once UnusedMember.Global
        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel == PermissionLevel.None)
            {
                return RedirectToPage("/Notifications/Overview", new {NotificationId});
            }

            return Page();
        }
    }
}
