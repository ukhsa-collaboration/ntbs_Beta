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
    public class ClinicalDetailsModel : NotificationModelBase
    {
        private readonly NtbsContext context;
        private readonly IAuditService auditService;

        public ClinicalDetails ClinicalDetails { get; set; }

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
        public FormattedDate FormattedMDRTreatmentDate { get; set; }

        public ClinicalDetailsModel(INotificationService service, NtbsContext context, IAuditService auditService) : base(service)
        {
            this.context = context;
            this.auditService = auditService;
        }

        public override async Task<IActionResult> OnGetAsync(int? id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationWithNotificationSitesAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            ClinicalDetails = Notification.ClinicalDetails;
            if (ClinicalDetails == null) {
                ClinicalDetails = new ClinicalDetails();
            }

            SetNotificationProperties<ClinicalDetails>(isBeingSubmitted, ClinicalDetails);

            var notificationSites = Notification.NotificationSites;
            SetupNotificationSiteMap(notificationSites);
            OtherSite = notificationSites.FirstOrDefault(ns => ns.SiteId == (int)SiteId.OTHER);
            Sites = (await context.GetAllSitesAsync()).ToList();

            PatientBirthYear = Notification.PatientDetails.Dob?.Year;

            FormattedSymptomDate = ClinicalDetails.SymptomStartDate.ConvertToFormattedDate();
            FormattedPresentationDate = ClinicalDetails.PresentationDate.ConvertToFormattedDate();
            FormattedDiagnosisDate = ClinicalDetails.DiagnosisDate.ConvertToFormattedDate();
            FormattedTreatmentDate = ClinicalDetails.TreatmentStartDate.ConvertToFormattedDate();
            FormattedDeathDate = ClinicalDetails.DeathDate.ConvertToFormattedDate();
            FormattedMDRTreatmentDate = ClinicalDetails.MDRTreatmentStartDate.ConvertToFormattedDate();


            if (ClinicalDetails.ShouldValidateFull)
            {
                TryValidateModel(this);
            }
            await auditService.OnGetAuditAsync(Notification.NotificationId, ClinicalDetails);
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

        protected override IActionResult RedirectToNextPage(int? notificationId) {
            return RedirectToPage("./ContactTracing", new {id = notificationId});
        } 

        protected override async Task<bool> ValidateAndSave(int NotificationId) {
            UpdateFlags();

            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.SymptomStartDate), FormattedSymptomDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.PresentationDate), FormattedPresentationDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DiagnosisDate), FormattedDiagnosisDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.TreatmentStartDate), FormattedTreatmentDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DeathDate), FormattedDeathDate);
            SetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.MDRTreatmentStartDate), FormattedMDRTreatmentDate);
            ValidateYearComparisonOnModel(ClinicalDetails, nameof(ClinicalDetails.BCGVaccinationYear),
                ClinicalDetails.BCGVaccinationYear, PatientBirthYear);
            
            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateClinicalDetailsAsync(notification, ClinicalDetails);
            await service.UpdateSitesAsync(notification, CreateNotificationSitesFromModel(NotificationId));

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

            if (!ClinicalDetails.IsMDRTreatment) {
                ClinicalDetails.MDRTreatmentStartDate = null;
                FormattedMDRTreatmentDate = ClinicalDetails.MDRTreatmentStartDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.MDRTreatmentStartDate");
            }
            
            if (!NotificationSiteMap[SiteId.OTHER]) {
                OtherSite.SiteDescription = null;
                ModelState.Remove("OtherSite.SiteDescription");
            }
        }

        private IEnumerable<NotificationSite> CreateNotificationSitesFromModel(int notificationId)
        {
            foreach (var item in NotificationSiteMap) {
                if (item.Value) 
                {
                    yield return new NotificationSite
                    {
                        NotificationId = notificationId,
                        SiteId = (int)item.Key,
                        SiteDescription = item.Key == SiteId.OTHER ? OtherSite.SiteDescription : null
                    };
                }
            }
        }

        public ContentResult OnGetValidateClinicalDetailsProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidateModelProperty<ClinicalDetails>(key, value, shouldValidateFull);
        }

        public ContentResult OnGetValidateClinicalDetailsListProperty(string key, IEnumerable<string> valueList, bool shouldValidateFull)
        {
            List<NotificationSite> notificationSites = new List<NotificationSite>();
            foreach (string value in valueList)
            {
                notificationSites.Add(new NotificationSite {
                    SiteId = (int) ((SiteId) Enum.Parse(typeof(SiteId), value))
                });
            }
            
            return ValidateModelProperty<Notification>(key, notificationSites, shouldValidateFull);
        }

        public ContentResult OnGetValidateClinicalDetailsDate(string key, string day, string month, string year)
        {
            return ValidateDate(new ClinicalDetails(), key, day, month, year);
        }

        public ContentResult OnGetValidateNotificationSiteProperty(string key, string value, bool shouldValidateFull)
        {
            var notification = new Notification { ShouldValidateFull = shouldValidateFull };
            var notificationSite = new NotificationSite { 
                Notification = notification,
                SiteId = (int) SiteId.OTHER
            };
            return ValidateProperty(notificationSite, key, value);
        }

        public ContentResult OnGetValidateClinicalDetailsYearComparison(int newYear, int existingYear)
        {
            return ValidateYearComparison(newYear, existingYear);
        }
        
        public ContentResult OnGetValidateClinicalDetailsProperties(IEnumerable<Dictionary<string, string>> keyValuePairs)
        {
            var propertyValueTuples = new List<Tuple<string, object>>();
            foreach (var keyValuePair in keyValuePairs) {
                propertyValueTuples.Add(new Tuple<string, object>(keyValuePair["key"], keyValuePair["value"]));
            }
            return ValidateMultipleProperties(new ClinicalDetails(), propertyValueTuples);
        }
    }
}
