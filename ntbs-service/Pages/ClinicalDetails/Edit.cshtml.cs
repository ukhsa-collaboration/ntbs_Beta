using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_ClinicalDetails
{
    public class EditModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext _context;

        public EditModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            _context = context;
        }

        [BindProperty]
        public ClinicalDetails ClinicalDetails { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }

        [BindProperty]
        public Dictionary<SiteId, bool> NotificationSiteMap { get; set; }
        [BindProperty]
        public List<Site> Sites { get; set; }
        [BindProperty]
        public NotificationSite OtherSite { get; set; }

        [BindProperty]
        public int? PatientBirthYear { get; set; }

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

            ClinicalDetails = notification.ClinicalDetails;
            NotificationId = notification.NotificationId;

            if (ClinicalDetails == null) {
                ClinicalDetails = new ClinicalDetails();
            }

            var notificationSites = notification.NotificationSites;
            OtherSite = notificationSites.FirstOrDefault(ns => ns.SiteId == (int)SiteId.OTHER);
            Sites = (await _context.GetAllSitesAsync()).ToList();
            NotificationSiteMap = CreateNotificationSiteMap(notificationSites, Sites);

            PatientBirthYear = notification.PatientDetails.Dob?.Year;

            FormattedSymptomDate = ClinicalDetails.SymptomStartDate.ConvertToFormattedDate();
            FormattedPresentationDate = ClinicalDetails.PresentationDate.ConvertToFormattedDate();
            FormattedDiagnosisDate = ClinicalDetails.DiagnosisDate.ConvertToFormattedDate();
            FormattedTreatmentDate = ClinicalDetails.TreatmentStartDate.ConvertToFormattedDate();
            FormattedDeathDate = ClinicalDetails.DeathDate.ConvertToFormattedDate();

            return Page();
        }

        private Dictionary<SiteId, bool> CreateNotificationSiteMap(IEnumerable<NotificationSite> notificationSites, IEnumerable<Site> sites)
        {
            var dict = new Dictionary<SiteId, bool>();
            foreach (var site in sites)
            {
                dict.Add((SiteId)site.SiteId, notificationSites.FirstOrDefault(ns => ns.SiteId == site.SiteId) != null);
            }
            return dict;
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            SetAndValidateDate(ClinicalDetails, nameof(ClinicalDetails.SymptomStartDate), FormattedSymptomDate);
            SetAndValidateDate(ClinicalDetails, nameof(ClinicalDetails.PresentationDate), FormattedPresentationDate);
            SetAndValidateDate(ClinicalDetails, nameof(ClinicalDetails.DiagnosisDate), FormattedDiagnosisDate);
            SetAndValidateDate(ClinicalDetails, nameof(ClinicalDetails.TreatmentStartDate), FormattedTreatmentDate);
            SetAndValidateDate(ClinicalDetails, nameof(ClinicalDetails.DeathDate), FormattedDeathDate);
            ValidateYearAgainstOtherYear(ClinicalDetails, nameof(ClinicalDetails.BCGVaccinationYear), ClinicalDetails.BCGVaccinationYear, PatientBirthYear);

            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id);
            }

            var notification = await service.GetNotificationAsync(id);
            await service.UpdateTimelineAsync(notification, ClinicalDetails);
            await service.UpdateSitesAsync(notification, CreateNotificationSitesFromModel(notification));

            return RedirectToPage("/Patients/Index");
        }

        private IEnumerable<NotificationSite> CreateNotificationSitesFromModel(Notification notification)
        {
            foreach (var item in NotificationSiteMap) {
                if (item.Value) 
                {
                    yield return new NotificationSite
                    {
                        NotificationId = notification.NotificationId,
                        SiteId = (int)item.Key,
                        SiteDescription = item.Key == SiteId.OTHER ? OtherSite.SiteDescription : null
                    };
                }
            }
        }
    }
}
