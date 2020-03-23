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
        public MDRDetailsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IAlertRepository alertRepository) : base(service,
            authorizationService,
            notificationRepository,
            alertRepository)
        {
            NotUKCountries = new SelectList(
                referenceDataRepository.GetAllCountriesApartFromUkAsync().Result,
                nameof(Country.CountryId),
                nameof(Country.Name)
            );

            CurrentPage = NotificationSubPaths.EditMDRDetails;
        }

        [BindProperty]
        public MDRDetails MDRDetails { get; set; }

        public SelectList NotUKCountries { get; set; }


        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (!Notification.IsMdr)
            {
                return NotFound();
            }

            MDRDetails = Notification.MDRDetails;
            await SetNotificationProperties(isBeingSubmitted, MDRDetails);

            if (MDRDetails.ShouldValidateFull)
            {
                TryValidateModel(MDRDetails, MDRDetails.GetType().Name);
            }

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
            UpdateFlags();
            await ValidateRelatedNotification();
            MDRDetails.SetValidationContext(Notification);

            if (TryValidateModel(MDRDetails.GetType().Name))
            {
                await Service.UpdateMDRDetailsAsync(Notification, MDRDetails);
            }
        }

        private async Task ValidateRelatedNotification()
        {
            if (MDRDetails.RelatedNotificationId != null)
            {
                var relatedNotification =
                    await NotificationRepository.GetNotificationAsync(MDRDetails.RelatedNotificationId.Value);
                if (relatedNotification == null || !relatedNotification.HasBeenNotified)
                {
                    ModelState.AddModelError("MDRDetails.RelatedNotificationId",
                        ValidationMessages.IdDoesNotMatchNtbsRecord);
                }
            }
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
            return ValidationService.GetPropertyValidationResult<MDRDetails>(key, value, shouldValidateFull);
        }
    }
}
