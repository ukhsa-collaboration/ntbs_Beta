using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class TreatmentOutcome
    {
        public int TreatmentOutcomeId { get; set; }
        public int NotificationId { get; set; }
        public DateTime? EndDate { get; set; }

        public virtual Notification Notification { get; set; }
    }
}
