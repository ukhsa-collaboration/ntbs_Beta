using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ComorbiditiesModel : NotificationEditModelBase
    {
        [BindProperty]
        public ComorbidityDetails ComorbidityDetails { get; set; }

        [BindProperty]
        public ImmunosuppressionDetails ImmunosuppressionDetails { get; set; }

        public ComorbiditiesModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository) { }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            ComorbidityDetails = Notification.ComorbidityDetails;
            ImmunosuppressionDetails = Notification.ImmunosuppressionDetails;

            await SetNotificationProperties(isBeingSubmitted, ComorbidityDetails);
            await SetNotificationProperties(isBeingSubmitted, ImmunosuppressionDetails);

            if (ComorbidityDetails.ShouldValidateFull)
            {
                TryValidateModel(ComorbidityDetails, nameof(ComorbidityDetails));
            }

            if (ImmunosuppressionDetails.ShouldValidateFull)
            {
                TryValidateModel(ImmunosuppressionDetails, nameof(ImmunosuppressionDetails));
            }

            return Page();
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./SocialContextAddresses", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            ComorbidityDetails.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(ComorbidityDetails, nameof(ComorbidityDetails)))
            {
                await Service.UpdateComorbidityAsync(Notification, ComorbidityDetails);
            }

            ImmunosuppressionDetails.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(ImmunosuppressionDetails, nameof(ImmunosuppressionDetails)))
            {
                await Service.UpdateImmunosuppresionDetailsAsync(Notification, ImmunosuppressionDetails);
            }
        }

        public IActionResult OnGetValidateImmunosuppression(
            string status,
            bool hasBioTherapy,
            bool hasTransplantation,
            bool hasOther,
            string otherDescription)
        {
            var parsedStatus = string.IsNullOrEmpty(status) ? null : (Status?)Enum.Parse(typeof(Status), status);
            var model = new ImmunosuppressionDetails
            {
                Status = parsedStatus,
                HasBioTherapy = hasBioTherapy,
                HasTransplantation = hasTransplantation,
                HasOther = hasOther,
                OtherDescription = string.IsNullOrEmpty(otherDescription) ? null : otherDescription
            };

            return ValidationService.GetFullModelValidationResult(model);
        }
    }
}
