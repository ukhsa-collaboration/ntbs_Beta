using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class MDRDetailsModel : NotificationEditModelBase
    {
        public MDRDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) : base(service, authorizationService, notificationRepository)
            { 
                NotUKCountries = new SelectList(
                    referenceDataRepository.GetAllCountriesApartFromUKAsync().Result,
                    nameof(Country.CountryId),
                    nameof(Country.Name)
                );
            }

        [BindProperty]
        public MDRDetails MDRDetails { get; set; }

        public SelectList NotUKCountries { get; set; }


        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            MDRDetails = Notification.MDRDetails;
            await SetNotificationProperties(isBeingSubmitted, MDRDetails);

            if (MDRDetails.ShouldValidateFull)
            {
                TryValidateModel(MDRDetails, MDRDetails.GetType().Name);
            }

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            // This is the last page in the flow, so there's no next page to go to
            return RedirectToPage("./MDRDetails", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            UpdateFlags();
            await ValidateRelatedNotification();
            MDRDetails.SetFullValidation(Notification.NotificationStatus);

            if (TryValidateModel(MDRDetails.GetType().Name))
            {
                await Service.UpdateMDRDetailsAsync(Notification, MDRDetails);
            }
        }

        private async Task ValidateRelatedNotification()
        {
            if (MDRDetails.RelatedNotificationId != null) {
                var relatedNotification = await GetRelatedNotification(MDRDetails.RelatedNotificationId.Value);
                if (relatedNotification == null)
                {
                    ModelState.AddModelError("MDRDetails.RelatedNotificationId", ValidationMessages.RelatedNotificationIdInvalid);
                }
            }
        }

        private async Task<Notification> GetRelatedNotification(int noticicationId)
        {
            return await NotificationRepository.GetNotificationAsync(noticicationId);
        }

        private void UpdateFlags()
        {
            if (MDRDetails.ExposureToKnownCaseStatus != Status.Yes || MDRDetails.CaseInUKStatus != Status.Yes)
            {
                MDRDetails.RelatedNotificationId = null;
                ModelState.Remove("MDRDetails.RelatedNotificationId");
            }
            if (MDRDetails.ExposureToKnownCaseStatus != Status.Yes || MDRDetails.CaseInUKStatus != Status.No)
            {
                MDRDetails.CountryId = null;
                ModelState.Remove("MDRDetails.CountryId");
            }

            if (MDRDetails.ExposureToKnownCaseStatus != Status.Yes)
            {
                MDRDetails.RelationshipToCase = null;
                MDRDetails.CaseInUKStatus = null;
                ModelState.Remove("MDRDetails.RelationshipToCase");
                ModelState.Remove("MDRDetails.CaseInUKStatus");
            }
        }

        public ContentResult OnGetValidateMDRDetailsProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.ValidateModelProperty<MDRDetails>(key, value, shouldValidateFull);
        }

        public async Task<ContentResult> OnGetValidateMDRDetailsRelatedNotificationAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return ValidationService.ValidContent();
            }
            if (int.TryParse(value, out var notificationId))
            {
                var relatedNotification = await GetRelatedNotification(notificationId);
                if (relatedNotification == null)
                {
                    return CreateJsonResponse(new { validationMessage = ValidationMessages.RelatedNotificationIdInvalid });
                }
                var info = NotificationInfo.CreateFromNotification(relatedNotification);
                return CreateJsonResponse(new { relatedNotification = info });
            }
            return CreateJsonResponse(new { validationMessage = ValidationMessages.RelatedNotificationIdMustBeInteger });
        }
    }
}
