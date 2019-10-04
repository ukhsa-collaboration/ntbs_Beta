using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ImmunosuppressionModel : NotificationModelBase
    {
        [BindProperty]
        public ImmunosuppressionDetails ImmunosuppressionDetails { get; set; }

        public ImmunosuppressionModel(INotificationService service) : base(service)
        {
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted = false)
        {
            Notification = await service.GetNotificationWithImmunosuppressionDetailsAsync(id);
            if (Notification == null) 
            {
                return NotFound();
            }
            ImmunosuppressionDetails = Notification.ImmunosuppressionDetails;

            SetNotificationProperties(isBeingSubmitted, ImmunosuppressionDetails);
            
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./PreviousHistory", new { id = notificationId });
        }

        protected override async Task<bool> ValidateAndSave(int notificationId)
        {
            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationWithImmunosuppressionDetailsAsync(notificationId);
            await service.UpdateImmunosuppresionDetailsAsync(notification, ImmunosuppressionDetails);
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


            return ValidateFullModel(model);
        }
    }
}