namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationMBovisAnimalExposureView
    public class MigrationDbMBovisAnimal : MigrationDbRecord
    {
        public int? YearOfExposure { get; set; }
        public string AnimalType { get; set; }
        public string Animal { get; set; }
        public string AnimalTbStatus { get; set; }
        public int? ExposureDuration { get; set; }
        public int? CountryId { get; set; }
        public string OtherDetails { get; set; }
    }
}
