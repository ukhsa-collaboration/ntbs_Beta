using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public virtual PatientDetails PatientDetails { get; set; }
        public virtual ClinicalDetails ClinicalDetails { get; set; }

        public virtual List<NotificationSite> NotificationSites { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual PatientTBHistory PatientTBHistory { get; set; }
        public virtual ContactTracing ContactTracing { get; set; }
        public virtual SocialRiskFactors SocialRiskFactors { get; set; }
    }
}
