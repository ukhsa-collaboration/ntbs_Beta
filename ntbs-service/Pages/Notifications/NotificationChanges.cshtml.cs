using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using ntbs_service.TagHelpers;

namespace ntbs_service.Pages.Notifications
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class NotificationChangesModel : NotificationModelBase
    {
        public IEnumerable<NotificationHistoryListItemModel> Changes { get; private set; }

        private readonly INotificationChangesService _notificationChangesService;

        public NotificationChangesModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            INotificationChangesService notificationChangesService)
            : base(service, authorizationService, notificationRepository)
        {
            _notificationChangesService = notificationChangesService;
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

            Changes = await _notificationChangesService.GetChangesList(NotificationId);

            return Page();
        }
    }
}
