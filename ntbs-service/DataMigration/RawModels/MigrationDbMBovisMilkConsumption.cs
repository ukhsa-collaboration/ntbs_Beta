namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationMBovisUnpasteurisedMilkConsumptionView
    public class MigrationDbMBovisMilkConsumption : MigrationDbRecord
    {
        public int? YearOfConsumption { get; set; }
        public string MilkProductType { get; set; }
        public string ConsumptionFrequency { get; set; }
        public int? CountryId { get; set; }
        public string OtherDetails { get; set; }
    }
}
