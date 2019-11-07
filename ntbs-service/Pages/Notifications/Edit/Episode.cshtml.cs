using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class EpisodeModel : NotificationEditModelBase
    {
        private readonly NtbsContext _context;
        private readonly IUserService _userService;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public SelectList TbServices { get; set; }
        public SelectList Hospitals { get; set; }

        [BindProperty]
        public FormattedDate FormattedNotificationDate { get; set; }

        [BindProperty]
        public Episode Episode { get; set; }

        public EpisodeModel(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IAuthorizationService authorizationService,
            IUserService userService,
            NtbsContext context) : base(notificationService, authorizationService, notificationRepository)
        {
            this._context = context;
            this._userService = userService;
            this._referenceDataRepository = referenceDataRepository;
        }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            Episode = Notification.Episode;
            await SetNotificationProperties(isBeingSubmitted, Episode);
            await SetTbServiceAndHospitalListsAsync();
            FormattedNotificationDate = Notification.NotificationDate.ConvertToFormattedDate();

            if (Episode.ShouldValidateFull)
            {
                ValidationService.TrySetAndValidateDateOnModel(Notification, nameof(Notification.NotificationDate), FormattedNotificationDate);
                TryValidateModel(Episode, Episode.GetType().Name);
            }

            return Page();
        }

        private async Task SetTbServiceAndHospitalListsAsync()
        {

            IEnumerable<string> tbServiceCodes;

            if (Notification.NotificationStatus == Models.Enums.NotificationStatus.Draft)
            {
                var services = await _userService.GetTbServicesAsync(User);
                tbServiceCodes = services.Select(s => s.Code);
                TbServices = new SelectList(services, nameof(TBService.Code), nameof(TBService.Name));
            }
            else
            {
                tbServiceCodes = new List<string> { Notification.Episode.TBServiceCode };
            }

            var hospitals = await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(tbServiceCodes);
            Hospitals = new SelectList(hospitals, nameof(Hospital.HospitalId), nameof(Hospital.Name));
        }

        public async Task<JsonResult> OnGetHospitalsByTbService(string tbServiceCode)
        {
            var tbServices = await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(new List<string> { tbServiceCode });
            return new JsonResult(tbServices);
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./ClinicalDetails", new { id = notificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            await SetValuesForValidation();
            if (Notification.NotificationStatus != Models.Enums.NotificationStatus.Draft && Notification.Episode.TBServiceCode != Episode.TBServiceCode)
            {
                ModelState.AddModelError("Episode.TBServiceCode", ValidationMessages.TBServiceCantChange);
            }

            if (TryValidateModel(Episode, Episode.GetType().Name))
            {
                await Service.UpdateEpisodeAsync(Notification, Episode);
            }
            else
            {
                // Detach notification to avoid getting cached notification when retrieving from context,
                // because cached notification date will change notification date on a banner even when invalid
                _context.Entry(Notification).State = EntityState.Detached;
            }
        }

        public ContentResult OnGetValidateEpisodeProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.ValidateModelProperty<Episode>(key, value, shouldValidateFull);
        }

        public async Task<ContentResult> OnGetValidateNotificationDateAsync(string key, string day, string month, string year, int notificationId)
        {
            // Query notification by Id when date validation depends on other properties of model
            Notification notification = await NotificationRepository.GetNotificationAsync(notificationId);
            return ValidationService.ValidateDate(notification, key, day, month, year);
        }

        private async Task SetValuesForValidation()
        {
            Episode.SetFullValidation(Notification.NotificationStatus);
            ValidationService.TrySetAndValidateDateOnModel(Notification, nameof(Notification.NotificationDate), FormattedNotificationDate);
            /*
            Binding only sets the entity ids, but not the actual entities.
            There's a validation rule that needs to check the relationship between the entities,
            therefore we need fetch the reference data from the db before validating
            */
            await GetHospitalAndTbService();
        }

        private async Task GetHospitalAndTbService()
        {
            if (Episode.HospitalId != null)
            {
                Episode.Hospital = await _referenceDataRepository.GetHospitalByGuidAsync(Episode.HospitalId.Value);
            }
            if (Episode.TBServiceCode != null)
            {
                Episode.TBService = await _referenceDataRepository.GetTbServiceByCodeAsync(Episode.TBServiceCode);
            }
        }
    }
}
