using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;

namespace ntbs_service.Pages_ClinicalTimelines
{
    public class EditModel : ValidationModel
    {
        private readonly INotificationRepository _repository;

        public EditModel(INotificationRepository repository)
        {
            _repository = repository;
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
            var notification = await _repository.GetNotificationAsync(1);

            ClinicalTimeline = notification.ClinicalTimeline;
            NotificationId = 1;

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

        public async Task<IActionResult> OnPostAsync()
        {
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.SymptomStartDate), FormattedSymptomDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.PresentationDate), FormattedPresentationDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.DiagnosisDate), FormattedDiagnosisDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.TreatmentStartDate), FormattedTreatmentDate);
            SetAndValidateDate(ClinicalTimeline, nameof(ClinicalTimeline.DeathDate), FormattedDeathDate);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var notification = await _repository.GetNotificationAsync(NotificationId);
            await _repository.UpdateTimelineAsync(notification, ClinicalTimeline);

            return RedirectToPage("/Patients/Index");
        }

        public ContentResult OnPostValidateClinicalTimelineDate(string key, string day, string month, string year)
        {
            return OnPostValidateDate(ClinicalTimeline, key, day, month, year);
        }
    }
}
