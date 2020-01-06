using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class OverviewModel : NotificationModelBase
    {
        protected IAlertRepository _alertRepository;
        private readonly ICultureAndResistanceService _cultureAndResistanceService;

        public CultureAndResistance CultureAndResistance { get; set; }
        public OverviewModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository,
            ICultureAndResistanceService cultureAndResistanceService)
            : base(service, authorizationService, notificationRepository)
        {
            _alertRepository = alertRepository;
            _cultureAndResistanceService = cultureAndResistanceService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationWithAllInfoAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            NotificationId = Notification.NotificationId;
            CultureAndResistance = await _cultureAndResistanceService.GetCultureAndResistanceDetailsAsync(NotificationId);

            await GetLinkedNotifications();
            await GetAlertsAsync();
            await AuthorizeAndSetBannerAsync();
            if (!HasEditPermission)
            {
                return Partial("./UnauthorizedWarning", this);
            }

            // This check has to happen after authorization as otherwise patient will redirect to overview and we'd be stuck in a loop.
            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                return RedirectToPage("./Edit/PatientDetails", new { NotificationId });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateLinkAsync()
        {
            var notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            var linkedNotification = await Service.CreateLinkedNotificationAsync(notification, User);

            return RedirectToPage("/Notifications/Edit/PatientDetails", new { linkedNotification.NotificationId });
        }

        public async Task GetAlertsAsync()
        {
            Alerts = await _alertRepository.GetAlertsForNotificationAsync(NotificationId);
        }
    }
}
