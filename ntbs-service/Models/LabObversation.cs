using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class LabObversation
    {
        public int LabObversationId { get; set; }
        public int NotificationId { get; set; }
        public int? InitialSputumSmearStatus { get; set; }
        public int? CxrctatDiagnosis { get; set; }
        public int? CultureAtAnySite { get; set; }
        public int? SmearStatus { get; set; }

        public virtual Notification Notification { get; set; }
    }
}
