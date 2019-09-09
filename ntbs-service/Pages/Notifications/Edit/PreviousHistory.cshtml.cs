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
    public class PreviousHistoryModel : ValidationModel
    {
        private readonly INotificationService service;

        public PreviousHistoryModel(INotificationService service)
        {
            this.service = service;
        }

        [BindProperty]
        public PatientTBHistory PatientTBHistory { get; set; }
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
            PatientTBHistory = notification.PatientTBHistory;

            if (PatientTBHistory == null) {
                PatientTBHistory = new PatientTBHistory();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostPreviousPageAsync(int? NotificationId)
        {
            bool validModel = await validateAndSave(NotificationId);

            if(!validModel) {
                return await OnGetAsync(NotificationId);
            }

            return RedirectToPage("./ClinicalTimelines", new {id = NotificationId});
        }

        public async Task<IActionResult> OnPostNextPageAsync(int? NotificationId)
        {
            bool validModel = await validateAndSave(NotificationId);

            if(!validModel) {
                return await OnGetAsync(NotificationId);
            }

            return RedirectToPage("../Index", new {id = NotificationId});
        }

        public async Task<bool> validateAndSave(int? NotificationId) {
            
            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdatePatientTBHistoryAsync(notification, PatientTBHistory);
            return true;
        }
    }
}