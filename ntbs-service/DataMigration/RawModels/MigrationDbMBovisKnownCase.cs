namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationMBovisExposureToKnownCaseView
    public class MigrationDbMBovisKnownCase : MigrationDbRecord
    {
        public int? YearOfExposure { get; set; }
        public string ExposureSetting { get; set; }
        public int? ExposureNotificationId { get; set; }
        public string NotifiedToPheStatus { get; set; }
        public string OtherDetails { get; set; }
    }
}
