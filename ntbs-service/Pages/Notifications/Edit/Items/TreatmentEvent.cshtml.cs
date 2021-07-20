using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Items
{
    public class TreatmentEventModel : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly ITreatmentEventRepository _treatmentEventRepository;
        private readonly IAlertService _alertService;

        [BindProperty(SupportsGet = true)]
        public int? RowId { get; set; }
        [BindProperty]
        public TreatmentEvent TreatmentEvent { get; set; }
        [BindProperty]
        public FormattedDate FormattedEventDate { get; set; }

        [Display(Name = "Outcome value")]
        [BindProperty]
        public TreatmentOutcomeType? SelectedTreatmentOutcomeType { get; set; }
        [Display(Name = "Additional information")]
        [BindProperty]
        public TreatmentOutcomeSubType? SelectedTreatmentOutcomeSubType { get; set; }
        public IEnumerable<SelectListItem> TreatmentEventTypes { get; set; }

        public IList<TreatmentEvent> TreatmentEvents { get; private set; }

        public TreatmentEventModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            ITreatmentEventRepository treatmentEventRepository,
            IAlertRepository alertRepository,
            IAlertService alertService,
            IUserHelper userHelper)
            : base(service, authorizationService, notificationRepository, alertRepository, userHelper)
        {
            _referenceDataRepository = referenceDataRepository;
            _treatmentEventRepository = treatmentEventRepository;
            _alertService = alertService;
        }

        // Pragma disabled 'not using async' to allow auto-magical wrapping in a Task
#pragma warning disable 1998
        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
