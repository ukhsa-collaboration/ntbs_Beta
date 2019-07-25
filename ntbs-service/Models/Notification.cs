using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Notification
    {
        public Notification()
        {
            CohortReview = new HashSet<CohortReview>();
            DrugResistence = new HashSet<DrugResistence>();
            Episode = new HashSet<Episode>();
            LabObversation = new HashSet<LabObversation>();
            TreatmentOutcome = new HashSet<TreatmentOutcome>();
        }

        public int NotificationId { get; set; }
        public int PatientId { get; set; }
        public int HospitalId { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<CohortReview> CohortReview { get; set; }
        public virtual ICollection<DrugResistence> DrugResistence { get; set; }
        public virtual ICollection<Episode> Episode { get; set; }
        public virtual ICollection<LabObversation> LabObversation { get; set; }
        public virtual ICollection<TreatmentOutcome> TreatmentOutcome { get; set; }
    }
}
