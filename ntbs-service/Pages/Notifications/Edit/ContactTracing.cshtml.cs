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

    public class ContactTracingModel : ValidationModel
    {
        private readonly INotificationService service;

        public ContactTracingModel(INotificationService service)
        {
            this.service = service;
        }

        [BindProperty]
        public ContactTracing ContactTracing { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var notification = await service.GetNotificationAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            NotificationId = notification.NotificationId;
            ContactTracing = notification.ContactTracing;

            if (ContactTracing == null) {
                ContactTracing = new ContactTracing();
            }

            return Page();
        }

    }
}
