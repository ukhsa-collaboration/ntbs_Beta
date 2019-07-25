using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Sex
    {
        public Sex()
        {
            Patient = new HashSet<Patient>();
        }

        public byte SexId { get; set; }
        public string Label { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }
    }
}
