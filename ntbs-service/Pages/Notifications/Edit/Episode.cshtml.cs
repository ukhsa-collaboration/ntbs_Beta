using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.FilteredSelectLists;
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
            this._context = context;
            this._userService = userService;
            this._referenceDataRepository = referenceDataRepository;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            Episode = Notification.Episode;
            await SetNotificationProperties(isBeingSubmitted, Episode);
            await SetDropdownsAsync();
            FormattedNotificationDate = Notification.NotificationDate.ConvertToFormattedDate();

            if (Episode.ShouldValidateFull)
            {
                ValidationService.TrySetFormattedDate(Notification, "Notification", nameof(Notification.NotificationDate), FormattedNotificationDate);
                TryValidateModel(Episode, Episode.GetType().Name);
            }

            return Page();
        }

        private async Task SetDropdownsAsync()
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

            var caseManagers = await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(tbServiceCodes);

            // If case manager not yet set, and current user is an allowed case manager for
            // the current tbServices then set currentUser as default case manager
            if (Episode.CaseManagerEmail == null)
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var upperUserEmail = userEmail?.ToUpperInvariant();
                if (caseManagers.Any(c => c.Email.ToUpperInvariant() == upperUserEmail))
                {
                    Episode.CaseManagerEmail = userEmail;
                }
            };

            CaseManagers = new SelectList(caseManagers, nameof(CaseManager.Email), nameof(CaseManager.FullName));
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
            TryValidateModel(Notification, nameof(Notification));
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
            if (Episode.CaseManagerEmail != null)
            {
                Episode.CaseManager = await _referenceDataRepository.GetCaseManagerByEmailAsync(Episode.CaseManagerEmail);
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
                        Value = n.Email,
                        Text = n.FullName
                    })
                });
        }
    }
}
