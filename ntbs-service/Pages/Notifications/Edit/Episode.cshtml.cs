using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class EpisodeModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext _context;
        
        public SelectList TBServices { get; set; }
        public SelectList Hospitals { get; set; }


        [BindProperty]
        public Episode Episode { get; set; }

        [BindProperty]
        public int NotificationId { get; set; }
        

        public EpisodeModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await service.GetNotificationAsync(id);
            NotificationId = notification.NotificationId;
            Episode = notification.Episode;

            if (Episode == null)
            {
                return NotFound();
            }

            TBServices = new SelectList(_context.GetAllTbServicesAsync().Result, 
                                        nameof(TBService.Code), 
                                        nameof(TBService.Name));

            Hospitals = new SelectList(_context.GetAllHospitalsAsync().Result, 
                                        nameof(Hospital.HospitalId), 
                                        nameof(Hospital.Name));

            return Page();
        }

        public JsonResult OnGetHospitalsByTBService(string tbServiceCode) 
        {
            var tbServices = _context.GetHospitalsByTBService(tbServiceCode).Result;
            Console.WriteLine(tbServices[0].Name);
            return new JsonResult(tbServices);
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateEpisodeAsync(notification, Episode);

            if (!ModelState.IsValid)
            {
                return await OnGetAsync(id);
            }

            return RedirectToPage("../Index");
        }

        public ContentResult OnPostValidateEpisodeProperty(string key, string value)
        {
            return OnPostValidateProperty(Episode, key, value);
        }
    }
}