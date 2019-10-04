using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class NotificationGroup
    {
        public int NotificationGroupId { get; set; }

        public virtual List<Notification> Notifications { get; set; }
    }
}