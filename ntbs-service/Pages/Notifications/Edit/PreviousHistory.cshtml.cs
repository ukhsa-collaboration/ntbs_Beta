using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class PreviousHistoryModel : NotificationEditModelBase
    {

        public PreviousHistoryModel(INotificationService service) : base(service) {}

        [BindProperty]
        public PatientTBHistory PatientTBHistory { get; set; }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            PatientTBHistory = Notification.PatientTBHistory;
            await SetNotificationProperties<PatientTBHistory>(isBeingSubmitted, PatientTBHistory);

            if (PatientTBHistory.ShouldValidateFull)
            {
                TryValidateModel(PatientTBHistory, PatientTBHistory.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        { 
            // This is the last page in the flow, so there's no next page to go to
            return RedirectToPage("./PreviousHistory", new { id = notificationId });
        }

        protected override async Task<bool> ValidateAndSave() {
            UpdateFlags();
            
            if (!ModelState.IsValid)
            {
                return false;
            }

            await service.UpdatePatientTBHistoryAsync(Notification, PatientTBHistory);
            return true;
        }
        
        private void UpdateFlags()
        {
            if (PatientTBHistory.NotPreviouslyHadTB ?? false) {
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