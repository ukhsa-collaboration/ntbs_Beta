using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class MDRDetails : ModelBase, IIsOwnedEntity
    {
        [Display(Name = "Has the patient been exposed to a known RR/MDR/XDR-TB Case?")]
        public Status? ExposureToKnownCaseStatus { get; set; }

        [MaxLength(40)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [RequiredIf(@"ExposureToKnownCaseStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.RelationshipToCaseIsRequired)]
        [Display(Name = "Relationship of the current case to the contact")]
        public string RelationshipToCase { get; set; }

        [RequiredIf(@"ExposureToKnownCaseStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.CaseInUKStatusIsRequired)]
        [Display(Name = "Was the contact a case in the UK?")]
        public Status? CaseInUKStatus { get; set; }

        // This should be a FK to Notification, but adding this results in EF deleting the FK to the owner, which is also Notification.
        // Probably some weirdness with owned relationships... not quite sure how to fix, so leaving as normal property for now.
        [Display(Name = "Contact's notification id")]
        public int? RelatedNotificationId { get; set; }

        [Display(Name = "Country in which contact occurred")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
        public bool MDRDetailsEntered =>
            ExposureToKnownCaseStatus != null ||
            RelationshipToCase != null ||
            CaseInUKStatus != null ||
            RelatedNotificationId != null ||
            CountryId != null;

        string IIsOwnedEntity.RootEntityType => RootEntities.Notification;
    }
}
