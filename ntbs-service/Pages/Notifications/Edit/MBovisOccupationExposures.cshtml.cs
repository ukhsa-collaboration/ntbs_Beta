using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class MBovisOccupationExposuresModel : NotificationEditModelBase
    {
        private readonly IEnhancedSurveillanceAlertsService _enhancedSurveillanceAlertsService;

        public MBovisOccupationExposuresModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IEnhancedSurveillanceAlertsService enhancedSurveillanceAlertsService,
            IAlertRepository alertRepository) : base(notificationService, authorizationService,
                notificationRepository, alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditMBovisOccupationExposures;
            _enhancedSurveillanceAlertsService = enhancedSurveillanceAlertsService;
        }

        [BindProperty]
        public MBovisDetails MBovisDetails { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (!Notification.IsMBovis)
            {
                return NotFound();
            }

            MBovisDetails = Notification.MBovisDetails;

            await SetNotificationProperties(isBeingSubmitted, MBovisDetails);

            if (MBovisDetails.ShouldValidateFull)
            {
                TryValidateModel(MBovisDetails, nameof(MBovisDetails));
            }

            return Page();
        }

        protected override IActionResult RedirectToCreate()
        {
            return RedirectToPage("./Items/NewMBovisOccupationExposure", new { NotificationId });
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./MBovisAnimalExposures", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            if (ActionName == ActionNameString.Create)
            {
                MBovisDetails.OccupationExposureStatus = Status.Yes;
            }
            // Set the collection so it can be included in the validation
            MBovisDetails.MBovisOccupationExposures = Notification.MBovisDetails.MBovisOccupationExposures;
            MBovisDetails.ProceedingToAdd = ActionName == ActionNameString.Create;
            MBovisDetails.SetValidationContext(Notification);

            if (TryValidateModel(MBovisDetails, nameof(MBovisDetails)))
            {
                await Service.UpdateMBovisDetailsOccupationExposureAsync(Notification, MBovisDetails);

                await _enhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(Notification);
            }
        }

        public ContentResult OnPostValidateMBovisDetailsProperty([FromBody]InputValidationModel validationData)
        {
            return ValidationService.GetPropertyValidationResult<MBovisDetails>(validationData.Key, validationData.Value, validationData.ShouldValidateFull);
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithMBovisOccupationExposureAsync(notificationId);
        }
    }
}
