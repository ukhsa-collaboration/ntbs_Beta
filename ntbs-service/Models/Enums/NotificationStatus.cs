using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum NotificationStatus
    {
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

    public static class NotificationStatusHelper
    {
        public static bool IsOpen(this NotificationStatus status)
        {
            switch (status)
            {
                case NotificationStatus.Draft:
                case NotificationStatus.Notified:
                    return true;
                case NotificationStatus.Deleted:
                case NotificationStatus.Denotified:
                case NotificationStatus.Legacy:
                case NotificationStatus.Closed:
                default:
                    return false;
            }
        }
    }
}
