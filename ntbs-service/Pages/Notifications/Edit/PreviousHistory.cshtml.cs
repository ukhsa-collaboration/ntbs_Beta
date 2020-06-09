using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
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
        public PreviousTbHistory PreviousTbHistory { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            PreviousTbHistory = Notification.PreviousTbHistory;
            await SetNotificationProperties(isBeingSubmitted, PreviousTbHistory);

            if (PreviousTbHistory.ShouldValidateFull)
            {
                TryValidateModel(PreviousTbHistory, PreviousTbHistory.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./TreatmentEvents", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();
            PreviousTbHistory.SetValidationContext(Notification);
            PreviousTbHistory.DobYear = Notification.PatientDetails.Dob?.Year;

            TryValidateModel(PreviousTbHistory, nameof(PreviousTbHistory));
            
            if (ModelState.IsValid)
            {
                await Service.UpdatePreviousTbHistoryAsync(Notification, PreviousTbHistory);
            }
        }

        private void UpdateFlags()
        {
            if (PreviousTbHistory.PreviouslyHadTb != Status.Yes)
            {
                // TODO NTBS-1282 add rest of things to clear now 
                PreviousTbHistory.PreviousTbDiagnosisYear = null;
                ModelState.Remove("PreviousTbHistory.PreviousTbDiagnosisYear");
            }
        }

        public ContentResult OnGetValidatePreviousHistoryProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<PreviousTbHistory>(key, value, shouldValidateFull);
        }
    }
}
