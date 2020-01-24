using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class TransferRequestActionModel : NotificationModelBase
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAlertService _alertService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        public ValidationService ValidationService;
        [BindProperty]
        [Required(ErrorMessage = "Please accept or decline the transfer")]
        public bool? AcceptTransfer { get; set; }
        [BindProperty]
        [MaxLength(200)]
        [RegularExpression(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, 
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [Display(Name = "Explanatory comment")]
        public string DeclineTransferReason { get; set; }

        [BindProperty]
        public int AlertId { get; set; }
        public TransferAlert TransferAlert { get; set; }


        public TransferRequestActionModel(
            INotificationService notificationService,
            IAlertService alertService, 
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) : base(notificationService, authorizationService, notificationRepository)
        {
            _alertService = alertService;
            _alertRepository = alertRepository;
            _referenceDataRepository = referenceDataRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            TransferAlert = (TransferAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest);
            await AuthorizeAndSetBannerAsync();
            
            // Check edit permission of user and redirect if not allowed
            if (!await AuthorizationService.IsUserAuthorizedToManageAlert(User, TransferAlert))
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }
            
            if (TransferAlert == null)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            TransferAlert = (TransferAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest);
            await AuthorizeAndSetBannerAsync();
            if(!ModelState.IsValid)
            {
                return Page();
            }

            if(AcceptTransfer == true)
            {
                await AcceptTransferAndDismissAlertAsync();
                return Partial("_AcceptedTransferConfirmation", this);
            }
            
            await RejectTransferAndDismissAlertAsync();
            return Partial("_RejectedTransferConfirmation", this);
        }

        public async Task AcceptTransferAndDismissAlertAsync()
        {
            Notification.Episode.TBServiceCode = TransferAlert.TbServiceCode;
            Notification.Episode.CaseManagerUsername = TransferAlert.CaseManagerUsername;
            // Set hospital to the first available hospital in the new TB service where it can be updated by the user after the transfer
            Notification.Episode.HospitalId =
                (await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(new List<string>
                {
                    TransferAlert.TbServiceCode
                })).FirstOrDefault()?.HospitalId;
            await Service.UpdateEpisodeAsync(Notification, Notification.Episode);
            await _alertService.DismissAlertAsync(TransferAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));
        }

        public async Task RejectTransferAndDismissAlertAsync()
        {
            var user = await _referenceDataRepository.GetCaseManagerByUsernameAsync(User.FindFirstValue(ClaimTypes.Email));
            var transferRejectedAlert = new TransferRejectedAlert()
            {
                CaseManagerUsername = Notification.Episode.CaseManagerUsername,
                NotificationId = NotificationId,
                RejectionReason = DeclineTransferReason,
                CaseManagerTbServiceString = $"{user.DisplayName}, {TransferAlert.TbServiceName}",
                TbServiceCode = Notification.Episode.TBServiceCode
            };

            // Dismiss any existing transfer rejected alert so that the new one can be created
            var pendingTransferRejectedAlert = (TransferRejectedAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId,
                                                                                                                                      AlertType.TransferRejected);
            if (pendingTransferRejectedAlert != null)
            {
                await _alertService.DismissAlertAsync(pendingTransferRejectedAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));
            }
            await _alertService.AddUniqueOpenAlertAsync(transferRejectedAlert);
            await _alertService.DismissAlertAsync(TransferAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));
        }
    }
}
