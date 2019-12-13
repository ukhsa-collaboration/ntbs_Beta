using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.Validations;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Children
{
    public class TreatmentEventModel : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IItemRepository<TreatmentEvent> _treatmentEventRepository;

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

        public TreatmentEventModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IItemRepository<TreatmentEvent> treatmentEventRepository)
            : base(service, authorizationService, notificationRepository)
        {
            _referenceDataRepository = referenceDataRepository;
            _treatmentEventRepository = treatmentEventRepository;
        }

        // Pragma disabled 'not using async' to allow auto-magical wrapping in a Task
#pragma warning disable 1998
        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
#pragma warning restore 1998
        {
            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                return NotFound();
            }

            if (RowId != null)
            {
                TreatmentEvent = Notification.TreatmentEvents.SingleOrDefault(r => r.TreatmentEventId == RowId.Value);
                if (TreatmentEvent == null)
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

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            TreatmentEvent.NotificationId = NotificationId;

            // The required date will be marked as missing on the model, since we are setting it manually, rather than binding it
            ModelState.Remove("TreatmentEvent.EventDate");
            ValidationService.TrySetFormattedDate(TreatmentEvent, nameof(TreatmentEvent), nameof(TreatmentEvent.EventDate), FormattedEventDate);

            // Add additional fields required for date validation
            TreatmentEvent.Dob = Notification.PatientDetails.Dob;
            TreatmentEvent.DateOfNotification = Notification.NotificationDate;

            // Manual handling of TreatmentOutcomeId below will cover all validation on property
            ModelState.Remove("TreatmentEvent.TreatmentOutcomeId");
            if (TreatmentEvent.TreatmentEventType == TreatmentEventType.TreatmentOutcome)
            {
                await TrySetOutcomeFromTypeAndSubTypeAsync();
            }
            else if (TreatmentEvent.TreatmentEventType == TreatmentEventType.TreatmentRestart)
            {
                TreatmentEvent.TreatmentOutcomeId = null;
            }

            if (TryValidateModel(TreatmentEvent, nameof(TreatmentEvent)))
            {
                if (RowId == null)
                {
                    await _treatmentEventRepository.AddAsync(TreatmentEvent);
                }
                else
                {
                    TreatmentEvent.TreatmentEventId = RowId.Value;
                    await _treatmentEventRepository.UpdateAsync(Notification, TreatmentEvent);
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (!(await AuthorizationService.CanEdit(User, Notification)))
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
                        this.GetMemberDisplayNameValue(nameof(SelectedTreatmentOutcomeType))));
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
        }


        protected override IActionResult RedirectAfterSaveForNotified()
        {
            return RedirectToPage("/Notifications/Edit/TreatmentEvents", new { NotificationId });
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            throw new System.NotImplementedException();
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithTreatmentEventsAsync(notificationId);
        }

        public ContentResult OnGetValidateTreatmentEventProperty(string key, string value)
        {
            return ValidationService.ValidateModelProperty<TreatmentEvent>(key, value, true);
        }

        public ContentResult OnGetValidateTreatmentEventDate(string key, string day, string month, string year)
        {
            return ValidationService.ValidateDate<TreatmentEvent>(key, day, month, year);
        }

        public ContentResult OnGetValidateSelectedTreatmentOutcomeTypeProperty(string key, TreatmentOutcomeType? value)
        {
            if (value == null)
            {
                var errorMessage = string.Format(ValidationMessages.RequiredSelect,
                    this.GetMemberDisplayNameValue(nameof(SelectedTreatmentOutcomeType)));
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
                        .Select(n => new OptionValue
                        {
                            Value = ((int?)n.TreatmentOutcomeSubType).ToString(),
                            Text = n.TreatmentOutcomeSubType?.GetDisplayName()
                        })
                });
        }
    }
}
