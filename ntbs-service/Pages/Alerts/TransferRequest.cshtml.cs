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
        private readonly IAlertRepository alertRepository;
        private readonly IAlertService alertService;
        private readonly IAuthorizationService authorizationService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        public ValidationService ValidationService { get; set; }

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
            this.alertService = alertService;
            this.alertRepository = alertRepository;
            this.authorizationService = authorizationService;
            this._referenceDataRepository = referenceDataRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            // TODO authorise for TB service on alert and notification
            await AuthorizeAndSetBannerAsync();
            // Get alert if it exists
            var alert = await alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest);
            if (alert != null)
            {
                TransferAlert = (TransferAlert)alert;
                // if user has access to tb service on alert
                // return Partial("_TransferAccept", this)
                return Partial("_TransferPendingPartial", this);
            }
            else
            {
                TransferAlert = new TransferAlert();
            }


            // Check edit permission and redirect if not allowed
            if (!HasEditPermission)
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            TbServices = new SelectList(tbServices, nameof(TBService.Code), nameof(TBService.Name));
            var caseManagers = await _referenceDataRepository.GetAllCaseManagers();
            CaseManagers = new SelectList(caseManagers, nameof(CaseManager.Email), nameof(CaseManager.FullName));
            var phecs = await _referenceDataRepository.GetAllPhecs();
            Phecs = new SelectList(phecs, nameof(PHEC.Code), nameof(PHEC.Name));

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (!(await AuthorizationService.CanEditNotificationAsync(User, Notification)))
            {
                return ForbiddenResult();
            }
            if(!ModelState.IsValid)
            {
                NotificationBannerModel = new NotificationBannerModel(Notification);
                var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
                TbServices = new SelectList(tbServices, nameof(TBService.Code), nameof(TBService.Name));
                var caseManagers = await _referenceDataRepository.GetAllCaseManagers();
                CaseManagers = new SelectList(caseManagers, nameof(CaseManager.Email), nameof(CaseManager.FullName));
                var phecs = await _referenceDataRepository.GetAllPhecs();
                Phecs = new SelectList(phecs, nameof(PHEC.Code), nameof(PHEC.Name));
                return Page();
            }
            await alertService.AddUniqueOpenAlertAsync(TransferAlert);

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        public async Task<IActionResult> OnPostCancelAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (!await AuthorizationService.CanEditNotificationAsync(User, Notification))
            {
                return ForbiddenResult();
            }
            TransferAlert = (TransferAlert)(await alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(NotificationId, AlertType.TransferRequest));
            await alertService.DismissAlertAsync(TransferAlert.AlertId, User.FindFirstValue(ClaimTypes.Email));

            NotificationBannerModel = new NotificationBannerModel(Notification);
            return Partial("_CancelTransferConfirmation", this);
        }

        public async Task<IActionResult> OnPostAcceptAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (!await AuthorizationService.CanEditNotificationAsync(User, Notification))
            {
                return ForbiddenResult();
            }
            Notification.Episode.CaseManagerEmail = TransferAlert.CaseManagerEmail;
            Notification.Episode.TBServiceCode = TransferAlert.TbServiceCode;

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

         public async Task<IActionResult> OnPostDeclineAsync()
        {
            Notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            if (!await AuthorizationService.CanEditNotificationAsync(User, Notification))
            {
                return ForbiddenResult();
            }

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        public async Task<JsonResult> OnGetGetFilteredTbServiceListsByPhecCode(string value)
        {
            var filteredTbServices = await _referenceDataRepository.GetTbServicesFromPhecCodeAsync(value);

            return new JsonResult(
                new FilteredTransferPageSelectLists
                {
                    TbServices = filteredTbServices.Select(n => new OptionValue
                    {
                        Value = n.Code.ToString(),
                        Text = n.Name
                    })
                });
        }

        public async Task<JsonResult> OnGetGetFilteredCaseManagersListByTbServiceCode(string value)
        {
            var filteredCaseManagers = await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(new List<string> {value});

            return new JsonResult(
                new FilteredTransferPageSelectLists
                {
                    CaseManagers = filteredCaseManagers.Select(n => new OptionValue
                    {
                        Value = n.Email.ToString(),
                        Text = n.FullName
                    })
                });
        }

    }
}