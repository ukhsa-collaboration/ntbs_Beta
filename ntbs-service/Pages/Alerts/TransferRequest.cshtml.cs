using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Alerts
{
    public class TransferRequestModel : NotificationModelBase
    {
        private readonly IAlertRepository alertRepository;
        private readonly IAlertService alertService;
        private readonly IAuthorizationService authorizationService;
        public ValidationService ValidationService { get; set; }

        [BindProperty]
        public TransferAlert TransferAlert { get; set; }

        [BindProperty]
        public int AlertId { get; set; }

        public TransferRequestModel(
            INotificationService notificationService,
            IAlertService alertService, 
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(notificationService, authorizationService, notificationRepository)
        {
            this.alertService = alertService;
            this.alertRepository = alertRepository;
            this.authorizationService = authorizationService;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            await AuthorizeAndSetBannerAsync();
            // Get alert if it exists

            // Check edit permission and redirect if not allowed
            // if (!HasEditPermission || Notification.NotificationStatus != NotificationStatus.Notified)
            // {
            //     return RedirectToPage("/Notifications/Overview", new { NotificationId });
            // }

            TransferAlert = new TransferAlert();

            return Page();
        }

        // public async Task<IActionResult> OnPostConfirmAsync()
        // {
        //     Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
        //     if (!(await AuthorizationService.CanEditNotificationAsync(User, Notification)))
        //     {
        //         return ForbiddenResult();
        //     }

        //     DenotificationDetails.DateOfNotification = Notification.NotificationDate;
        //     ValidationService.TrySetFormattedDate(DenotificationDetails,
        //                                           "DenotificationDetails",
        //                                           nameof(DenotificationDetails.DateOfDenotification),
        //                                           FormattedDenotificationDate);
        //     TryValidateModel(DenotificationDetails, "DenotificationDetails");
        //     if (!ModelState.IsValid)
        //     {
        //         NotificationBannerModel = new NotificationBannerModel(Notification);
        //         return Page();
        //     }

        //     if (Notification.NotificationStatus == NotificationStatus.Notified)
        //     {
        //         await Service.DenotifyNotificationAsync(NotificationId, DenotificationDetails);
        //     }

        //     return RedirectToPage("/Notifications/Overview", new { NotificationId });
        // }

    }
}