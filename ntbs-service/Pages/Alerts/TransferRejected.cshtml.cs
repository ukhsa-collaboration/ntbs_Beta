using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Alerts
{
    public class TransferRejectedModel : NotificationModelBase
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAlertService _alertService;
        public ValidationService ValidationService;

        [BindProperty]
        public int AlertId { get; set; }
        public TransferRejectedAlert TransferRejectedAlert { get; set; }


        public TransferRejectedModel(
            INotificationService notificationService,
            IAlertService alertService, 
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(notificationService, authorizationService, notificationRepository)
        {
            _alertService = alertService;
            _alertRepository = alertRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            TransferRejectedAlert =
                await _alertRepository.GetOpenAlertByNotificationId<TransferRejectedAlert>(NotificationId);
            await AuthorizeAndSetBannerAsync();
            
            // Check edit permission of user and redirect if they don't have permission or the alert does not exist
            var (permissionLevel, _) = await _authorizationService.GetPermissionLevelAsync(User, Notification);
            if (permissionLevel != PermissionLevel.Edit || TransferRejectedAlert == null)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TransferRejectedAlert = 
                await _alertRepository.GetOpenAlertByNotificationId<TransferRejectedAlert>(NotificationId);
            await _alertService.DismissAlertAsync(TransferRejectedAlert.AlertId, User.FindFirstValue(ClaimTypes.Upn));
            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }
    }
}
