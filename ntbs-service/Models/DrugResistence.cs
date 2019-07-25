using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class DrugResistence
    {
        public int DrugResistenceId { get; set; }
        public int NotificationId { get; set; }
        public byte ResistentDrugId { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual ResistentDrug ResistentDrug { get; set; }
    }
}
