using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class NotificationSite : ModelBase
    {
        public int NotificationId { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }
        
        [Display(Name = "Site description")]
        [RequiredIf("SiteId == SiteId.OTHER && ShouldValidateFull", ErrorMessage = ValidationMessages.DiseaseSiteOtherIsRequired)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string SiteDescription { get; set; }
    }
}