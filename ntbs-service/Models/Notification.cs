using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ClinicalTimeline ClinicalTimeline { get; set; }
    }
}
