
using System.ComponentModel.DataAnnotations;
using System.Text;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class ImmunosuppressionDetails : ModelBase, IOwnedEntityForAuditing
    {
        [AssertThat(@"TestAtLeastOneSelectedWhenYes", 
            ErrorMessage = ValidationMessages.ImmunosuppressionTypeRequired)]
        public Status? Status { get; set; }

        public bool? HasBioTherapy { get; set; }
        public bool? HasTransplantation { get; set; }
        public bool? HasOther { get; set; }

        [RequiredIf("Status == Enums.Status.Yes && HasOther == true",
            ErrorMessage = ValidationMessages.ImmunosuppressionDetailRequired)]
        [MaxLength(100)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Immunosuppression type description")]
        public string OtherDescription { get; set; }

        public string CreateTypesOfImmunosuppressionString()
        {
            var sb = new StringBuilder();
            sb.Append(HasBioTherapy == true ? "Biological Therapy" : string.Empty);

            if (HasTransplantation == true)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.Append("Transplantation");
            }

            if (HasOther == true)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.Append("Other");
            }

            return sb.ToString();
        }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;

        public bool TestAtLeastOneSelectedWhenYes 
            => Status != Enums.Status.Yes
                || HasBioTherapy == true || HasTransplantation == true || HasOther == true;
    }
}
