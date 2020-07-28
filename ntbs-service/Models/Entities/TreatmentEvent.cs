using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Display(Name = "Treatment event")]
    public class TreatmentEvent : ModelBase, IHasRootEntityForAuditing
    {
        public int TreatmentEventId { get; set; }

        [Display(Name = "Event Date")]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [ValidClinicalDate]
        [AssertThat(nameof(EventDateAfterDob), ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [AssertThat(nameof(AfterNotificationDateUnlessPostMortem), ErrorMessage = ValidationMessages.DateShouldBeLaterThanNotification)]
        public DateTime? EventDate { get; set; }

        [Display(Name = "Event")]
        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        public TreatmentEventType? TreatmentEventType { get; set; }

        [RequiredIf(nameof(TreatmentEventTypeIsOutcome), ErrorMessage = ValidationMessages.TreatmentOutcomeRequiredForOutcome)]
        [AssertThat(nameof(TreatmentEventTypeIsNotRestart), ErrorMessage = ValidationMessages.TreatmentOutcomeInvalidForRestart)]
        public int? TreatmentOutcomeId { get; set; }
        public virtual TreatmentOutcome TreatmentOutcome { get; set; }

        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        public string Note { get; set; }

        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        
        public string CaseManagerUsername { get; set; }
        public User CaseManager { get; set; }

        [NotMapped]
        public DateTime? Dob { get; set; }
        [NotMapped]
        public DateTime? DateOfNotification { get; set; }
        [NotMapped]
        public bool IsNotificationPostMortem { get; set; }
        
        public bool TreatmentEventTypeIsOutcome => TreatmentEventType == Enums.TreatmentEventType.TreatmentOutcome;
        public bool TreatmentEventTypeIsNotRestart => TreatmentEventType != Enums.TreatmentEventType.TreatmentRestart;
        private bool TreatmentEventIsDeathEvent => TreatmentOutcome?.TreatmentOutcomeType == TreatmentOutcomeType.Died;
        public bool AfterNotificationDateUnlessPostMortem => EventDateAfterNotificationDate || (IsNotificationPostMortem && TreatmentEventIsDeathEvent);
        public bool EventDateAfterDob => Dob == null || EventDate >= Dob;
        private bool EventDateAfterNotificationDate => DateOfNotification == null || EventDate >= DateOfNotification;
        public string FormattedEventDate => EventDate.ConvertToString();

        public string FormattedEventTypeAndOutcome => GetFormattedEventTypeAndOutcome();
        private string GetFormattedEventTypeAndOutcome()
        {
            var eventAndValue = new StringBuilder();

            if (TreatmentEventType == null)
            {
                return eventAndValue.ToString();
            }
            eventAndValue.Append(TreatmentEventType.GetDisplayName());

            if (TreatmentOutcome == null)
            {
                return eventAndValue.ToString();
            }
            eventAndValue.Append($" - {TreatmentOutcome.TreatmentOutcomeType.GetDisplayName()}");

            return eventAndValue.ToString();
        }

        string IHasRootEntityForAuditing.RootEntityType => RootEntities.Notification;
        string IHasRootEntityForAuditing.RootId => NotificationId.ToString();

        public static IList<TreatmentEventType> EditableTreatmentEventTypes => new List<TreatmentEventType>
        {
            Enums.TreatmentEventType.TreatmentOutcome,
            Enums.TreatmentEventType.TreatmentRestart
        };

        private static IList<TreatmentEventType> StartingTreatmentEventTypes => new List<TreatmentEventType>
        {
            Enums.TreatmentEventType.TreatmentStart, 
            Enums.TreatmentEventType.DiagnosisMade
        };

        public bool IsEditable => TreatmentEventType != null 
                                  && EditableTreatmentEventTypes.Contains(TreatmentEventType.Value);

        public bool IsStartingEvent => TreatmentEventType != null
                                       && StartingTreatmentEventTypes.Contains(TreatmentEventType.Value);
    }
}
