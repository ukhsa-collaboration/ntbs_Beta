using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    [BindProperties]
    public class ClinicalDetailsModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext context;

        public ClinicalDetails ClinicalDetails { get; set; }
        public int NotificationId { get; set; }

        public Dictionary<SiteId, bool> NotificationSiteMap { get; set; }

        public List<Site> Sites { get; set; }
        // We want to bind to the full model rather than a string for SiteDescription so we can validate against the data annotation
        public NotificationSite OtherSite { get; set; }

        public int? PatientBirthYear { get; set; }

        public FormattedDate FormattedSymptomDate { get; set; }
        public FormattedDate FormattedPresentationDate { get; set; }
        public FormattedDate FormattedDiagnosisDate { get; set; }
        public FormattedDate FormattedTreatmentDate { get; set; }
        public FormattedDate FormattedDeathDate { get; set; }

        public ClinicalDetailsModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            this.context = context;
        }

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
            SetupNotificationSiteMap(notificationSites);
            OtherSite = notificationSites.FirstOrDefault(ns => ns.SiteId == (int)SiteId.OTHER);
            Sites = (await context.GetAllSitesAsync()).ToList();

            PatientBirthYear = notification.PatientDetails.Dob?.Year;

            FormattedSymptomDate = ClinicalDetails.SymptomStartDate.ConvertToFormattedDate();
            FormattedPresentationDate = ClinicalDetails.PresentationDate.ConvertToFormattedDate();
            FormattedDiagnosisDate = ClinicalDetails.DiagnosisDate.ConvertToFormattedDate();
            FormattedTreatmentDate = ClinicalDetails.TreatmentStartDate.ConvertToFormattedDate();
            FormattedDeathDate = ClinicalDetails.DeathDate.ConvertToFormattedDate();

            return Page();
        }

        private void SetupNotificationSiteMap(IEnumerable<NotificationSite> notificationSites)
        {
            NotificationSiteMap = new Dictionary<SiteId, bool>();
            foreach (SiteId siteId in Enum.GetValues(typeof(SiteId)))
            {
                NotificationSiteMap.Add(siteId, notificationSites.FirstOrDefault(ns => ns.SiteId == (int)siteId) != null);
            }
        }


        public async Task<IActionResult> OnPostAsync(int? NotificationId)
        {
            bool validModel = await validateAndSave(NotificationId);

            if(!validModel) {
                return await OnGetAsync(NotificationId);
            }

            return RedirectToPage("./ContactTracing", new {id = NotificationId});
        }

        private async Task<bool> validateAndSave(int? NotificationId) {
            UpdateFlags();

            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.SymptomStartDate), FormattedSymptomDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.PresentationDate), FormattedPresentationDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DiagnosisDate), FormattedDiagnosisDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.TreatmentStartDate), FormattedTreatmentDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DeathDate), FormattedDeathDate);
            ValidateYearComparisonOnModel(ClinicalDetails, nameof(ClinicalDetails.BCGVaccinationYear),
                ClinicalDetails.BCGVaccinationYear, PatientBirthYear);

            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateTimelineAsync(notification, ClinicalDetails);
            await service.UpdateSitesAsync(notification, CreateNotificationSitesFromModel(notification));

            return true;
        }

        private void UpdateFlags() {
            if (ClinicalDetails.DidNotStartTreatment) {
                ClinicalDetails.TreatmentStartDate = null;
                FormattedTreatmentDate = ClinicalDetails.TreatmentStartDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.TreatmentStartDate");
            }
            
            if (!ClinicalDetails.IsPostMortem) {
                ClinicalDetails.DeathDate = null;
                FormattedDeathDate = ClinicalDetails.DeathDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.DeathDate");
            }

            if (ClinicalDetails.BCGVaccinationState != Status.Yes) {
                ClinicalDetails.BCGVaccinationYear = null;
                ModelState.Remove("ClinicalDetails.BCGVaccinationYear");
            }
            
            if (!NotificationSiteMap[SiteId.OTHER]) {
                OtherSite.SiteDescription = null;
                ModelState.Remove("OtherSite.SiteDescription");
            }

           
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

        public ContentResult OnGetValidateClinicalDetailsProperty(string key, string value)
        {
            return ValidateProperty(new ClinicalDetails(), key, value);
        }
        public ContentResult OnGetValidateClinicalDetailsDate(string key, string day, string month, string year)
        {
            return ValidateDate(new ClinicalDetails(), key, day, month, year);
        }

        public ContentResult OnGetValidateNotificationSiteProperty(string key, string value)
        {
            return ValidateProperty(new NotificationSite(), key, value);
        }

        public ContentResult OnGetValidateClinicalDetailsYearComparison(int newYear, int existingYear)
        {
            return ValidateYearComparison(newYear, existingYear);
        }
    }
}
