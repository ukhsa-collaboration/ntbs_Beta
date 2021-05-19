using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class LegacyImportNotificationLogMessage
    {
        public int LegacyImportNotificationLogMessageId { get; set; }
        public int LegacyImportMigrationRunId { get; set; }
        public string OldNotificationId { get; set; }
        public LogMessageLevel LogMessageLevel { get; set; }
        public string Message { get; set; }
    }
}
