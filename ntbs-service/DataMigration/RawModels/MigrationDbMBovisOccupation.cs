namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationMBovisOccupationExposuresView
    public class MigrationDbMBovisOccupation : MigrationDbRecord
    {
        public int? YearOfExposure { get; set; }
        public string OccupationSetting { get; set; }
        public int? OccupationDuration { get; set; }
        public int? CountryId { get; set; }
        public string OtherDetails { get; set; }
    }
}
