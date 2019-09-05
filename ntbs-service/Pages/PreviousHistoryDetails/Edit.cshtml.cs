using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_PatientTBHistory
{
    public class EditModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext _context;

        public EditModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            _context = context;
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id);
            }

            var notification = await service.GetNotificationAsync(id);
            await service.UpdatePatientTBHistoryAsync(notification, PatientTBHistory);
            
            return RedirectToPage("/Patients/Index");
        }
    }
}