using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ExpressiveAnnotations.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Alerts
{
    public class TransferRequestModel : NotificationModelBase
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAlertService _alertService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        public ValidationService ValidationService;

        public TransferAlert TransferAlert { get; set; }

        [BindProperty]
        public TransferRequest TransferRequest { get; set; }

        [BindProperty]
        public int AlertId { get; set; }
        public SelectList TbServices { get; set; }
        public SelectList CaseManagers { get; set; }
        public SelectList Phecs { get; set; }

        public TransferRequestModel(
            INotificationService notificationService,
            IAlertService alertService,
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) 
            : base(notificationService, authorizationService, notificationRepository)
        {
            _alertService = alertService;
            _alertRepository = alertRepository;
            _referenceDataRepository = referenceDataRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            await AuthorizeAndSetBannerAsync();
            // Check edit permission and redirect if not allowed
            if (PermissionLevel != PermissionLevel.Edit)
            {
                return RedirectToPage("/Notifications/Overview", new {NotificationId});
            }

            var pendingTransferAlert =
                await _alertRepository.GetOpenAlertByNotificationId<TransferAlert>(NotificationId);
            if (pendingTransferAlert != null)
            {
                TransferAlert = pendingTransferAlert;
                return Partial("_TransferPendingPartial", this);
            }

            TransferRequest = new TransferRequest();
            await SetDropdownsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            await GetRelatedEntities();
            ModelState.Clear();
            TryValidateModel(TransferRequest, nameof(TransferRequest));
            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                await AuthorizeAndSetBannerAsync();
                return Page();
            }

            var transferAlert = new TransferAlert
            {
                 TbServiceCode = TransferRequest.TbServiceCode,
                 CaseManagerUsername = TransferRequest.CaseManagerUsername,
                 TransferReason = TransferRequest.TransferReason,
                 OtherReasonDescription = TransferRequest.OtherReasonDescription,
                 TransferRequestNote = TransferRequest.TransferRequestNote
            };
            await _alertService.AddUniqueOpenAlertAsync(transferAlert);

            return RedirectToPage("/Notifications/Overview", new {NotificationId});
        }

        private async Task GetRelatedEntities()
        {
            if (TransferRequest.TbServiceCode != null)
            {
                TransferRequest.TbService =
                    await _referenceDataRepository.GetTbServiceByCodeAsync(TransferRequest.TbServiceCode);
            }

            if (TransferRequest.CaseManagerUsername != null)
            {
                TransferRequest.CaseManager =
                    await _referenceDataRepository.GetCaseManagerByUsernameAsync(TransferRequest.CaseManagerUsername);
            }

            TransferRequest.NotificationTbServiceCode = Notification.HospitalDetails.TBServiceCode;
        }

        private async Task SetDropdownsAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            TbServices = new SelectList(tbServices, nameof(TBService.Code), nameof(TBService.Name));
            var caseManagers = await _referenceDataRepository.GetAllCaseManagers();
            CaseManagers = new SelectList(caseManagers,
                nameof(Models.Entities.User.Username),
                nameof(Models.Entities.User.FullName));
            var phecs = await _referenceDataRepository.GetAllPhecs();
            Phecs = new SelectList(phecs, nameof(PHEC.Code), nameof(PHEC.Name));
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            TransferAlert = await _alertRepository.GetOpenAlertByNotificationId<TransferAlert>(NotificationId);
            await _alertService.DismissAlertAsync(TransferAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));

            NotificationBannerModel = new NotificationBannerModel(Notification);
            return Partial("_CancelTransferConfirmation", this);
        }

        public async Task<JsonResult> OnGetFilteredTbServiceListsByPhecCode(string value)
        {
            var filteredTbServices = await _referenceDataRepository.GetTbServicesFromPhecCodeAsync(value);

            return new JsonResult(
                new FilteredTransferPageSelectLists
                {
                    TbServices = filteredTbServices.Select(n => new OptionValue
                    {
                        Value = n.Code.ToString(),
                        Text = n.Name
                    }),
                    CaseManagers = new List<OptionValue>()
                });
        }

        public async Task<JsonResult> OnGetFilteredCaseManagersListByTbServiceCode(string value)
        {
            var filteredCaseManagers =
                await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(new List<string> {value});

            return new JsonResult(
                new FilteredCaseManagerList
                {
                    CaseManagers = filteredCaseManagers.Select(n => new OptionValue
                    {
                        Value = n.Username.ToString(),
                        Text = n.FullName
                    })
                });
        }
    }

    public class TransferRequest
    {
        [BindProperty]
        [Display(Name = "TB Service")]
        [Required(ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(nameof(TransferDestinationNotCurrentTbService),
            ErrorMessage = ValidationMessages.TransferDestinationCannotBeCurrentTbService)]
        public string TbServiceCode { get; set; }

        public virtual TBService TbService { get; set; }

        [BindProperty]
        [Display(Name = "Case Manager")]
        [AssertThat(nameof(CaseManagerAllowedForTbService),
            ErrorMessage = ValidationMessages.CaseManagerMustBeAllowedForSelectedTbService)]
        public string CaseManagerUsername { get; set; }

        public virtual User CaseManager { get; set; }

        [BindProperty]
        public TransferReason TransferReason { get; set; }

        [BindProperty]
        [Display(Name = "Other description")]
        [MaxLength(200)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string OtherReasonDescription { get; set; }

        [BindProperty]
        [Display(Name = "Optional note")]
        [MaxLength(200)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string TransferRequestNote { get; set; }


        public bool CaseManagerAllowedForTbService
        {
            // ReSharper disable once UnusedMember.Global - used in the validation
            get
            {
                // If email not set, or TBService missing (ergo navigation properties not yet retrieved) pass validation
                if (string.IsNullOrEmpty(CaseManagerUsername) || TbServiceCode == null)
                {
                    return true;
                }

                if (CaseManager?.CaseManagerTbServices == null || CaseManager.CaseManagerTbServices.Count == 0)
                {
                    return false;
                }

                return CaseManager.CaseManagerTbServices.Any(c => c.TbServiceCode == TbServiceCode);
            }
        }

        public string NotificationTbServiceCode { get; set; }

        public bool TransferDestinationNotCurrentTbService => TbServiceCode != NotificationTbServiceCode;
    }
}
