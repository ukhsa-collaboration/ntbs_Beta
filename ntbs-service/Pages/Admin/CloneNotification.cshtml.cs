using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    // ReSharper disable once ClassNeverInstantiated.Global
    public class CloneNotification : PageModel
    {
        private readonly ValidationService _validationService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly INotificationCloningService _cloningService;

        public SelectList TbServices { get; }
        public SelectList Hospitals { get; }
        public SelectList CaseManagers { get; }

        public CloneNotification(
            IReferenceDataRepository referenceDataRepository,
            INotificationRepository notificationRepository,
            INotificationCloningService cloningService)
        {
            _referenceDataRepository = referenceDataRepository;
            _notificationRepository = notificationRepository;
            _cloningService = cloningService;
            _validationService = new ValidationService(this);

            var services = referenceDataRepository.GetAllTbServicesAsync().Result;
            TbServices = new SelectList(services, nameof(TBService.Code), nameof(TBService.Name));

            var hospitals = referenceDataRepository.GetHospitalsByTbServiceCodesAsync(services.Select(s => s.Code))
                .Result;
            Hospitals = new SelectList(hospitals, nameof(Hospital.HospitalId), nameof(Hospital.Name));

            var caseManagers = referenceDataRepository.GetAllCaseManagers().Result;
            CaseManagers = new SelectList(caseManagers,
                nameof(Models.Entities.User.Username),
                nameof(Models.Entities.User.FullName));
        }

        [BindProperty(SupportsGet = true)]
        [Display(Name = "Source notification id")]
        public int? NotificationId { get; set; }

        [BindProperty(SupportsGet = true)]

        public string TBServiceCode { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid? HospitalId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CaseManagerUsername { get; set; }

        [BindProperty(SupportsGet = true)]
        [StringLength(35)]
        [RegularExpression(ValidationRegexes.CharacterValidation,
            ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Given name")]
        public string GivenName { get; set; }

        [BindProperty(SupportsGet = true)]
        [StringLength(35)]
        [RegularExpression(ValidationRegexes.CharacterValidation,
            ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }

        // ReSharper disable once UnusedMember.Global
        public void OnGet()
        {
        }

        // ReSharper disable once UnusedMember.Global
        public async Task<IActionResult> OnPostAsync()
        {
            if (NotificationId == null)
            {
                ModelState.AddModelError(nameof(NotificationId), "Notification Id is required");
                return Page();
            }

            var notificationToClone = await _notificationRepository.GetNotificationWithAllInfoAsync(NotificationId.Value);
            if (notificationToClone == null)
            {
                ModelState.AddModelError(nameof(NotificationId), "Notification not found");
                return Page();
            }

            var clone = _cloningService.Clone(notificationToClone);
            OverrideValues(clone);

            await _notificationRepository.AddNotificationAsync(notificationToClone);
            return RedirectToPage("/Notifications/Overview", new { notificationToClone.NotificationId });
        }

        private void OverrideValues(Notification clone)
        {
            if (TBServiceCode != null)
            {
                clone.HospitalDetails.TBServiceCode = TBServiceCode;
            }

            if (HospitalId != null)
            {
                clone.HospitalDetails.HospitalId = HospitalId;
            }

            if (CaseManagerUsername != null)
            {
                clone.HospitalDetails.CaseManagerUsername = CaseManagerUsername;
            }

            if (GivenName != null)
            {
                clone.PatientDetails.GivenName = GivenName;
            }

            if (FamilyName != null)
            {
                clone.PatientDetails.FamilyName = FamilyName;
            }
        }

        // ReSharper disable once UnusedMember.Global
        public ContentResult OnGetValidateProperty(string key)
        {
            return _validationService.GetValidationResult(this, key);
        }

        // ReSharper disable once UnusedMember.Global
        public async Task<JsonResult> OnGetGetFilteredListsByTbService(string value)
        {
            var filteredHospitalDetailsPageSelectLists =
                await _referenceDataRepository.GetFilteredHospitalDetailsPageSelectListsByTbService(value);
            return new JsonResult(filteredHospitalDetailsPageSelectLists);
        }

        public bool IsValid(string key)
        {
            return _validationService.IsValid(key);
        }
    }
}
