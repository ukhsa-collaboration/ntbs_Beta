using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums 
{
    public enum NotificationStatus {
        [Display(Name = "Draft")]
        Draft,
        [Display(Name = "Notification")]
        Notified,
        [Display(Name = "Denotified")]
        Denotified,
        [Display(Name = "Deleted")]
        Deleted,
        [Display(Name = "Legacy")]
        Legacy,
        [Display(Name = "Notification")]
        Closed
    }
}
