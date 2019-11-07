using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;
using System;
using System.Threading.Tasks;
using ntbs_service.DataAccess;

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

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await NotificationRepository.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;

            await AuthorizeAndSetBannerAsync();
            if (!HasEditPermission || Notification.NotificationStatus != NotificationStatus.Notified)
            {
                return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
            }

            await GetLinkedNotifications();

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (!(await AuthorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            DenotificationDetails.DateOfNotification = Notification.NotificationDate;
            ValidationService.TrySetAndValidateDateOnModel(DenotificationDetails, nameof(DenotificationDetails.DateOfDenotification), FormattedDenotificationDate);
            if (!ModelState.IsValid)
            {
                NotificationBannerModel = new NotificationBannerModel(Notification);
                return Page();
            }

            if (Notification.NotificationStatus == NotificationStatus.Notified)
            {
                await Service.DenotifyNotificationAsync(NotificationId, DenotificationDetails);
            }

            return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
        }
    }
}
