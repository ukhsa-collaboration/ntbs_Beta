using System;
using System.Collections.Generic;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    public class Notification
    {
        public Notification() {
            NotificationStatus = Enums.NotificationStatus.Draft;
        }
        public int NotificationId { get; set; }
        public DateTime? SubmissionDate { get; set; }
        public NotificationStatus NotificationStatus { get; set; }

        public virtual PatientDetails PatientDetails { get; set; }
        public virtual ClinicalDetails ClinicalDetails { get; set; }
        public virtual List<NotificationSite> NotificationSites { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual PatientTBHistory PatientTBHistory { get; set; }
        public virtual ContactTracing ContactTracing { get; set; }
        public virtual SocialRiskFactors SocialRiskFactors { get; set; }
    }
}
