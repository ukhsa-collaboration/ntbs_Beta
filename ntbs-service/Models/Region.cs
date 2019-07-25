using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class Region
    {
        public Region()
        {
            Patient = new HashSet<Patient>();
        }

        public byte RegionId { get; set; }
        public string Label { get; set; }

        public virtual ICollection<Patient> Patient { get; set; }
    }
}
