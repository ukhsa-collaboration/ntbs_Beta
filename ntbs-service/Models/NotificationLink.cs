namespace ntbs_service.Models
{
    public class NotificationLink
    {
        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; }

        public int LinkedNotificationId { get; set; }
        public virtual Notification LinkedNotification{ get; set; }
    }
}