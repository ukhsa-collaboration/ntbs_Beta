using System;
using System.Collections.Generic;
using ntbs_service.Models.Entities.Alerts;

namespace ntbs_service.Models.Entities
{
    public class LegacyImportMigrationRun
    {
        public int LegacyImportMigrationRunId { get; set; }
        public DateTime StartTime { get; set; }
        public string AppRelease { get; set; }
        public string FileName { get; set; }
        public string Description { get; set; }
        public string LegacyIdList { get; set; }
        public DateTime? RangeStartDate { get; set; }
        public DateTime? RangeEndDate { get; set; }

        public virtual ICollection<LegacyImportNotificationOutcome> LegacyImportNotificationOutcomes { get; set; }
        public virtual ICollection<LegacyImportNotificationLogMessage> LegacyImportNotificationLogMessages { get; set; }
    }
}
