using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Enums;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class MDRDetails : ModelBase
    {
        public Status? ExposureToKnownCaseStatus { get; set; }

        [MaxLength(40)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [RequiredIf(@"ExposureToKnownCaseStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.RelationshipToCaseIsRequired)]
        public string RelationshipToCase { get; set; }

        [RequiredIf(@"ExposureToKnownCaseStatus == Enums.Status.Yes", ErrorMessage = ValidationMessages.CaseInUKStatusIsRequired)]
        public Status? CaseInUKStatus { get; set; }

        public int? RelatedNotificationId { get; set; }

        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }
    }
}