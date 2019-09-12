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

        public async Task<IActionResult> OnPostPreviousPageAsync(int? NotificationId)
        {
            bool validModel = await validateAndSave(NotificationId);

            if(!validModel) {
                return await OnGetAsync(NotificationId);
            }

            return RedirectToPage("./Episode", new {id = NotificationId});
        }

        public async Task<IActionResult> OnPostNextPageAsync(int? NotificationId)
        {
            bool validModel = await validateAndSave(NotificationId);

            if(!validModel) {
                return await OnGetAsync(NotificationId);
            }

            return RedirectToPage("./PreviousHistory", new {id = NotificationId});
        }

        public async Task<bool> validateAndSave(int? NotificationId) {

            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateContactTracingAsync(notification, ContactTracing);
            return true;
        }

        public ContentResult OnPostValidateContactTracing(string key, string adultsIdentified, string childrenIdentified, string adultsScreened, string childrenScreened,
                            string adultsActiveTB, string childrenActiveTB, string adultsLatentTB, string childrenLatentTB, string adultsStartedTreatment, 
                            string childrenStartedTreatment, string adultsFinishedTreatment, string childrenFinishedTreatment)
        {
            return ValidateContactTracing(ContactTracing, key, adultsIdentified, childrenIdentified, adultsScreened, childrenScreened, adultsActiveTB, childrenActiveTB,
                adultsLatentTB, childrenLatentTB, adultsStartedTreatment, childrenStartedTreatment, adultsFinishedTreatment, childrenFinishedTreatment);
        }
    }
}
