using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class NotificationSite
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }

        [RequiredIf("SiteId == SiteId.OTHER && Notification.ShouldValidateFull", ErrorMessage = ValidationMessages.DiseaseSiteOtherIsRequired)]
        [RegularExpression(ValidationRegexes.ValidCharactersForName, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string SiteDescription { get; set; }
    }
}