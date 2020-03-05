using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PreviousHistoryModel : NotificationEditModelBase
    {
        public PreviousHistoryModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository) : base(service, authorizationService, notificationRepository, alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditPreviousHistory;
        }

        [BindProperty]
        public PatientTBHistory PatientTbHistory { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            PatientTbHistory = Notification.PatientTBHistory;
            await SetNotificationProperties(isBeingSubmitted, PatientTbHistory);

            if (PatientTbHistory.ShouldValidateFull)
            {
                TryValidateModel(PatientTbHistory, PatientTbHistory.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            string nextPage;
            if (Notification.IsMdr)
            {
                nextPage = "./MDRDetails";
            }
            else if (Notification.IsMBovis)
            {
                nextPage = "./MBovisExposureToKnownCases";
            }
            else
            {
                nextPage = "./PreviousHistory";
            }
            return RedirectToPage(nextPage, new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();
            PatientTbHistory.SetValidationContext(Notification);
            PatientTbHistory.DobYear = Notification.PatientDetails.Dob?.Year;

            TryValidateModel(PatientTbHistory, nameof(PatientTbHistory));
            
            if (ModelState.IsValid)
            {
                await Service.UpdatePatientTbHistoryAsync(Notification, PatientTbHistory);
            }
        }

        private void UpdateFlags()
        {
            if (PatientTbHistory.PreviouslyHadTB == false)
            {
                PatientTbHistory.PreviousTBDiagnosisYear = null;
                ModelState.Remove("PatientTBHistory.PreviousTBDiagnosisYear");
            }
        }

        public ContentResult OnGetValidatePreviousHistoryProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<PatientTBHistory>(key, value, shouldValidateFull);
        }
    }
}
