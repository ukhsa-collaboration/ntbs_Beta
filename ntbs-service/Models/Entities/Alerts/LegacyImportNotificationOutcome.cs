namespace ntbs_service.Models.Entities.Alerts
{
    public class LegacyImportNotificationOutcome
    {
        public int LegacyImportNotificationOutcomeId { get; set; }
        public int LegacyImportMigrationRunId { get; set; }
        public string OldNotificationId { get; set; }
        public int? NtbsId { get; set; }
        public bool? SuccessfullyMigrated { get; set; }
        public string Notes { get; set; }
    }
}
