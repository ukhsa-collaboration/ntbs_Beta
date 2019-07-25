using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Episode
    {
        public int EpisodeId { get; set; }
        public int NotificationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Notification Notification { get; set; }
    }
}
