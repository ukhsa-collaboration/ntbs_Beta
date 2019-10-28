using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages.Exceptions;
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

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted = false)
        {
            try
            {
                await SetNotificationAndAuthorize(id);
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch (NotAuthorizedException)
            {
                return RedirectToOverview(id);
            }

            ImmunosuppressionDetails = Notification.ImmunosuppressionDetails;
            await SetNotificationProperties(isBeingSubmitted, ImmunosuppressionDetails);
            
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./PreviousHistory", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave()
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            await service.UpdateImmunosuppresionDetailsAsync(Notification, ImmunosuppressionDetails);
            return true;
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