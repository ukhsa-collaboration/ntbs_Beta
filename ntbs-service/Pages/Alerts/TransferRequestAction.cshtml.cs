using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ValidationService ValidationService;
        [BindProperty]
        [Required]
        public bool? AcceptTransfer { get; set; }
        [BindProperty]
        [RegularExpression(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended)]
        public string DeclineTransferReason { get; set; }

        [BindProperty]
        public TransferAlert TransferAlert { get; set; }

        [BindProperty]
        public int AlertId { get; set; }

        public TransferRequestActionModel(
            INotificationService notificationService,
            IAlertService alertService, 
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(notificationService, authorizationService, notificationRepository)
        {
            _alertService = alertService;
            _alertRepository = alertRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            TransferAlert = (TransferAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest);
            await AuthorizeAndSetBannerAsync();
            // Check edit permission OF ALERT and redirect if not allowed
            // if (!HasEditPermission)
            // {
            //     return RedirectToPage("/Notifications/Overview", new { NotificationId });
            // }
            
            if (TransferAlert == null)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            // TryValidateModel(TransferAlert, nameof(TransferAlert));
            if(!ModelState.IsValid)
            {
                await AuthorizeAndSetBannerAsync();
                return Page();
            }

            if(AcceptTransfer == true)
            {
                Notification.Episode.TBServiceCode = TransferAlert.TbServiceCode;
                Notification.Episode.CaseManagerUsername = TransferAlert.CaseManagerUsername;
                await _alertService.DismissAlertAsync(TransferAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));
                await AuthorizeAndSetBannerAsync();
                return Partial("_AcceptedTransferConfirmation", this);
            }
            
            await _alertService.DismissAlertAsync(TransferAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));
            await AuthorizeAndSetBannerAsync();
            return Partial("_RejectedTransferConfirmation", this);
        }
    }
}