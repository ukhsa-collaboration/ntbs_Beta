using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
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
            TransferRejectedAlert = (TransferRejectedAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRejected);
            await AuthorizeAndSetBannerAsync();
            
            // Check edit permission of user and redirect if they don't have permission
            if (!HasEditPermission)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }
            
            if (TransferRejectedAlert == null)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TransferRejectedAlert = (TransferRejectedAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRejected);
            await _alertService.DismissAlertAsync(TransferRejectedAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));
            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }
    }
}