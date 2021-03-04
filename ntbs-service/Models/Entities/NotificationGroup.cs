using System.Collections.Generic;

namespace ntbs_service.Models.Entities
{
    /*
        For linking of notifications belonging to the same patient.
        Nullable on Notification and only set when there is a link between the notification and another one.
     */
    public class NotificationGroup
    {
        public int NotificationGroupId { get; set; }

        public virtual List<Notification> Notifications { get; set; }
    }
}
