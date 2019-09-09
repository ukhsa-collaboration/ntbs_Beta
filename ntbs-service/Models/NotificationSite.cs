using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class NotificationSite
    {
        public int NotificationId { get; set; }
        public Notification Notification { get; set; }

        public int SiteId { get; set; }
        public Site Site { get; set; }

        [RegularExpression(ValidationRegex.NameFormat, ErrorMessage = ValidationMessages.NameFormat)]
        public string SiteDescription { get; set; }
    }
}