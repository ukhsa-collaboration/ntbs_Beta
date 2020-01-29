using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PreviousHistoryModel : NotificationEditModelBase
    {
        public PreviousHistoryModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository) { }

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

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            string nextPage;
            if(Notification.ClinicalDetails?.IsMDRTreatment == true) // TODO NTBS-368 drug resistance profile check - should probably abstract check into method
            {
                nextPage = "./MDRDetails";
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

            if (TryValidateModel(PatientTbHistory.GetType().Name))
            {
                await Service.UpdatePatientTbHistoryAsync(Notification, PatientTbHistory);
            }
        }

        private void UpdateFlags()
        {
            if (PatientTbHistory.NotPreviouslyHadTB == true)
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
