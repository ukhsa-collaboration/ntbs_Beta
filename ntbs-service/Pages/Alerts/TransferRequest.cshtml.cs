using System;
using System.Collections.Generic;
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

        [BindProperty]
        public TransferAlert TransferAlert { get; set; }

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
            await AuthorizeAndSetBannerAsync();
            // Check edit permission and redirect if not allowed
            if (PermissionLevel != PermissionLevel.Edit)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }
            
            var pendingTransferAlert = (TransferAlert)await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest);
            if (pendingTransferAlert != null)
            {
                TransferAlert = pendingTransferAlert;
                return Partial("_TransferPendingPartial", this);
            }
            else
            {
                TransferAlert = new TransferAlert();
            }

            await SetDropdownsAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            await GetRelatedEntities();
            ModelState.Clear();
            TryValidateModel(TransferAlert, nameof(TransferAlert));
            if(!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                await AuthorizeAndSetBannerAsync();
                return Page();
            }
            await _alertService.AddUniqueOpenAlertAsync(TransferAlert);

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        private async Task GetRelatedEntities()
        {
            if (TransferAlert.TbServiceCode != null)
            {
                TransferAlert.TbService = await _referenceDataRepository.GetTbServiceByCodeAsync(TransferAlert.TbServiceCode);
            }
            if (TransferAlert.CaseManagerUsername != null)
            {
                TransferAlert.CaseManager = await _referenceDataRepository.GetCaseManagerByUsernameAsync(TransferAlert.CaseManagerUsername);
            }
            TransferAlert.NotificationTbServiceCode = Notification.HospitalDetails.TBServiceCode;
        }

        private async Task SetDropdownsAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            TbServices = new SelectList(tbServices, nameof(TBService.Code), nameof(TBService.Name));
            var caseManagers = await _referenceDataRepository.GetAllCaseManagers();
            CaseManagers = new SelectList(caseManagers, nameof(Models.Entities.User.Username), nameof(Models.Entities.User.FullName));
            var phecs = await _referenceDataRepository.GetAllPhecs();
            Phecs = new SelectList(phecs, nameof(PHEC.Code), nameof(PHEC.Name));
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            TransferAlert = (TransferAlert)(await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest));
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
                    CaseManagers = new List<OptionValue> {}
                });
        }

        public async Task<JsonResult> OnGetFilteredCaseManagersListByTbServiceCode(string value)
        {
            var filteredCaseManagers = await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(new List<string> {value});

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
}
