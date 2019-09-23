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
        private readonly IAuditService auditService;

        public ContactTracingModel(INotificationService service, IAuditService auditService) : base(service)
        {
            this.auditService = auditService;
        }

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }

        public override async Task<IActionResult> OnGetAsync(int? id)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            ContactTracing = Notification.ContactTracing;

            if (ContactTracing == null) {
                ContactTracing = new ContactTracing();
            }

            await auditService.OnGetAuditAsync(Notification.NotificationId, ContactTracing);
            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./SocialRiskFactors", new {id = notificationId});
        }

        protected override async Task<bool> ValidateAndSave(int? NotificationId) {

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
            return ValidateFullModel(model, key, "ContactTracing");
        }
    }
}
