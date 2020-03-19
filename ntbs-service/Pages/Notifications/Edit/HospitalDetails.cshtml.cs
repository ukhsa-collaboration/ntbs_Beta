using System;
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
        private readonly IUserService _userService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IItemRepository<TreatmentEvent> _treatmentEventRepository;

        public SelectList TbServices { get; set; }
        public SelectList Hospitals { get; set; }
        public SelectList CaseManagers { get; set; }

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
            IItemRepository<TreatmentEvent> treatmentEventRepository,
            NtbsContext context) : base(notificationService, authorizationService, notificationRepository, alertRepository)
        {
            _context = context;
            _userService = userService;
            _referenceDataRepository = referenceDataRepository;
            _treatmentEventRepository = treatmentEventRepository;

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
                TryValidateModel(HospitalDetails, HospitalDetails.GetType().Name);
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

            var hospitals = (await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(tbServiceCodes))
                .Where(h => 
                    h.IsLegacy == false 
                    || h.HospitalId == Notification.HospitalDetails.Hospital?.HospitalId);
            
            Hospitals = new SelectList(hospitals, nameof(Hospital.HospitalId), nameof(Hospital.Name));

            var caseManagers = await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(tbServiceCodes);
            CaseManagers = new SelectList(caseManagers, nameof(Models.Entities.User.Username), nameof(Models.Entities.User.FullName));
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./ClinicalDetails", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task ValidateAndSave()
        {
            FormattedNotificationDate.TryConvertToDateTime(out var notificationDate);
            var notificationDateHasChanged = notificationDate.HasValue && notificationDate != Notification.NotificationDate;
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
                if (notificationDateHasChanged)
                {
                    UpdateTreatmentStartEvent(notificationDate.Value);    
                }
            }
            else
            {
                // Detach notification to avoid getting cached notification when retrieving from context,
                // because cached notification date will change notification date on a banner even when invalid
                _context.Entry(Notification).State = EntityState.Detached;

            }
        }

        public ContentResult OnGetValidateHospitalDetailsProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<HospitalDetails>(key, value, shouldValidateFull);
        }

        public async Task<ContentResult> OnGetValidateNotificationDateAsync(string key, string day, string month, string year, int notificationId)
        {
            // Query notification by Id when date validation depends on other properties of model
            Notification notification = await NotificationRepository.GetNotificationAsync(notificationId);
            return ValidationService.GetDateValidationResult(notification, key, day, month, year);
        }
        
        private void UpdateTreatmentStartEvent(DateTime notificationDate)
        {
            if (Notification.ClinicalDetails.TreatmentStartDate == null)
            {
                var treatmentStartEvent = Notification.TreatmentEvents.SingleOrDefault(t => t.TreatmentEventType == TreatmentEventType.TreatmentStart);
                if (treatmentStartEvent == null)
                {
                    return;
                }

                treatmentStartEvent.EventDate = notificationDate;
                _treatmentEventRepository.UpdateAsync(Notification, treatmentStartEvent);
            }
        }

        private async Task SetValuesForValidation()
        {
            HospitalDetails.SetValidationContext(Notification);
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
            if (HospitalDetails.HospitalId != null)
            {
                HospitalDetails.Hospital = await _referenceDataRepository.GetHospitalByGuidAsync(HospitalDetails.HospitalId.Value);
            }
            if (HospitalDetails.TBServiceCode != null)
            {
                HospitalDetails.TBService = await _referenceDataRepository.GetTbServiceByCodeAsync(HospitalDetails.TBServiceCode);
            }
            if (HospitalDetails.CaseManagerUsername != null)
            {
                HospitalDetails.CaseManager = await _referenceDataRepository.GetCaseManagerByUsernameAsync(HospitalDetails.CaseManagerUsername);
            }
        }

        public async Task<JsonResult> OnGetGetFilteredListsByTbService(string value)
        {
            var filteredHospitalDetailsPageSelectLists = 
                await _referenceDataRepository.GetFilteredHospitalDetailsPageSelectListsByTbService(value);
            return new JsonResult(filteredHospitalDetailsPageSelectLists);
        }
    }
}