#pragma warning restore 1998
        {
            if (RowId != null)
            {
                TreatmentEvent = Notification.TreatmentEvents.SingleOrDefault(r => r.TreatmentEventId == RowId.Value);
                if (TreatmentEvent == null || !TreatmentEvent.IsEditable)
                {
                    return NotFound();
                }

                FormattedEventDate = TreatmentEvent.EventDate.ConvertToFormattedDate();
                if (TreatmentEvent.TreatmentOutcome != null)
                {
                    SelectedTreatmentOutcomeType = TreatmentEvent.TreatmentOutcome.TreatmentOutcomeType;
                    SelectedTreatmentOutcomeSubType = TreatmentEvent.TreatmentOutcome.TreatmentOutcomeSubType;
                }
            }

            TreatmentEventTypes = TreatmentEvent.EditableTreatmentEventTypes
                .Select(t => new SelectListItem
                {
                    Value = ((int)t).ToString(),
                    Text = t.GetDisplayName()
                });

            TreatmentEvents = Notification.GetTreatmentEventsWithStartingEvent().OrderForEpisodes().ToList();

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            TreatmentEvent.NotificationId = NotificationId;
            TreatmentEvent.CaseManagerId = Notification.HospitalDetails.CaseManagerId;
            TreatmentEvent.TbServiceCode = Notification.HospitalDetails.TBServiceCode;
            TreatmentEvent.IsNotificationPostMortem = Notification.ClinicalDetails.IsPostMortem ?? false;

            // The required date will be marked as missing on the model, since we are setting it manually, rather than binding it
            ModelState.Remove("TreatmentEvent.EventDate");
            ValidationService.TrySetFormattedDate(TreatmentEvent, nameof(TreatmentEvent), nameof(TreatmentEvent.EventDate), FormattedEventDate);

            // TreatmentOutcomeId will be marked as missing on the model, since setting manually
            ModelState.Remove("TreatmentEvent.TreatmentOutcomeId");

            // Add additional fields required for date validation
            TreatmentEvent.Dob = Notification.PatientDetails.Dob;
            TreatmentEvent.DateOfNotification = Notification.NotificationDate;

            switch (TreatmentEvent.TreatmentEventType)
            {
                case TreatmentEventType.TreatmentOutcome:
                    await TrySetOutcomeFromTypeAndSubTypeAsync();
                    break;
                case TreatmentEventType.TreatmentRestart:
                    TreatmentEvent.TreatmentOutcomeId = null;
                    break;
            }

            if (TryValidateModel(TreatmentEvent, nameof(TreatmentEvent)))
            {
                if (RowId == null)
                {
                    TreatmentEvent.CaseManagerId = Notification.HospitalDetails.CaseManagerId;
                    TreatmentEvent.TbServiceCode = Notification.HospitalDetails.TBServiceCode;
                    await _treatmentEventRepository.AddAsync(TreatmentEvent);
                }
                else
                {
                    TreatmentEvent.TreatmentEventId = RowId.Value;
                    await _treatmentEventRepository.UpdateAsync(Notification, TreatmentEvent);
                }
                await _alertService.AutoDismissAlertAsync<DataQualityTreatmentOutcome12>(Notification);
                await _alertService.AutoDismissAlertAsync<DataQualityTreatmentOutcome24>(Notification);
                await _alertService.AutoDismissAlertAsync<DataQualityTreatmentOutcome36>(Notification);
            }
            else
            {
                // Manual handling of TreatmentOutcomeId in switch statement below will cover all validation for property
                ModelState.Remove("TreatmentEvent.TreatmentOutcomeId");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            var (permissionLevel, _) = await _authorizationService.GetPermissionLevelAsync(User, Notification);
            if (permissionLevel != PermissionLevel.Edit)
            {
                return ForbiddenResult();
            }

            var treatmentEvent = Notification.TreatmentEvents
                .SingleOrDefault(s => RowId != null && s.TreatmentEventId == RowId.Value);

            if (treatmentEvent == null)
            {
                return NotFound();
            }

            await _treatmentEventRepository.DeleteAsync(treatmentEvent);

            return RedirectToPage("/Notifications/Edit/TreatmentEvents", new { NotificationId });
        }

        private async Task TrySetOutcomeFromTypeAndSubTypeAsync()
        {
            if (!SelectedTreatmentOutcomeType.HasValue)
            {
                ModelState.AddModelError(
                    nameof(SelectedTreatmentOutcomeType),
                    string.Format(
                        ValidationMessages.RequiredSelect,
                        this.GetMemberDisplayName(nameof(SelectedTreatmentOutcomeType))));
                return;
            }

            var treatmentOutcome = await _referenceDataRepository.GetTreatmentOutcomeForTypeAndSubType(
                SelectedTreatmentOutcomeType.Value,
                SelectedTreatmentOutcomeSubType);

            if (treatmentOutcome == null)
            {
                ModelState.AddModelError(
                    nameof(SelectedTreatmentOutcomeSubType),
                    ValidationMessages.SubTypeDoesNotCorrespondToOutcome);
                return;
            }

            TreatmentEvent.TreatmentOutcomeId = treatmentOutcome.TreatmentOutcomeId;
            TreatmentEvent.TreatmentOutcome = treatmentOutcome;
        }


        protected override IActionResult RedirectForNotified()
        {
            return RedirectToPage("/Notifications/Edit/TreatmentEvents", new { NotificationId });
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/TreatmentEvents", new { NotificationId });
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithTreatmentEventsAsync(notificationId);
        }

        public ContentResult OnPostValidateTreatmentEventProperty([FromBody]InputValidationModel validationData)
        {
            return ValidationService.GetPropertyValidationResult<TreatmentEvent>(validationData.Key, validationData.Value, true);
        }

        public async Task<ContentResult> OnPostValidateTreatmentEventDate([FromBody]DateValidationModel validationData)
        {
            var isLegacy = await NotificationRepository.IsNotificationLegacyAsync(NotificationId);
            return ValidationService.GetDateValidationResult<TreatmentEvent>(validationData.Key, validationData.Day, validationData.Month, validationData.Year, isLegacy);
        }

        public ContentResult OnPostValidateSelectedTreatmentOutcomeTypeProperty([FromBody]InputValidationModel validationData)
        {
            if (!int.TryParse(validationData.Value, out var outcomeType) || !Enum.IsDefined(typeof(TreatmentOutcomeType), outcomeType))
            {
                var errorMessage = string.Format(ValidationMessages.RequiredSelect,
                    this.GetMemberDisplayName(nameof(SelectedTreatmentOutcomeType)));
                return Content(errorMessage);
            }

            return ValidationService.ValidContent();
        }

        public async Task<JsonResult> OnGetFilteredOutcomeSubTypesForType(TreatmentOutcomeType? value)
        {
            if (value == null)
            {
                return new JsonResult(new FilteredTreatmentOutcomeTypes());
            }

            var filteredTreatmentOutcomes = await _referenceDataRepository.GetTreatmentOutcomesForType(value.Value);
            return new JsonResult(
                new FilteredTreatmentOutcomeTypes
                {
                    SubTypes = filteredTreatmentOutcomes
                        .Where(n => n.TreatmentOutcomeSubType != null)
                        .Select(n => OptionValue.FromEnum(n.TreatmentOutcomeSubType))
                });
        }
    }
}
