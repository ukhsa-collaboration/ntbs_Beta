using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class TreatmentEvent : ModelBase, IHasRootEntity
    {
        public int TreatmentEventId { get; set; }

        [Display(Name = "Event Date")]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [ValidClinicalDate]
        [AssertThat(@"EventDateAfterDob", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [AssertThat(@"EventDateAfterNotificationDate", ErrorMessage = ValidationMessages.DateShouldBeLaterThanNotification)]
        public DateTime? EventDate { get; set; }

        [Display(Name = "Event")]
        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        public TreatmentEventType? TreatmentEventType { get; set; }

        [RequiredIf("TreatmentEventTypeIsOutcome", ErrorMessage = ValidationMessages.TreatmentOutcomeRequiredForOutcome)]
        [AssertThat("TreatmentEventTypeIsNotRestart", ErrorMessage = ValidationMessages.TreatmentOutcomeInvalidForRestart)]
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
        
        public bool TreatmentEventTypeIsOutcome => TreatmentEventType == Enums.TreatmentEventType.TreatmentOutcome;
        public bool TreatmentEventTypeIsNotRestart => TreatmentEventType != Enums.TreatmentEventType.TreatmentRestart;

        public bool EventDateAfterDob => Dob == null || EventDate >= Dob;
        public bool EventDateAfterNotificationDate => DateOfNotification == null || EventDate >= DateOfNotification;

        string IHasRootEntity.RootEntityType => RootEntities.Notification;
        string IHasRootEntity.RootId => NotificationId.ToString();

        public static IList<TreatmentEventType> EditableTreatmentEventTypes => new List<TreatmentEventType>
        {
            Enums.TreatmentEventType.TreatmentOutcome,
            Enums.TreatmentEventType.TreatmentRestart
        };
        public bool IsEditable => TreatmentEventType != null && EditableTreatmentEventTypes.Contains(TreatmentEventType.Value);
    }
}
