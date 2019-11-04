using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Models;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;
using ntbs_service.Helpers;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Pages.Notifications.Edit
{

    public class EpisodeModel : NotificationEditModelBase
    {
        private readonly NtbsContext context;
        private readonly IUserService userService;

        public SelectList TBServices { get; set; }
        public SelectList Hospitals { get; set; }

        [BindProperty]
        public FormattedDate FormattedNotificationDate { get; set; }

        [BindProperty]
        public Episode Episode { get; set; }

        public EpisodeModel(INotificationService service,
                            IAuthorizationService authorizationService,
                            NtbsContext context,
                            IUserService userService) : base(service, authorizationService)
        {
            this.context = context;
            this.userService = userService;
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            return await base.OnGetAsync(id, isBeingSubmitted);
        }
        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            Episode = Notification.Episode;
            await SetNotificationProperties(isBeingSubmitted, Episode);
            await SetTbServiceAndHospitalListsAsync();
            FormattedNotificationDate = Notification.NotificationDate.ConvertToFormattedDate();

            if (Episode.ShouldValidateFull)
            {
                validationService.TrySetAndValidateDateOnModel(Notification, nameof(Notification.NotificationDate), FormattedNotificationDate);
                TryValidateModel(Episode, Episode.GetType().Name);
            }

            return Page();
        }

        private async Task SetTbServiceAndHospitalListsAsync()
        {

            IEnumerable<string> tbServiceCodes;
            if(Notification.NotificationStatus == Models.Enums.NotificationStatus.Draft) 
            {
                var services = await userService.GetTbServicesAsync(User);
                tbServiceCodes = services.Select(s => s.Code);
                TBServices = new SelectList(services, nameof(TBService.Code), nameof(TBService.Name));
            }
            else
            {
                tbServiceCodes = new List<string>() {Notification.Episode.TBServiceCode};
            }
            var hospitals = await context.GetHospitalsByTbServiceCodesAsync(tbServiceCodes);
            
            Hospitals = new SelectList(hospitals, nameof(Hospital.HospitalId), nameof(Hospital.Name));
        }

        public async Task<JsonResult> OnGetHospitalsByTBService(string tbServiceCode)
        {
            var tbServices = await context.GetHospitalsByTbServiceCodesAsync(new List<string> { tbServiceCode });
            return new JsonResult(tbServices);
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./ClinicalDetails", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            SetValuesForValidation();
            if(Notification.NotificationStatus != Models.Enums.NotificationStatus.Draft && Notification.Episode.TBServiceCode != Episode.TBServiceCode)
            {
                ModelState.AddModelError("Episode.TBServiceCode", ValidationMessages.TBServiceCantChange);
            }

            if (TryValidateModel(Episode, Episode.GetType().Name))
            {
                await service.UpdateEpisodeAsync(Notification, Episode);
            }
            else
            {
                // Detach notification to avoid getting cached notification when retrieving from context,
                // because cached notification date will change notification date on a banner even when invalid
                context.Entry(Notification).State = EntityState.Detached;
            }
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

        private void SetValuesForValidation()
        {
            Episode.SetFullValidation(Notification.NotificationStatus);
            validationService.TrySetAndValidateDateOnModel(Notification, nameof(Notification.NotificationDate), FormattedNotificationDate);
            /*
            Binding only sets the entity ids, but not the actual entities.
            There's a validation rule that needs to check the relationship between the entities,
            therefore we need fetch the reference data from the db before validating
            */
            SetHospitalAndTbService();
        }

        private void SetHospitalAndTbService()
        {
            if (Episode.HospitalId != null)
            {
                Episode.Hospital = context.Hospital.Where(h => h.HospitalId == Episode.HospitalId).FirstOrDefault();
            }
            if (Episode.TBServiceCode != null)
            {
                Episode.TBService = context.TbService.Where(s => s.Code == Episode.TBServiceCode).FirstOrDefault();
            }
        }
    }
}
