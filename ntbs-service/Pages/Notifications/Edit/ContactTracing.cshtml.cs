using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
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
            ContactTracing = Notification.ContactTracing;
            
            SetNotificationProperties<ContactTracing>(isBeingSubmitted, ContactTracing);
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./SocialRiskFactors", new {id = notificationId});
        }

        protected override async Task<bool> ValidateAndSave(int NotificationId) {

            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateContactTracingAsync(notification, ContactTracing);
            return true;
        }

        public ContentResult OnGetValidateContactTracing(ContactTracing model, string key)
        {
            return ValidateFullModel(model);
        }
    }
}
