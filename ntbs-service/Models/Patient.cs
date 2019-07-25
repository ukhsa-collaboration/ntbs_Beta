using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Patient
    {
        public Patient()
        {
            Notification = new HashSet<Notification>();
        }

        public int PatientId { get; set; }
        public string Surname { get; set; }
        public string Forename { get; set; }
        public string NhsNumber { get; set; }
        public byte SexId { get; set; }
        public DateTime Dob { get; set; }
        public byte? UkBorn { get; set; }
        public byte RegionId { get; set; }
        public Guid? EtsId { get; set; }
        public int? LtbrId { get; set; }

        public virtual Region Region { get; set; }
        public virtual Sex Sex { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
    }
}
