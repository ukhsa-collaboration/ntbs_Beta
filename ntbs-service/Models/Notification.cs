using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Notification
    {
        public Notification()
        {
            DrugResistence = new HashSet<DrugResistence>();
            Episode = new HashSet<Episode>();
        }

        public int NotificationId { get; set; }
        public int PatientId { get; set; }
        public int HospitalId { get; set; }

        public virtual Hospital Hospital { get; set; }
        public virtual Patient Patient { get; set; }
        public virtual ICollection<DrugResistence> DrugResistence { get; set; }
        public virtual ICollection<Episode> Episode { get; set; }
    }
}
