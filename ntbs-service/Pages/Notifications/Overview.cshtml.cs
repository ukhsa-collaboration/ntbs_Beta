using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class OverviewModel : NotificationModelBase
    {
        protected IAlertService _alertService;
        private readonly ICultureAndResistanceService _cultureAndResistanceService;
        
        public CultureAndResistance CultureAndResistance { get; set; }
        public Dictionary<int, List<TreatmentEvent>> GroupedTreatmentEvents { get; set; }
        
        public TreatmentOutcome OutcomeAt12Month { get; set; }
        public TreatmentOutcome OutcomeAt24Month { get; set; }
        public TreatmentOutcome OutcomeAt36Month { get; set; }
        
        public OverviewModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IAlertService alertService,
            INotificationRepository notificationRepository,
            ICultureAndResistanceService cultureAndResistanceService) : base(service, authorizationService, notificationRepository)
        {
            _alertService = alertService;
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

            await GetLinkedNotifications();
            await GetAlertsAsync();
            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel == PermissionLevel.None)
            {
                return Partial("./UnauthorizedWarning", this);
            }

            // This check has to happen after authorization as otherwise patient will redirect to overview and we'd be stuck in a loop.
            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                return RedirectToPage("./Edit/PatientDetails", new { NotificationId });
            }

            CultureAndResistance = await _cultureAndResistanceService.GetCultureAndResistanceDetailsAsync(NotificationId);
            GroupedTreatmentEvents = Notification.TreatmentEvents.GroupByEpisode();

            CalculateOutcomeTypes();
            return Page();
        }

        private void CalculateOutcomeTypes()
        {
            if ( Notification.NotificationDate == null)
            {
                return;
            }

            var notificationDate = Notification.NotificationDate.Value;
            var treatmentEvent12Month = 
                Notification.TreatmentEvents.GetMostRecentTreatmentOutcomeInPeriod(notificationDate,
                    notificationDate.AddMonths(12));
            
            OutcomeAt12Month = treatmentEvent12Month?.EventDate < DateTime.Today.AddMonths(-12) ? treatmentEvent12Month.TreatmentOutcome : null;
            if (OutcomeAt12Month?.TreatmentOutcomeType != TreatmentOutcomeType.NotEvaluated 
                || OutcomeAt12Month?.TreatmentOutcomeSubType == TreatmentOutcomeSubType.TransferredAbroad)
            {
                return;
            }          
            
            var treatmentEvent24Month = Notification.TreatmentEvents.GetMostRecentTreatmentOutcomeInPeriod(
                notificationDate.AddMonths(12), 
                notificationDate.AddMonths(24));
            
            OutcomeAt24Month = treatmentEvent24Month?.EventDate < DateTime.Today.AddMonths(-24) ? treatmentEvent24Month?.TreatmentOutcome : null;
            if (OutcomeAt24Month?.TreatmentOutcomeType != TreatmentOutcomeType.NotEvaluated
                || OutcomeAt24Month?.TreatmentOutcomeSubType == TreatmentOutcomeSubType.TransferredAbroad)
            {
                return;
            }
            
            var treatmentEvent36Month = Notification.TreatmentEvents.GetMostRecentTreatmentOutcomeInPeriod(
                notificationDate.AddMonths(24),
                notificationDate.AddMonths(36));
            
            OutcomeAt36Month = treatmentEvent36Month?.EventDate < DateTime.Today.AddMonths(-36) ? treatmentEvent36Month?.TreatmentOutcome : null;
        }

        public async Task<IActionResult> OnPostCreateLinkAsync()
        {
            var notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            var linkedNotification = await Service.CreateLinkedNotificationAsync(notification, User);

            return RedirectToPage("/Notifications/Edit/PatientDetails", new { linkedNotification.NotificationId });
        }

        public async Task GetAlertsAsync()
        {
            Alerts = await _alertService.GetAlertsForNotificationAsync(NotificationId, User);
        }
    }
}
