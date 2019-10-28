using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ContactTracingModel : NotificationEditModelBase
    {
        public ContactTracingModel(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService) {}

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            return await base.OnGetAsync(id, isBeingSubmitted);
        }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            ContactTracing = Notification.ContactTracing;
            await SetNotificationProperties<ContactTracing>(isBeingSubmitted, ContactTracing);

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./SocialRiskFactors", new {id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave() {
            ContactTracing.SetFullValidation(Notification.NotificationStatus);
            if (!TryValidateModel(this))
            {
                return false;
            }

            await service.UpdateContactTracingAsync(Notification, ContactTracing);
            return true;
        }

        public ContentResult OnGetValidateContactTracing(ContactTracing model, string key)
        {
            return validationService.ValidateFullModel(model);
        }
    }
}
