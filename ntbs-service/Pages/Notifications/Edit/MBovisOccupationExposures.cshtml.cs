using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class MBovisOccupationExposuresModel : NotificationEditModelBase
    {
        public MBovisOccupationExposuresModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository) : base(notificationService, authorizationService,
                notificationRepository, alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditMBovisOccupationExposures;
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
                MBovisDetails.HasOccupationExposure = true;
            }
            // Set the collection so it can be included in the validation
            MBovisDetails.MBovisOccupationExposures = Notification.MBovisDetails.MBovisOccupationExposures;
            MBovisDetails.ProceedingToAdd = ActionName == ActionNameString.Create;
            MBovisDetails.SetValidationContext(Notification);

            if (TryValidateModel(MBovisDetails, nameof(MBovisDetails)))
            {
                await Service.UpdateMBovisDetailsOccupationExposureAsync(Notification, MBovisDetails);
            }
        }

        public ContentResult OnGetValidateMBovisDetailsProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<MBovisDetails>(key, value, shouldValidateFull);
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithMBovisOccupationExposureAsync(notificationId);
        }
    }
}
