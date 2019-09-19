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
    public class EpisodeModel : NotificationModelBase
    {
        private readonly NtbsContext context;
        
        public SelectList TBServices { get; set; }
        public SelectList Hospitals { get; set; }


        [BindProperty]
        public Episode Episode { get; set; }

        public EpisodeModel(INotificationService service, NtbsContext context) : base(service)
        {
            this.context = context;
        }

        public override async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await service.GetNotificationAsync(id);
            NotificationId = notification.NotificationId;
            NotificationStatus = notification.NotificationStatus;
            Episode = notification.Episode;

            if (Episode == null)
            {
                return NotFound();
            }

            TBServices = new SelectList(context.GetAllTbServicesAsync().Result, 
                                        nameof(TBService.Code), 
                                        nameof(TBService.Name));

            Hospitals = new SelectList(context.GetAllHospitalsAsync().Result, 
                                        nameof(Hospital.HospitalId), 
                                        nameof(Hospital.Name));

            return Page();
        }

        public JsonResult OnGetHospitalsByTBService(string tbServiceCode) 
        {
            var tbServices = context.GetHospitalsByTBService(tbServiceCode).Result;
            return new JsonResult(tbServices);
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./ClinicalDetails", new {id = notificationId});
        }

        protected override async Task<bool> ValidateAndSave(int? NotificationId) {
    
            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationAsync(NotificationId);
            await service.UpdateEpisodeAsync(notification, Episode);
            return true;
        }

        public ContentResult OnGetValidateEpisodeProperty(string key, string value)
        {
            return ValidateProperty(new Episode(), key, value);
        }
    }
}