using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;
using System;
using System.Threading.Tasks;

namespace ntbs_service.Pages.Notifications
{
    public class DenotifyModel : NotificationModelBase
    {
        public ValidationService  ValidationService { get; set; }

        [BindProperty]
        public DenotificationDetails DenotificationDetails { get; set; }

        [BindProperty]
        public FormattedDate FormattedDenotificationDate { get; set; }

        public DenotifyModel(INotificationService service) : base(service)
        {
            ValidationService = new ValidationService(this);

            if (FormattedDenotificationDate == null)
            {
                FormattedDenotificationDate = FormattedDate.Today();
            }
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;
            if (Notification.NotificationStatus != NotificationStatus.Notified)
            {
                return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);

            await GetLinkedNotifications();

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            Notification = await service.GetNotificationAsync(NotificationId);
            DenotificationDetails.DateOfNotification = Notification.NotificationDate;
            ValidationService.TrySetAndValidateDateOnModel(DenotificationDetails, nameof(DenotificationDetails.DateOfDenotification), FormattedDenotificationDate);
            if (!ModelState.IsValid)
            {
                NotificationBannerModel = new NotificationBannerModel(Notification);
                return Page();
            }

            if (Notification.NotificationStatus == NotificationStatus.Notified)
            {
                await service.DenotifyNotification(NotificationId, DenotificationDetails);
            }

            return RedirectToPage("/Notifications/Overview", new { id = NotificationId });
        }
    }
}