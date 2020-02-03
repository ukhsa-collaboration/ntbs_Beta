using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
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
        public Status? ImmunosuppressionStatus { get; set; }

        [BindProperty]
        public bool HasBioTherapy { get; set; }
        [BindProperty]
        public bool HasTransplantation { get; set; }
        [BindProperty]
        public bool HasOther { get; set; }

        [BindProperty]
        public string OtherDescription { get; set; }

        
        public ImmunosuppressionDetails ImmunosuppressionDetails { get; set; }

        public ComorbiditiesModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
            CurrentPage = NotificationSubPaths.EditComorbidities;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            ComorbidityDetails = Notification.ComorbidityDetails;
            ImmunosuppressionDetails = Notification.ImmunosuppressionDetails;

            ImmunosuppressionStatus = ImmunosuppressionDetails.Status;
            HasBioTherapy = ImmunosuppressionDetails.HasBioTherapy == true;
            HasTransplantation = ImmunosuppressionDetails.HasTransplantation == true;
            HasOther = ImmunosuppressionDetails.HasOther == true;
            OtherDescription = ImmunosuppressionDetails.OtherDescription;

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
            ComorbidityDetails.SetValidationContext(Notification);

            ImmunosuppressionDetails = new ImmunosuppressionDetails {
                Status = ImmunosuppressionStatus,
                HasBioTherapy = HasBioTherapy,
                HasTransplantation = HasTransplantation,
                HasOther = HasOther,
                OtherDescription = OtherDescription,
            };

            ImmunosuppressionDetails.SetValidationContext(Notification);

            if (TryValidateModel(ComorbidityDetails, nameof(ComorbidityDetails))
                && TryValidateModel(ImmunosuppressionDetails, nameof(ImmunosuppressionDetails)))
            {
                await Service.UpdateComorbidityAsync(Notification, ComorbidityDetails);
                await Service.UpdateImmunosuppresionDetailsAsync(Notification, ImmunosuppressionDetails);
            }
        }

        public IActionResult OnGetValidateImmunosuppression(
            string immunosuppressionStatus,
            bool hasBioTherapy,
            bool hasTransplantation,
            bool hasOther,
            string otherDescription)
        {
            var parsedStatus = string.IsNullOrEmpty(immunosuppressionStatus) ? null : (Status?)Enum.Parse(typeof(Status), immunosuppressionStatus);
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
