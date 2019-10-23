using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    [BindProperties]
    public class ClinicalDetailsModel : NotificationEditModelBase
    {
        private readonly NtbsContext context;
        public HIVTestStatus HivTestStatuses;

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

        public ClinicalDetailsModel(INotificationService service, NtbsContext context) : base(service)
        {
            this.context = context;
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationWithNotificationSitesAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            ClinicalDetails = Notification.ClinicalDetails;
            await SetNotificationProperties<ClinicalDetails>(isBeingSubmitted, ClinicalDetails);

            var notificationSites = Notification.NotificationSites;
            notificationSites.ForEach(x => x.ShouldValidateFull = Notification.ShouldValidateFull);
            
            SetupNotificationSiteMap(notificationSites);
            OtherSite = new NotificationSite {
                SiteId = (int) SiteId.OTHER,
                SiteDescription = notificationSites.FirstOrDefault(ns => ns.SiteId == (int)SiteId.OTHER)?.SiteDescription,
                ShouldValidateFull = Notification.ShouldValidateFull
            };
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

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./ContactTracing", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave()
        {
            UpdateFlags();

            validationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.SymptomStartDate), FormattedSymptomDate);
            validationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.PresentationDate), FormattedPresentationDate);
            validationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DiagnosisDate), FormattedDiagnosisDate);
            validationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.TreatmentStartDate), FormattedTreatmentDate);
            validationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DeathDate), FormattedDeathDate);
            validationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.MDRTreatmentStartDate), FormattedMDRTreatmentDate);
            if (ClinicalDetails.BCGVaccinationYear != null)
            {
                validationService.ValidateYearComparisonOnModel(ClinicalDetails, nameof(ClinicalDetails.BCGVaccinationYear),
                (int)ClinicalDetails.BCGVaccinationYear, PatientBirthYear);
            }
            
            var notificationSites = CreateNotificationSitesFromModel(Notification);
            ClinicalDetails.SetFullValidation(Notification.NotificationStatus);
            OtherSite?.SetFullValidation(Notification.NotificationStatus);
            // Separate notification with notification sites only is needed to check if notification sites are valid,
            // and to avoid updating notification site when updating Clinical Details
            var notificationWithSitesOnly = new Notification {
                ShouldValidateFull = Notification.ShouldValidateFull,
                NotificationSites = notificationSites.ToList()
            };
            
            var isValid = TryValidateModel(this);
            // Validate notification with sites regardless previous validation result
            isValid = TryValidateModel(notificationWithSitesOnly, notificationWithSitesOnly.GetType().Name) && isValid;

            if (isValid)
            {
                await service.UpdateClinicalDetailsAsync(Notification, ClinicalDetails);
                await service.UpdateSitesAsync(Notification.NotificationId, notificationSites);
            }

            return isValid;
        }

        private void UpdateFlags()
        {
            if (ClinicalDetails.DidNotStartTreatment == true)
            {
                ClinicalDetails.TreatmentStartDate = null;
                FormattedTreatmentDate = ClinicalDetails.TreatmentStartDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.TreatmentStartDate");
            }

            if (ClinicalDetails.IsPostMortem == false)
            {
                ClinicalDetails.DeathDate = null;
                FormattedDeathDate = ClinicalDetails.DeathDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.DeathDate");
            }

            if (ClinicalDetails.BCGVaccinationState != Status.Yes)
            {
                ClinicalDetails.BCGVaccinationYear = null;
                ModelState.Remove("ClinicalDetails.BCGVaccinationYear");
            }

            if (ClinicalDetails.IsMDRTreatment.HasValue && !ClinicalDetails.IsMDRTreatment.Value)
            {
                ClinicalDetails.MDRTreatmentStartDate = null;
                FormattedMDRTreatmentDate = ClinicalDetails.MDRTreatmentStartDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.MDRTreatmentStartDate");
            }

            if (!NotificationSiteMap.ContainsKey(SiteId.OTHER) || !NotificationSiteMap[SiteId.OTHER])
            {
                OtherSite = null;
                ModelState.Remove("OtherSite.SiteDescription");
            }
        }

        private IEnumerable<NotificationSite> CreateNotificationSitesFromModel(Notification notification)
        {
            foreach (var item in NotificationSiteMap)
            {
                if (item.Value)
                {
                    yield return new NotificationSite
                    {
                        ShouldValidateFull = notification.ShouldValidateFull,
                        NotificationId = notification.NotificationId,
                        SiteId = (int)item.Key,
                        SiteDescription = item.Key == SiteId.OTHER ? OtherSite.SiteDescription : null
                    };
                }
            }
        }

        public ContentResult OnGetValidateClinicalDetailsDate(string key, string day, string month, string year)
        {
            return validationService.ValidateDate<ClinicalDetails>(key, day, month, year);
        }

        public ContentResult OnGetValidateNotificationSites(IEnumerable<string> valueList, bool shouldValidateFull)
        {
            const string key = "NotificationSites";
            var notificationSites = new List<NotificationSite>();
            foreach (var value in valueList)
            {
                notificationSites.Add(new NotificationSite
                {
                    SiteId = (int)Enum.Parse(typeof(SiteId), value)
                });
            }

            return validationService.ValidateModelProperty<Notification>(key, notificationSites, shouldValidateFull);
        }

        public ContentResult OnGetValidateNotificationSiteProperty(string key, string value, bool shouldValidateFull)
        {
            var notificationSite = new NotificationSite
            {
                ShouldValidateFull = shouldValidateFull,
                SiteId = (int)SiteId.OTHER
            };
            return validationService.ValidateProperty(notificationSite, key, value);
        }

        public ContentResult OnGetValidateClinicalDetailsYearComparison(int newYear, int existingYear)
        {
            return validationService.ValidateYearComparison(newYear, existingYear);
        }

        public ContentResult OnGetValidateClinicalDetailsProperties(IEnumerable<Dictionary<string, string>> keyValuePairs)
        {
            var propertyValueTuples = new List<Tuple<string, object>>();
            foreach (var keyValuePair in keyValuePairs)
            {
                propertyValueTuples.Add(new Tuple<string, object>(keyValuePair["key"], keyValuePair["value"]));
            }
            return validationService.ValidateMultipleProperties<ClinicalDetails>(propertyValueTuples);
        }
    }
}
