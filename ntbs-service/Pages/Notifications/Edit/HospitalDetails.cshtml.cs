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
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class HospitalDetailsModel : NotificationEditModelBase
    {
        private readonly NtbsContext _context;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserService _userService;

        public SelectList TbServices { get; set; }
        public SelectList Hospitals { get; set; }
        public SelectList CaseManagers { get; set; }
        public bool HasNonActiveCaseManager { get; set; }

        [BindProperty]
        public FormattedDate FormattedNotificationDate { get; set; }

        [BindProperty]
        public HospitalDetails HospitalDetails { get; set; }

        public HospitalDetailsModel(
            INotificationService notificationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository,
            IReferenceDataRepository referenceDataRepository,
            IAuthorizationService authorizationService,
            IUserService userService,
            NtbsContext context,
            IUserHelper userHelper) : base(notificationService, authorizationService, notificationRepository, alertRepository, userHelper)
        {
            _context = context;
            _userService = userService;
            _referenceDataRepository = referenceDataRepository;

            CurrentPage = NotificationSubPaths.EditHospitalDetails;
        }

        protected override Task<Notification> GetNotificationAsync(int notificationId)
        {
            return NotificationRepository.GetNotificationWithCaseManagerTbServicesAsync(notificationId);
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            HospitalDetails = Notification.HospitalDetails;
            await SetNotificationProperties(isBeingSubmitted, HospitalDetails);
            await SetDropdownsAsync();
            FormattedNotificationDate = Notification.NotificationDate.ConvertToFormattedDate();

            if (HospitalDetails.ShouldValidateFull)
            {
                ValidationService.TrySetFormattedDate(
                    Notification,
                    nameof(Notification),
                    nameof(Notification.NotificationDate),
                    FormattedNotificationDate);
                TryValidateModel(HospitalDetails, nameof(HospitalDetails));
            }

            return Page();
        }

        private async Task SetDropdownsAsync()
        {
            IList<string> tbServiceCodes;

            if (Notification.NotificationStatus == Models.Enums.NotificationStatus.Draft)
            {
                var services = await _userService.GetTbServicesAsync(User);
                if (Notification.HospitalDetails.TBService?.IsLegacy == true)
                {
                    services = services.Prepend(Notification.HospitalDetails.TBService);
                }

                tbServiceCodes = services.Select(s => s.Code).ToList();
                TbServices = new SelectList(services, nameof(TBService.Code), nameof(TBService.Name));
            }
            else
            {
                tbServiceCodes = new List<string> { Notification.HospitalDetails.TBServiceCode };
            }

            var hospitals = await GetActiveOrCurrentHospitalsByTbServiceCodesAsync(tbServiceCodes, Notification);

            Hospitals = new SelectList(hospitals, nameof(Hospital.HospitalId), nameof(Hospital.Name));

            var caseManagers = await GetCaseManagerDropdownValues(tbServiceCodes);
            CaseManagers = new SelectList(
                caseManagers,
                nameof(Models.Entities.User.Id),
                nameof(Models.Entities.User.DisplayName));
        }

        private async Task<IEnumerable<Hospital>> GetActiveOrCurrentHospitalsByTbServiceCodesAsync(
            IEnumerable<string> tbServices,
            Notification notification)
        {
            return (await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(tbServices))
                .Where(h =>
                    h.IsLegacy == false
                    || h.HospitalId == notification.HospitalDetails.Hospital?.HospitalId);
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./ClinicalDetails", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            await SetValuesForValidation();
            if (Notification.NotificationStatus != NotificationStatus.Draft && Notification.HospitalDetails.TBServiceCode != HospitalDetails.TBServiceCode)
            {
                ModelState.AddModelError("HospitalDetails.TBServiceCode", ValidationMessages.TBServiceCantChange);
            }
            TryValidateModel(HospitalDetails, nameof(HospitalDetails));
            ValidationService.ValidateProperty(
                Notification,
                nameof(Notification),
                Notification.NotificationDate,
                nameof(Notification.NotificationDate));
            if (ModelState.IsValid)
            {
                await Service.UpdateHospitalDetailsAsync(Notification, HospitalDetails);
            }
            else
            {
                // Detach notification to avoid getting cached notification when retrieving from context,
                // because cached notification date will change notification date on a banner even when invalid
                _context.Entry(Notification).State = EntityState.Detached;
            }
        }

        public ContentResult OnPostValidateHospitalDetailsProperty([FromBody]InputValidationModel validationData)
        {
            return ValidationService.GetPropertyValidationResult<HospitalDetails>(validationData.Key, validationData.Value, validationData.ShouldValidateFull);
        }

        public async Task<ContentResult> OnPostValidateNotificationDateAsync([FromBody]DateValidationModel validationData)
        {
            // Query notification by Id when date validation depends on other properties of model
            var notification = await NotificationRepository.GetNotificationAsync(validationData.NotificationId);
            return ValidationService.GetDateValidationResult(notification, validationData.Key, validationData.Day, validationData.Month, validationData.Year);
        }

        private async Task SetValuesForValidation()
        {
            HospitalDetails.NotificationId = NotificationId;
            HospitalDetails.ExistingCaseManagerId = Notification.HospitalDetails.CaseManagerId;
            HospitalDetails.SetValidationContext(Notification);
            ValidationService.TrySetFormattedDate(Notification, "Notification", nameof(Notification.NotificationDate), FormattedNotificationDate);
            /*
            Binding only sets the entity ids, but not the actual entities.
            There's a validation rule that needs to check the relationship between the entities,
            therefore we need fetch the reference data from the db before validating
            */
            await GetRelatedEntities();
        }

        private async Task<IList<User>> GetCaseManagerDropdownValues(IList<string> tbServiceCodes)
        {
            var caseManagersToReturn = await _referenceDataRepository.GetActiveCaseManagersByTbServiceCodesAsync(tbServiceCodes);
            if (HospitalDetails.CaseManager != null && !caseManagersToReturn.Select(u => u.Username).Contains(HospitalDetails.CaseManager.Username))
            {
                caseManagersToReturn.Add(Notification.HospitalDetails.CaseManager);
                HasNonActiveCaseManager = true;
            }

            return caseManagersToReturn;
        }

        private async Task GetRelatedEntities()
        {
            if (HospitalDetails.HospitalId.HasValue)
            {
                HospitalDetails.Hospital = await _referenceDataRepository.GetHospitalByGuidAsync(HospitalDetails.HospitalId.Value);
            }
            if (HospitalDetails.TBServiceCode != null)
            {
                HospitalDetails.TBService = await _referenceDataRepository.GetTbServiceByCodeAsync(HospitalDetails.TBServiceCode);
            }
            if (HospitalDetails.CaseManagerId.HasValue)
            {
                HospitalDetails.CaseManager = await _referenceDataRepository.GetUserByIdAsync(HospitalDetails.CaseManagerId.Value);
            }
        }

        public async Task<JsonResult> OnGetGetFilteredListsByTbService(string value)
        {
            var notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            var tbServiceCodeAsList = new List<string> { value };

            var filteredHospitals =
                await GetActiveOrCurrentHospitalsByTbServiceCodesAsync(tbServiceCodeAsList, notification);

            var filteredCaseManagers =
                await _referenceDataRepository.GetActiveCaseManagersByTbServiceCodesAsync(tbServiceCodeAsList);

            var filteredHospitalDetailsPageSelectLists = new FilteredHospitalDetailsPageSelectLists
            {
                Hospitals = filteredHospitals.Select(n => new OptionValue
                {
                    Value = n.HospitalId.ToString(),
                    Text = n.Name
                }),
                CaseManagers = filteredCaseManagers.Select(n => new OptionValue
                {
                    Value = n.Id.ToString(),
                    Text = n.DisplayName
                })
            };

            return new JsonResult(filteredHospitalDetailsPageSelectLists);
        }
    }
}
