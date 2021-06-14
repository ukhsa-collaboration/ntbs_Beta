using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class OverviewModel : NotificationModelBase
    {
        private readonly IAlertService _alertService;
        private readonly ICultureAndResistanceService _cultureAndResistanceService;
        private readonly IAuditService _auditService;

        public CultureAndResistance CultureAndResistance { get; set; }
        public List<TreatmentPeriod> TreatmentPeriods { get; set; }

        public bool Should12MonthOutcomeBeDisplayed { get; set; }
        public bool Should24MonthOutcomeBeDisplayed { get; set; }
        public bool Should36MonthOutcomeBeDisplayed { get; set; }
        public TreatmentOutcome OutcomeAt12Months { get; set; }
        public TreatmentOutcome OutcomeAt24Months { get; set; }
        public TreatmentOutcome OutcomeAt36Months { get; set; }

        public OverviewModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IAlertService alertService,
            INotificationRepository notificationRepository,
            ICultureAndResistanceService cultureAndResistanceService,
            IAuditService auditService,
            IUserHelper userHelper) : base(service, authorizationService, userHelper, notificationRepository)
        {
            _alertService = alertService;
            _cultureAndResistanceService = cultureAndResistanceService;
            _auditService = auditService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            PrepareBreadcrumbs();

            Notification = await NotificationRepository.GetNotificationWithAllInfoAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            NotificationId = Notification.NotificationId;

            await GetLinkedNotificationsAsync();
            await GetAlertsAsync();
            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel == PermissionLevel.None ||
                (PermissionLevel == PermissionLevel.ReadOnly && Notification.NotificationStatus == NotificationStatus.Draft))
            {
                return Partial("./UnauthorizedWarning", this);
            }

            // This check has to happen after authorization as otherwise patient will redirect to overview and we'd be stuck in a loop.
            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                return RedirectToPage("./Edit/PatientDetails", new { NotificationId });
            }

            CultureAndResistance = await _cultureAndResistanceService.GetCultureAndResistanceDetailsAsync(NotificationId);
            TreatmentPeriods = Notification.TreatmentEvents.GroupEpisodesIntoPeriods();

            CalculateTreatmentOutcomes();
            return Page();
        }

        private void CalculateTreatmentOutcomes()
        {
            if (TreatmentOutcomesHelper.IsTreatmentOutcomeExpectedAtXYears(Notification, 1))
            {
                Should12MonthOutcomeBeDisplayed = true;
                OutcomeAt12Months = TreatmentOutcomesHelper.GetTreatmentOutcomeAtXYears(Notification, 1);
            }
            if (TreatmentOutcomesHelper.IsTreatmentOutcomeExpectedAtXYears(Notification, 2))
            {
                Should24MonthOutcomeBeDisplayed = true;
                OutcomeAt24Months = TreatmentOutcomesHelper.GetTreatmentOutcomeAtXYears(Notification, 2);
            }
            if (TreatmentOutcomesHelper.IsTreatmentOutcomeExpectedAtXYears(Notification, 3))
            {
                Should36MonthOutcomeBeDisplayed = true;
                OutcomeAt36Months = TreatmentOutcomesHelper.GetTreatmentOutcomeAtXYears(Notification, 3);
            }
        }

        public async Task<IActionResult> OnPostCreateLinkAsync()
        {
            if (UserIsReadOnly())
            {
                return Page();
            }
            var notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            var linkedNotification = await Service.CreateLinkedNotificationAsync(notification, User);

            return RedirectToPage("/Notifications/Edit/PatientDetails", new { linkedNotification.NotificationId });
        }

        public async Task GetAlertsAsync()
        {
            Alerts = await _alertService.GetAlertsForNotificationAsync(NotificationId, User);
        }
        
        public async Task<ContentResult> OnPostAuditPrintAsync()
        {
            await _auditService.AuditPrint(NotificationId, UserHelper.GetUsername(HttpContext));
            return new ContentResult();
        }
    }
}
