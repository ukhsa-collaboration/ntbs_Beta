using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class PreviousHistoryModel : NotificationEditModelBase
    {

        public PreviousHistoryModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService) {}

        [BindProperty]
        public PatientTBHistory PatientTBHistory { get; set; }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();
            if (!HasEditPermission)
            {
                return RedirectToOverview(id);
            }

            PatientTBHistory = Notification.PatientTBHistory;
            await SetNotificationProperties(isBeingSubmitted, PatientTBHistory);

            if (PatientTBHistory.ShouldValidateFull)
            {
                TryValidateModel(PatientTBHistory, PatientTBHistory.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            // This is the last page in the flow, so there's no next page to go to
            return RedirectToPage("./PreviousHistory", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave()
        {
            UpdateFlags();
            
            PatientTBHistory.SetFullValidation(Notification.NotificationStatus);
            if (!TryValidateModel(this))
            {
                return false;
            }

            await service.UpdatePatientTBHistoryAsync(Notification, PatientTBHistory);
            return true;
        }

        private void UpdateFlags()
        {
            if (PatientTBHistory.NotPreviouslyHadTB == true)
            {
                PatientTBHistory.PreviousTBDiagnosisYear = null;
                ModelState.Remove("PatientTBHistory.PreviousTBDiagnosisYear");
            }
        }

        public ContentResult OnGetValidatePreviousHistoryProperty(string key, string value, bool shouldValidateFull)
        {
            return validationService.ValidateModelProperty<PatientTBHistory>(key, value, shouldValidateFull);
        }
    }
}