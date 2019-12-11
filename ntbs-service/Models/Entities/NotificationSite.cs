using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class NotificationSite : ModelBase
    {
        public int NotificationId { get; set; }

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        [DisplayName("Site description")]
        [RequiredIf("SiteId == SiteId.OTHER && ShouldValidateFull", ErrorMessage = ValidationMessages.FieldRequired)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string SiteDescription { get; set; }
    }
}
