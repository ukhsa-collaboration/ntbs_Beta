using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ImmunosuppressionModel : NotificationEditModelBase
    {
        [BindProperty]
        public ImmunosuppressionDetails ImmunosuppressionDetails { get; set; }

        public ImmunosuppressionModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService)
        {
        }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            ImmunosuppressionDetails = Notification.ImmunosuppressionDetails;
            await SetNotificationProperties(isBeingSubmitted, ImmunosuppressionDetails);
            
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./PreviousHistory", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            if (ModelState.IsValid)
            {
                await service.UpdateImmunosuppresionDetailsAsync(Notification, ImmunosuppressionDetails);
            }
        }

        public IActionResult OnGetValidate(
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

            return validationService.ValidateFullModel(model);
        }
    }
}
