using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class ContactTracingModel : NotificationModelBase
    {
        public ContactTracingModel(INotificationService service) : base(service)
        {
        }

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            NotificationId = Notification.NotificationId;
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            ContactTracing = Notification.ContactTracing;
            if (ContactTracing == null) {
                ContactTracing = new ContactTracing();
            }
            
            SetNotificationProperties<ContactTracing>(isBeingSubmitted, ContactTracing);
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./SocialRiskFactors", new {id = notificationId});
        }

        protected override async Task<bool> ValidateAndSave() {

            if (!ModelState.IsValid)
            {
                return false;
            }

            await service.UpdateContactTracingAsync(Notification, ContactTracing);
            return true;
        }

        public ContentResult OnGetValidateContactTracing(ContactTracing model, string key)
        {
            return ValidateFullModel(model, key, "ContactTracing");
        }
    }
}
