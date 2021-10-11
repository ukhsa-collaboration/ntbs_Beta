using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    [Display(Name = "MDR details")]
    public partial class MDRDetails : ModelBase, IOwnedEntityForAuditing
    {
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

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
