using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class PreviousHistoryModel : NotificationModelBase
    {
        private readonly IAuditService auditService;

        public PreviousHistoryModel(INotificationService service, IAuditService auditService) : base(service)
        {
            this.auditService = auditService;
        }

        [BindProperty]
        public PatientTBHistory PatientTBHistory { get; set; }

        public override async Task<IActionResult> OnGetAsync(int? id)
        {
            var notification = await service.GetNotificationAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            NotificationId = notification.NotificationId;
            NotificationStatus = notification.NotificationStatus;
            PatientTBHistory = notification.PatientTBHistory;

            if (PatientTBHistory == null) {
                PatientTBHistory = new PatientTBHistory();
            }

            await auditService.OnGetAuditAsync(notification.NotificationId, PatientTBHistory);
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        { 
            return RedirectToPage("../Index");
        }

        protected override async Task<bool> ValidateAndSave(int? NotificationId) {
            UpdateFlags();
            
            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdatePatientTBHistoryAsync(notification, PatientTBHistory);
            return true;
        }
        
        private void UpdateFlags()
        {
            if (PatientTBHistory.NotPreviouslyHadTB) {
                PatientTBHistory.PreviousTBDiagnosisYear = null;
                ModelState.Remove("PatientTBHistory.PreviousTBDiagnosisYear");
            }
        }

        public ContentResult OnGetValidatePreviousHistoryProperty(string key, int value)
        {
            return ValidateProperty(new PatientTBHistory(), key, value);
        }
    }
}