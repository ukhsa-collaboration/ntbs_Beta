using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class DenotifyModel : NotificationModelBase
    {
        public ValidationService ValidationService { get; set; }

        [BindProperty]
        public DenotificationDetails DenotificationDetails { get; set; }

        [BindProperty]
        public FormattedDate FormattedDenotificationDate { get; set; }

        public DenotifyModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
            ValidationService = new ValidationService(this);

            if (FormattedDenotificationDate == null)
            {
                var now = DateTime.Now;
                FormattedDenotificationDate = new FormattedDate()
                {
                    Day = now.Day.ToString(),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString()
                };
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel != PermissionLevel.Edit || Notification.NotificationStatus != NotificationStatus.Notified)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            await GetLinkedNotificationsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            
            var (permissionLevel, _) = await _authorizationService.GetPermissionLevelAsync(User, Notification);
            if (permissionLevel != PermissionLevel.Edit)
            {
                return ForbiddenResult();
            }

            DenotificationDetails.DateOfNotification = Notification.NotificationDate;
            ValidationService.TrySetFormattedDate(DenotificationDetails,
                                                  "DenotificationDetails",
                                                  nameof(DenotificationDetails.DateOfDenotification),
                                                  FormattedDenotificationDate);
            TryValidateModel(DenotificationDetails, "DenotificationDetails");
            if (!ModelState.IsValid)
            {
                NotificationBannerModel = new NotificationBannerModel(Notification);
                return Page();
            }

            if (Notification.NotificationStatus == NotificationStatus.Notified)
            {
                await Service.DenotifyNotificationAsync(
                    NotificationId, 
                    DenotificationDetails,
                    User.FindFirstValue(ClaimTypes.Email));
            }

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }
    }
}
