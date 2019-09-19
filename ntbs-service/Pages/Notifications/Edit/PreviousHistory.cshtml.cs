using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class PreviousHistoryModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly IAuditService auditService;

        public PreviousHistoryModel(INotificationService service, IAuditService auditService)
        {
            this.service = service;
            this.auditService = auditService;
        }

        [BindProperty]
        public PatientTBHistory PatientTBHistory { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var notification = await service.GetNotificationAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            NotificationId = notification.NotificationId;
            PatientTBHistory = notification.PatientTBHistory;

            if (PatientTBHistory == null) {
                PatientTBHistory = new PatientTBHistory();
            }

            await auditService.OnGetAuditAsync(notification.NotificationId, PatientTBHistory);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? NotificationId)
        {
            bool validModel = await validateAndSave(NotificationId);

            if (!validModel) {
                return await OnGetAsync(NotificationId);
            }

            return RedirectToPage("../Index", new {id = NotificationId});
        }

        public async Task<bool> validateAndSave(int? NotificationId) {
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