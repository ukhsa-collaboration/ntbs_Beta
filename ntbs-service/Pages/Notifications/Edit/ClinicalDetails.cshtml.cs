using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    [BindProperties]
    public class ClinicalDetailsModel : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public ClinicalDetails ClinicalDetails { get; set; }

        public Dictionary<SiteId, bool> NotificationSiteMap { get; set; }

        public List<Site> Sites { get; set; }
        // We want to bind to the full model rather than a string for SiteDescription so we can validate against the data annotation
        public NotificationSite OtherSite { get; set; }

        public int? PatientBirthYear { get; set; }

        public FormattedDate FormattedSymptomDate { get; set; }
        public FormattedDate FormattedFirstPresentationDate { get; set; }
        public FormattedDate FormattedTbServicePresentationDate { get; set; }
        public FormattedDate FormattedDiagnosisDate { get; set; }
        public FormattedDate FormattedTreatmentDate { get; set; }
        public FormattedDate FormattedDeathDate { get; set; }
        public FormattedDate FormattedMdrTreatmentDate { get; set; }

        public ClinicalDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) : base(service, authorizationService, notificationRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            ClinicalDetails = Notification.ClinicalDetails;
            await SetNotificationProperties(isBeingSubmitted, ClinicalDetails);

            var notificationSites = Notification.NotificationSites;
            notificationSites.ForEach(x => x.ShouldValidateFull = Notification.ShouldValidateFull);

            SetupNotificationSiteMap(notificationSites);
            OtherSite = new NotificationSite
            {
                SiteId = (int)SiteId.OTHER,
                SiteDescription = notificationSites.FirstOrDefault(ns => ns.SiteId == (int)SiteId.OTHER)?.SiteDescription,
                ShouldValidateFull = Notification.ShouldValidateFull
            };
            Sites = (await _referenceDataRepository.GetAllSitesAsync()).ToList();

            PatientBirthYear = Notification.PatientDetails.Dob?.Year;

            FormattedSymptomDate = ClinicalDetails.SymptomStartDate.ConvertToFormattedDate();
            FormattedFirstPresentationDate = ClinicalDetails.FirstPresentationDate.ConvertToFormattedDate();
            FormattedTbServicePresentationDate = ClinicalDetails.TBServicePresentationDate.ConvertToFormattedDate();
            FormattedDiagnosisDate = ClinicalDetails.DiagnosisDate.ConvertToFormattedDate();
            FormattedTreatmentDate = ClinicalDetails.TreatmentStartDate.ConvertToFormattedDate();
            FormattedDeathDate = ClinicalDetails.DeathDate.ConvertToFormattedDate();
            FormattedMdrTreatmentDate = ClinicalDetails.MDRTreatmentStartDate.ConvertToFormattedDate();

            if (ClinicalDetails.ShouldValidateFull)
            {
                TryValidateModel(this);
            }

            return Page();
        }

        protected override async Task<Notification> GetNotification(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithNotificationSitesAsync(notificationId);
        }

        private void SetupNotificationSiteMap(IEnumerable<NotificationSite> notificationSites)
        {
            NotificationSiteMap = new Dictionary<SiteId, bool>();
            foreach (SiteId siteId in Enum.GetValues(typeof(SiteId)))
            {
                NotificationSiteMap.Add(siteId, notificationSites.FirstOrDefault(ns => ns.SiteId == (int)siteId) != null);
            }
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./ContactTracing", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();

            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.SymptomStartDate), FormattedSymptomDate);
            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.FirstPresentationDate), FormattedFirstPresentationDate);
            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.TBServicePresentationDate), FormattedTbServicePresentationDate);
            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DiagnosisDate), FormattedDiagnosisDate);
            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.TreatmentStartDate), FormattedTreatmentDate);
            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.DeathDate), FormattedDeathDate);
            ValidationService.TrySetAndValidateDateOnModel(ClinicalDetails, nameof(ClinicalDetails.MDRTreatmentStartDate), FormattedMdrTreatmentDate);
            if (ClinicalDetails.BCGVaccinationYear != null)
            {
                ValidationService.ValidateYearComparisonOnModel(ClinicalDetails, nameof(ClinicalDetails.BCGVaccinationYear),
                (int)ClinicalDetails.BCGVaccinationYear, PatientBirthYear);
            }

            var notificationSites = CreateNotificationSitesFromModel(Notification);

            ClinicalDetails.SetFullValidation(Notification.NotificationStatus);
            OtherSite?.SetFullValidation(Notification.NotificationStatus);

            // Since notification has other properties which are not populated by this page but have validation rules, 
            // validation of a whole Notification model will result in validation errors.
            // Therefore we need to manually validate NotificationSites and TryValidate other models
            if (Notification.ShouldValidateFull && !notificationSites.Any())
            {
                ModelState.AddModelError("Notification.NotificationSites", ValidationMessages.DiseaseSiteIsRequired);
            }

            TryValidateModel(ClinicalDetails, ClinicalDetails.GetType().Name);
            if (OtherSite != null)
            {
                TryValidateModel(OtherSite, OtherSite.GetType().Name);
            }

            if (ModelState.IsValid)
            {
                await Service.UpdateClinicalDetailsAsync(Notification, ClinicalDetails);
                await Service.UpdateSitesAsync(Notification.NotificationId, notificationSites);
            }
        }

        private void UpdateFlags()
        {
            if (ClinicalDetails.IsSymptomatic == false)
            {
                ClinicalDetails.SymptomStartDate = null;
                FormattedSymptomDate = ClinicalDetails.SymptomStartDate.ConvertToFormattedDate();
                ModelState.Remove("ClinicalDetails.SymptomStartDate");
            }

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
                FormattedMdrTreatmentDate = ClinicalDetails.MDRTreatmentStartDate.ConvertToFormattedDate();
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
            foreach (var (siteId, value) in NotificationSiteMap)
            {
                if (value)
                {
                    yield return new NotificationSite
                    {
                        ShouldValidateFull = notification.ShouldValidateFull,
                        NotificationId = notification.NotificationId,
                        SiteId = (int)siteId,
                        SiteDescription = siteId == SiteId.OTHER ? OtherSite.SiteDescription : null
                    };
                }
            }
        }

        public ContentResult OnGetValidateClinicalDetailsDate(string key, string day, string month, string year)
        {
            return ValidationService.ValidateDate<ClinicalDetails>(key, day, month, year);
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

            return ValidationService.ValidateModelProperty<Notification>(key, notificationSites, shouldValidateFull);
        }

        public ContentResult OnGetValidateNotificationSiteProperty(string key, string value, bool shouldValidateFull)
        {
            var notificationSite = new NotificationSite
            {
                ShouldValidateFull = shouldValidateFull,
                SiteId = (int)SiteId.OTHER
            };
            return ValidationService.ValidateProperty(notificationSite, key, value);
        }

        public ContentResult OnGetValidateClinicalDetailsYearComparison(int newYear, int existingYear)
        {
            return ValidationService.ValidateYearComparison(newYear, existingYear);
        }

        public ContentResult OnGetValidateClinicalDetailsProperties(IEnumerable<Dictionary<string, string>> keyValuePairs)
        {
            var propertyValueTuples = new List<Tuple<string, object>>();
            foreach (var keyValuePair in keyValuePairs)
            {
                propertyValueTuples.Add(new Tuple<string, object>(keyValuePair["key"], keyValuePair["value"]));
            }
            return ValidationService.ValidateMultipleProperties<ClinicalDetails>(propertyValueTuples);
        }
    }
}
