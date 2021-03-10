using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
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
            IAlertRepository alertRepository) : base(service,
            authorizationService,
            notificationRepository,
            alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditMDRDetails;
            _referenceDataRepository = referenceDataRepository;
            _enhancedSurveillanceAlertsService = enhancedSurveillanceAlertsService;
        }

        [BindProperty]
        public MDRDetails MDRDetails { get; set; }

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

        public ContentResult OnGetValidateMDRDetailsProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<MDRDetails>(key, value, shouldValidateFull);
        }
    }
}
