using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Models.ViewModels;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.AlertsAndActions
{
    public class TransferRequestActionModel : NotificationModelBase
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAlertService _alertService;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly ITreatmentEventRepository _treatmentEventRepository;

        public ValidationService ValidationService;

        [BindProperty]
        public TransferRequestActionViewModel TransferRequest { get; set; }
        
        [BindProperty]
        public FormattedDate FormattedTransferDate { get; set; }


        public TransferRequestActionModel(
            INotificationService notificationService,
            IAlertService alertService,
            IAlertRepository alertRepository,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            ITreatmentEventRepository treatmentEventRepository,
            IUserHelper userHelper)
            : base(notificationService, authorizationService, userHelper, notificationRepository)
        {
            _treatmentEventRepository = treatmentEventRepository;
            _alertService = alertService;
            _alertRepository = alertRepository;
            _referenceDataRepository = referenceDataRepository;
            ValidationService = new ValidationService(this);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            TransferRequest = new TransferRequestActionViewModel();
            await GetNotificationAndAlert();

            // Check edit permission of user and redirect if not allowed
            if (TransferRequest.TransferAlert is null || !await _authorizationService.IsUserAuthorizedToManageAlert(User, TransferRequest.TransferAlert))
            {
                return RedirectToPage("/Notifications/Overview", new { NotificationId });
            }

            await SetDropdownsAsync();
            TransferRequest.TargetCaseManagerId = TransferRequest.TransferAlert.CaseManagerId;
            TransferRequest.TransferDate = TransferRequest.TransferAlert.TransferDate ?? DateTime.Now.Date;
            FormattedTransferDate = TransferRequest.TransferDate.ConvertToFormattedDate();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await GetNotificationAndAlert();
            ValidationService.TrySetFormattedDate(TransferRequest, "TransferRequest", nameof(TransferRequest.TransferDate), FormattedTransferDate);
            ModelState.Clear();
            TryValidateModel(TransferRequest, nameof(TransferRequest));
            
            if (TransferRequest.AcceptTransfer != true)
            {
                ModelState.Remove("TransferRequest.TransferDate");
            }
            
            if (!ModelState.IsValid)
            {
                await SetDropdownsAsync();
                await AuthorizeAndSetBannerAsync();
                return Page();
            }

            if (TransferRequest.AcceptTransfer == true)
            {
                await AcceptTransferAndDismissAlertAsync();
                await AuthorizeAndSetBannerAsync();
                return Partial("_AcceptedTransferConfirmation", this);
            }

            await RejectTransferAndDismissAlertAsync();
            return Partial("_RejectedTransferConfirmation", this);
        }

        private async Task SetDropdownsAsync()
        {
            var hospitals = await _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(
                new List<string> { TransferRequest.TransferAlert.TbServiceCode });
            TransferRequest.Hospitals = new SelectList(hospitals,
                nameof(Hospital.HospitalId),
                nameof(Hospital.Name));
            var caseManagers = await _referenceDataRepository.GetActiveCaseManagersByTbServiceCodesAsync(
                new List<string> { TransferRequest.TransferAlert.TbServiceCode });
            TransferRequest.CaseManagers = new SelectList(caseManagers,
                nameof(Models.Entities.User.Id),
                nameof(Models.Entities.User.DisplayName));
        }

        public async Task AcceptTransferAndDismissAlertAsync()
        {
            var transferDate = TransferRequest.LatestTransferDate?.Date == TransferRequest.TransferDate.Value.Date
                ? TransferRequest.LatestTransferDate.Value.AddMinutes(1)
                : TransferRequest.TransferDate.Value;
            var transferOutEvent = new TreatmentEvent
            {
                NotificationId = NotificationId,
                EventDate = transferDate,
                TreatmentEventType = TreatmentEventType.TransferOut,
                CaseManagerId = Notification.HospitalDetails.CaseManagerId,
                TbServiceCode = Notification.HospitalDetails.TBServiceCode,
                Note = TransferRequest.TransferAlert.TransferRequestNote
            };
            var transferInEvent = new TreatmentEvent
            {
                NotificationId = NotificationId,
                EventDate = transferDate.AddSeconds(1),
                TreatmentEventType = TreatmentEventType.TransferIn,
                CaseManagerId = TransferRequest.TransferAlert.CaseManagerId,
                TbServiceCode = TransferRequest.TransferAlert.TbServiceCode,
                Note = TransferRequest.TransferAlert.TransferRequestNote
            };

            var previousTbService = new PreviousTbService()
            {
                TransferDate = transferDate,
                TbServiceCode = Notification.HospitalDetails.TBServiceCode,
                PhecCode = Notification.HospitalDetails?.TBService?.PHECCode
            };

            Notification.PreviousTbServices.Add(previousTbService);
            Notification.HospitalDetails.TBServiceCode = TransferRequest.TransferAlert.TbServiceCode;
            Notification.HospitalDetails.HospitalId = TransferRequest.TargetHospitalId;
            Notification.HospitalDetails.CaseManagerId = TransferRequest.TargetCaseManagerId;

            // Drop consultant and local patient ID because these are almost certainly going to be different at the new
            // TB service, as per NTBS-1559
            Notification.HospitalDetails.Consultant = null;
            Notification.PatientDetails.LocalPatientId = null;
            // Drop any shared service, see NTBS-2832
            Notification.HospitalDetails.SecondaryTBServiceCode = null;
            Notification.HospitalDetails.ReasonForTBServiceShare = null;
            // We want to save all these changes in the same transaction so that we make sure the time between the first
            // and last audit logs are within the timeframe for them to be grouped together on the changes page. We
            // experienced an issue where the audits from an acceptance were over this threshold and our app didn't
            // know how to handle the group. See NTBS-2457
            Service.UpdatePatientDetailsWithoutSave(Notification, Notification.PatientDetails);
            Service.UpdateHospitalDetailsWithoutSave(Notification, Notification.HospitalDetails);
            _treatmentEventRepository.AddWithoutSave(transferOutEvent);
            _treatmentEventRepository.AddWithoutSave(transferInEvent);
            await _alertService.DismissAlertWithoutSaveAsync(TransferRequest.TransferAlert.AlertId, User.Username());
            await _treatmentEventRepository.SaveChangesAsync(NotificationAuditType.Edited);
        }

        public async Task RejectTransferAndDismissAlertAsync()
        {
            var user = await _referenceDataRepository.GetUserByUsernameAsync(User.Username());
            var transferRejectedAlert = new TransferRejectedAlert
            {
                NotificationId = NotificationId,
                RejectionReason = TransferRequest.DeclineTransferReason ?? "No reason was given when declining this transfer.",
                DecliningUserAndTbServiceString = $"{user.DisplayName}, {TransferRequest.TransferAlert.TbService.Name}"
            };

            // Dismiss any existing transfer rejected alert so that the new one can be created
            var pendingTransferRejectedAlert =
                await _alertRepository.GetOpenAlertByNotificationId<TransferRejectedAlert>(NotificationId);
            if (pendingTransferRejectedAlert != null)
            {
                await _alertService.DismissAlertAsync(pendingTransferRejectedAlert.AlertId, User.Username());
            }
            await _alertService.AddUniqueOpenAlertAsync(transferRejectedAlert);
            await _alertService.DismissAlertAsync(TransferRequest.TransferAlert.AlertId, User.Username());
        }

        private async Task GetNotificationAndAlert()
        {
            await GetNotificationAndSetValuesForValidation();
            TransferRequest.TransferAlert = await _alertRepository.GetOpenTransferAlertByNotificationId(NotificationId);
            await AuthorizeAndSetBannerAsync();
        }

        private async Task GetNotificationAndSetValuesForValidation()
        {
            Notification = await GetNotificationAsync(NotificationId);
            TransferRequest.NotificationStartDate =
                NotificationHelper.Earliest(new[] {
                    Notification.ClinicalDetails.DiagnosisDate,
                    Notification.ClinicalDetails.TreatmentStartDate,
                    Notification.NotificationDate});
            TransferRequest.LatestTransferDate = Notification.TreatmentEvents.OrderForEpisodes()
                .LastOrDefault(te => te.TreatmentEventType == TreatmentEventType.TransferIn)?.EventDate;
        }

        public async Task<ContentResult> OnPostValidateTransferRequestDate([FromBody] DateValidationModel validationData)
        {
            await GetNotificationAndSetValuesForValidation();
            var transferWithValidationData = new TransferViewModel
            {
                NotificationStartDate = TransferRequest.NotificationStartDate,
                LatestTransferDate = TransferRequest.LatestTransferDate
            };
            return ValidationService.GetDateValidationResult(transferWithValidationData, validationData.Key, validationData.Day, validationData.Month, validationData.Year);
        }
    }
}
