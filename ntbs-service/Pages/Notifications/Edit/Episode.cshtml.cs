using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.ReferenceEntities;
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
        public SelectList CaseManagers { get; set; }

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
            _context = context;
            _userService = userService;
            _referenceDataRepository = referenceDataRepository;

            CurrentPage = NotificationSubPaths.EditEpisode;
        }

        protected override Task<Notification> GetNotificationAsync(int notificationId)
        {
            return NotificationRepository.GetNotificationWithCaseManagerTbServices(notificationId);
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            Episode = Notification.Episode;
            await SetNotificationProperties(isBeingSubmitted, Episode);
            await SetDropdownsAsync();
            FormattedNotificationDate = Notification.NotificationDate.ConvertToFormattedDate();

            if (Episode.ShouldValidateFull)
            {
                ValidationService.TrySetFormattedDate(
                    Notification, 
                    nameof(Notification), 
                    nameof(Notification.NotificationDate), 
                    FormattedNotificationDate);
                TryValidateModel(Episode, Episode.GetType().Name);
            }

            return Page();
        }

        private async Task SetDropdownsAsync()
        {
            IList<string> tbServiceCodes;

            if (Notification.NotificationStatus == Models.Enums.NotificationStatus.Draft)
            {
                var services = await _userService.GetTbServicesAsync(User);
                if (Notification.Episode.TBService?.IsLegacy == true)
                {
                    services = services.Prepend(Notification.Episode.TBService);
                }
                tbServiceCodes = services.Select(s => s.Code).ToList();
                TbServices = new SelectList(services, nameof(TBService.Code), nameof(TBService.Name));
            }
            else
            {
                tbServiceCodes = new List<string> { Notification.Episode.TBServiceCode };
            }

            var hospitals = (await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(tbServiceCodes))
                .Where(h => 
                    h.IsLegacy == false 
                    || h.HospitalId == Notification.Episode.Hospital?.HospitalId);
            
            Hospitals = new SelectList(hospitals, nameof(Hospital.HospitalId), nameof(Hospital.Name));

            var caseManagers = await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(tbServiceCodes);
            CaseManagers = new SelectList(caseManagers, nameof(Models.Entities.User.Username), nameof(Models.Entities.User.FullName));
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./ClinicalDetails", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            await SetValuesForValidation();
            if (Notification.NotificationStatus != Models.Enums.NotificationStatus.Draft && Notification.Episode.TBServiceCode != Episode.TBServiceCode)
            {
                ModelState.AddModelError("Episode.TBServiceCode", ValidationMessages.TBServiceCantChange);
            }
            TryValidateModel(Episode, nameof(Episode));
            ValidationService.ValidateProperty(
                Notification,
                nameof(Notification),
                Notification.NotificationDate,
                nameof(Notification.NotificationDate));
            if (ModelState.IsValid)
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
            return ValidationService.GetPropertyValidationResult<Episode>(key, value, shouldValidateFull);
        }

        public async Task<ContentResult> OnGetValidateNotificationDateAsync(string key, string day, string month, string year, int notificationId)
        {
            // Query notification by Id when date validation depends on other properties of model
            Notification notification = await NotificationRepository.GetNotificationAsync(notificationId);
            return ValidationService.GetDateValidationResult(notification, key, day, month, year);
        }

        private async Task SetValuesForValidation()
        {
            Episode.SetValidationContext(Notification);
            ValidationService.TrySetFormattedDate(Notification, "Notification", nameof(Notification.NotificationDate), FormattedNotificationDate);
            /*
            Binding only sets the entity ids, but not the actual entities.
            There's a validation rule that needs to check the relationship between the entities,
            therefore we need fetch the reference data from the db before validating
            */
            await GetRelatedEntities();
        }

        private async Task GetRelatedEntities()
        {
            if (Episode.HospitalId != null)
            {
                Episode.Hospital = await _referenceDataRepository.GetHospitalByGuidAsync(Episode.HospitalId.Value);
            }
            if (Episode.TBServiceCode != null)
            {
                Episode.TBService = await _referenceDataRepository.GetTbServiceByCodeAsync(Episode.TBServiceCode);
            }
            if (Episode.CaseManagerUsername != null)
            {
                Episode.CaseManager = await _referenceDataRepository.GetCaseManagerByUsernameAsync(Episode.CaseManagerUsername);
            }
        }

        public async Task<JsonResult> OnGetGetFilteredListsByTbService(string value)
        {
            var tbServiceCodeAsList = new List<string> { value };
            var filteredHospitals = await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(tbServiceCodeAsList);
            var filteredCaseManagers = await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(tbServiceCodeAsList);

            return new JsonResult(
                new FilteredEpisodePageSelectLists
                {
                    Hospitals = filteredHospitals.Select(n => new OptionValue
                    {
                        Value = n.HospitalId.ToString(),
                        Text = n.Name
                    }),
                    CaseManagers = filteredCaseManagers.Select(n => new OptionValue
                    {
                        Value = n.Username,
                        Text = n.FullName
                    })
                });
        }
    }
}
