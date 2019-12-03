using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PreviousHistoryModel : NotificationEditModelBase
    {
        public PreviousHistoryModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository) : base(service, authorizationService, alertRepository, notificationRepository) { }

        [BindProperty]
        public PatientTBHistory PatientTbHistory { get; set; }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            PatientTbHistory = Notification.PatientTBHistory;
            await SetNotificationProperties(isBeingSubmitted, PatientTbHistory);

            if (PatientTbHistory.ShouldValidateFull)
            {
                TryValidateModel(PatientTbHistory, PatientTbHistory.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./MDRDetails", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();
            PatientTbHistory.SetFullValidation(Notification.NotificationStatus);

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
            return ValidationService.ValidateModelProperty<PatientTBHistory>(key, value, shouldValidateFull);
        }
    }
}
