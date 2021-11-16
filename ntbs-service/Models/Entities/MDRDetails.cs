using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    [Display(Name = "MDR details")]
    public partial class MDRDetails : ModelBase, IOwnedEntityForAuditing
    {
        [Display(Name = "MDR treatment start date")]
        // This use of the DatesHaveBeenSet property is a bit hacky - we want to make sure that the property is
        // only "required" after it has been set (from the formatted date). If we don't do this, then an error
        // will be produced when the form data is initially validated by the framework, whether or not a valid
        // date has actually been provided.
        [RequiredIf(@"DatesHaveBeenSet && IsLegacy != true", ErrorMessage = ValidationMessages.FieldRequired)]
        [AssertThat(@"AfterDob(MDRTreatmentStartDate)", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [ValidClinicalDate]
        public DateTime? MDRTreatmentStartDate { get; set; }

        [MaxLength(10)]
        [ValidDuration]
        [Display(Name = "Expected treatment duration")]
        public string ExpectedTreatmentDurationInMonths { get; set; }

        [Display(Name = "Has the patient been exposed to a known RR/MDR/XDR case?")]
        public Status? ExposureToKnownCaseStatus { get; set; }

        [MaxLength(90)]
        [RegularExpression(ValidationRegexes.CharacterValidationAsciiBasic, ErrorMessage = ValidationMessages.InvalidCharacter)]
        [RequiredIf(@"ExposureToKnownCaseStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.RelationshipToCaseIsRequired)]
        [Display(Name = "Relationship of the current case to the contact")]
        public string RelationshipToCase { get; set; }

        public bool IsCountryUK => Country?.IsoCode == Countries.UkCode;
        [RequiredIf(nameof(IsCountryUK), ErrorMessage = ValidationMessages.NotifiedToPheStatusIsRequired)]
        [Display(Name = "Was the case notified to PHE?")]
        public Status? NotifiedToPheStatus { get; set; }

        // This should be a FK to Notification, but adding this results in EF deleting the FK to the owner, which is also Notification.
        // Probably some weirdness with owned relationships... not quite sure how to fix, so leaving as normal property for now.
        [Display(Name = "Contact's Notification ID")]
        [RequiredIf(@"NotifiedToPheStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.FieldRequired)]
        public int? RelatedNotificationId { get; set; }

        [RequiredIf(@"ExposureToKnownCaseStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Country of exposure")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        [Display(Name = "Discussed at the British Thoracic Society MDR forum?")]
        public Status? DiscussedAtMDRForum { get; set; }

        [NotMapped]
        public DateTime? Dob { get; set; }
        public bool AfterDob(DateTime date) => Dob == null || date >= Dob;

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
