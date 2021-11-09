using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class MDRDetailsModel : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IEnhancedSurveillanceAlertsService _enhancedSurveillanceAlertsService;
        public List<string> RenderConditionalCountryFieldIds;

        public MDRDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IEnhancedSurveillanceAlertsService enhancedSurveillanceAlertsService,
            IAlertRepository alertRepository,
            IUserHelper userHelper)
            : base(
                service,
                authorizationService,
                notificationRepository,
                alertRepository,
                userHelper)
        {
            CurrentPage = NotificationSubPaths.EditMDRDetails;
            _referenceDataRepository = referenceDataRepository;
            _enhancedSurveillanceAlertsService = enhancedSurveillanceAlertsService;
        }

        [BindProperty]
        public MDRDetails MDRDetails { get; set; }

        [BindProperty]
        public FormattedDate FormattedMdrTreatmentDate { get; set; }

        public SelectList Countries { get; set; }


        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (!Notification.IsMdr)
            {
                return NotFound();
            }

            var countries = await _referenceDataRepository.GetAllCountriesAsync();
            Countries = new SelectList(countries, nameof(Country.CountryId), nameof(Country.Name));

            MDRDetails = Notification.MDRDetails;
            MDRDetails.Treatment = Notification.ClinicalDetails.TreatmentRegimen;
            FormattedMdrTreatmentDate = MDRDetails.MDRTreatmentStartDate.ConvertToFormattedDate();
            await SetNotificationProperties(isBeingSubmitted, MDRDetails);

            if (MDRDetails.ShouldValidateFull)
            {
                TryValidateModel(MDRDetails, MDRDetails.GetType().Name);
            }

            RenderConditionalCountryFieldIds = countries
                .Where(c => c.IsoCode == Models.Countries.UkCode)
                .Select(c => c.CountryId.ToString())
                .ToList();

            return Page();
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            var nextPage = Notification.IsMBovis ? "./MBovisExposureToKnownCases" : "./MDRDetails";
            return RedirectToPage(nextPage,
            new
            {
                NotificationId,
                isBeingSubmitted
            });
        }

        protected override async Task ValidateAndSave()
        {
            if (MDRDetails.CountryId != null)
            {
                MDRDetails.Country = await _referenceDataRepository.GetCountryByIdAsync((int)MDRDetails.CountryId);
            }

            UpdateFlags();
            ValidateRelatedNotificationId();

            ValidationService.TrySetFormattedDate(MDRDetails, "MDRDetails", nameof(MDRDetails.MDRTreatmentStartDate), FormattedMdrTreatmentDate);
            MDRDetails.DatesHaveBeenSet = true;

            // Add additional field required for date validation
            MDRDetails.Dob = Notification.PatientDetails.Dob;
            MDRDetails.Treatment = Notification.ClinicalDetails.TreatmentRegimen;

            MDRDetails.SetValidationContext(Notification);

            if (TryValidateModel(MDRDetails, nameof(MDRDetails)))
            {
                await Service.UpdateMDRDetailsAsync(Notification, MDRDetails);

                await _enhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(Notification);
            }
        }

        private void ValidateRelatedNotificationId()
        {
            if (MDRDetails.RelatedNotificationId != null)
            {
                if (MDRDetails.RelatedNotificationId == NotificationId)
                {
                    ModelState.AddModelError(
                        $"MDRDetails.RelatedNotificationId",
                        ValidationMessages.RelatedNotificationIdCannotBeSameAsNotificationId);
                }
            }
        }

        private void UpdateFlags()
        {
            if (MDRDetails.ExposureToKnownCaseStatus != Status.Yes)
            {
                MDRDetails.CountryId = null;
                MDRDetails.Country = null;
                ModelState.Remove("MDRDetails.CountryId");
                MDRDetails.RelationshipToCase = null;
                ModelState.Remove("MDRDetails.RelationshipToCase");
            }

            if (!MDRDetails.IsCountryUK)
            {
                MDRDetails.NotifiedToPheStatus = null;
                ModelState.Remove("MDRDetails.NotifiedToPheStatus");
            }

            if (MDRDetails.NotifiedToPheStatus != Status.Yes)
            {
                MDRDetails.RelatedNotificationId = null;
                ModelState.Remove("MDRDetails.RelatedNotificationId");
            }
        }

        public ContentResult OnPostValidateMDRDetailsProperty([FromBody]InputValidationModel validationData)
        {
            return ValidationService.GetPropertyValidationResult<MDRDetails>(validationData.Key, validationData.Value, validationData.ShouldValidateFull);
        }

        public async Task<ContentResult> OnPostValidateMDRDetailsDate([FromBody]DateValidationModel validationData)
        {
            var isLegacy = await NotificationRepository.IsNotificationLegacyAsync(NotificationId);
            return ValidationService.GetDateValidationResult<MDRDetails>(validationData.Key, validationData.Day, validationData.Month, validationData.Year, isLegacy);
        }
    }
}
