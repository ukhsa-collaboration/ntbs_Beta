using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ContactTracingModel : NotificationEditModelBase
    {
        public ContactTracingModel(INotificationService service) : base(service) {}

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            ContactTracing = Notification.ContactTracing;
            await SetNotificationProperties<ContactTracing>(isBeingSubmitted, ContactTracing);

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./SocialRiskFactors", new {id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave() {
            ContactTracing.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(ContactTracing, ContactTracing.GetType().Name))
            {
                await service.UpdateContactTracingAsync(Notification, ContactTracing);
            }
        }

        public ContentResult OnGetValidateContactTracing(ContactTracing model, string key)
        {
            return validationService.ValidateFullModel(model);
        }
    }
}
