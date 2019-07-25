using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class ResistentDrug
    {
        public ResistentDrug()
        {
            DrugResistence = new HashSet<DrugResistence>();
        }

        public byte ResistentDrugId { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }

        public virtual ICollection<DrugResistence> DrugResistence { get; set; }
    }
}
