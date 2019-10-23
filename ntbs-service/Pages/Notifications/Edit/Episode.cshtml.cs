using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;
using ntbs_service.Helpers;

namespace ntbs_service.Pages.Notifications.Edit
{
    
    public class EpisodeModel : NotificationEditModelBase
    {
        private readonly NtbsContext context;

        public SelectList TBServices { get; set; }
        public SelectList Hospitals { get; set; }

        [BindProperty]
        public FormattedDate FormattedNotificationDate { get; set; }
        
        [BindProperty]
        public Episode Episode { get; set; }

        public EpisodeModel(INotificationService service, NtbsContext context) : base(service)
        {
            this.context = context;
            
            TBServices = new SelectList(context.GetAllTbServicesAsync().Result,
                                        nameof(TBService.Code),
                                        nameof(TBService.Name));

            Hospitals = new SelectList(context.GetAllHospitalsAsync().Result,
                                        nameof(Hospital.HospitalId),
                                        nameof(Hospital.Name));
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            Episode = Notification.Episode;
            await SetNotificationProperties<Episode>(isBeingSubmitted, Episode);

            FormattedNotificationDate = Notification.NotificationDate.ConvertToFormattedDate();

            if (Episode.ShouldValidateFull)
            {
                TryValidateModel(Episode, Episode.GetType().Name);
            }

            return Page();
        }

        public JsonResult OnGetHospitalsByTBService(string tbServiceCode)
        {
            var tbServices = context.GetHospitalsByTBService(tbServiceCode).Result;
            return new JsonResult(tbServices);
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./ClinicalDetails", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task<bool> ValidateAndSave() 
        {
            Episode.SetFullValidation(Notification.NotificationStatus);
            validationService.TrySetAndValidateDateOnModel(Notification, nameof(Notification.NotificationDate), FormattedNotificationDate);

            if (!TryValidateModel(Episode, Episode.GetType().Name))
            {
                return false;
            }

            await service.UpdateEpisodeAsync(Notification, Episode);
            return true;
        }

        public ContentResult OnGetValidateEpisodeProperty(string key, string value, bool shouldValidateFull)
        {
            return validationService.ValidateModelProperty<Episode>(key, value, shouldValidateFull);
        }

        public async Task<ContentResult> OnGetValidateNotificationDateAsync(string key, string day, string month, string year, int notificationId)
        {
            // Query notification by Id when date validation depends on other properties of model
            Notification notification = await service.GetNotificationAsync(notificationId);
            return validationService.ValidateDate(notification, key, day, month, year);
        }
    }
}