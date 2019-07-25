using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Hospital
    {
        public Hospital()
        {
            Notification = new HashSet<Notification>();
        }

        public int HospitalId { get; set; }
        public string Label { get; set; }

        public virtual ICollection<Notification> Notification { get; set; }
    }
}
