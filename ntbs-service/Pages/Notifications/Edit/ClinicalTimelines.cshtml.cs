using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class ClinicalTimelineModel : ValidationModel
    {
        private readonly INotificationService service;

        public ClinicalTimelineModel(INotificationService service)
        {
            this.service = service;
        }

        [BindProperty]
        public ClinicalTimeline ClinicalTimeline { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }

        [BindProperty]
        public FormattedDate FormattedSymptomDate { get; set; }
        [BindProperty]
        public FormattedDate FormattedPresentationDate { get; set; }
        [BindProperty]
        public FormattedDate FormattedDiagnosisDate { get; set; }
        [BindProperty]
        public FormattedDate FormattedTreatmentDate { get; set; }
        [BindProperty]
        public FormattedDate FormattedDeathDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var notification = await service.GetNotificationAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            ClinicalTimeline = notification.ClinicalTimeline;
            NotificationId = notification.NotificationId;

            if (ClinicalTimeline == null) {
                ClinicalTimeline = new ClinicalTimeline();
            }

            FormattedSymptomDate = ClinicalTimeline.SymptomStartDate.ConvertToFormattedDate();
            FormattedPresentationDate = ClinicalTimeline.PresentationDate.ConvertToFormattedDate();
            FormattedDiagnosisDate = ClinicalTimeline.DiagnosisDate.ConvertToFormattedDate();
            FormattedTreatmentDate = ClinicalTimeline.TreatmentStartDate.ConvertToFormattedDate();
            FormattedDeathDate = ClinicalTimeline.DeathDate.ConvertToFormattedDate();

            return Page();
        }

        public async Task<IActionResult> OnPostPreviousPageAsync(int? NotificationId)
        {
            await validateAndSave(NotificationId);

            return RedirectToPage("/Patients/Edit", new {id = NotificationId});
        }

        public async Task<IActionResult> OnPostNextPageAsync(int? NotificationId)
        {
            await validateAndSave(NotificationId);

            return RedirectToPage("/ClinicalTimelines/Edit", new {id = NotificationId});
        }

        public async Task<bool> validateAndSave(int? NotificationId) {
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.SymptomStartDate), FormattedSymptomDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.PresentationDate), FormattedPresentationDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.DiagnosisDate), FormattedDiagnosisDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.TreatmentStartDate), FormattedTreatmentDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.DeathDate), FormattedDeathDate);

            if (!ModelState.IsValid)
            {
                await OnGetAsync(NotificationId);
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateTimelineAsync(notification, ClinicalTimeline);
            return true;
        }

        public ContentResult OnPostValidateClinicalTimelineDate(string key, string day, string month, string year)
        {
            return OnPostValidateDate(ClinicalTimeline, key, day, month, year);
        }
    }
}
