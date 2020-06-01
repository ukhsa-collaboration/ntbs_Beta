using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public partial class ImmunosuppressionDetails : ModelBase, IOwnedEntityForAuditing
    {
        [Display(Name = "Is the patient immunosuppressed?")]
        [AssertThat(nameof(TestAtLeastOneSelectedWhenYes), 
            ErrorMessage = ValidationMessages.ImmunosuppressionTypeRequired)]
        public Status? Status { get; set; }

        public bool? HasBioTherapy { get; set; }
        public bool? HasTransplantation { get; set; }
        public bool? HasOther { get; set; }

        [RequiredIf("Status == Enums.Status.Yes && HasOther == true",
            ErrorMessage = ValidationMessages.ImmunosuppressionDetailRequired)]
        [MaxLength(100)]
        [RegularExpression(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, ErrorMessage = ValidationMessages.InvalidCharacter)]
        [Display(Name = "Immunosuppression type description")]
        public string OtherDescription { get; set; }
        
        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;

        public bool TestAtLeastOneSelectedWhenYes 
            => Status != Enums.Status.Yes
                || HasBioTherapy == true || HasTransplantation == true || HasOther == true;
    }
}
