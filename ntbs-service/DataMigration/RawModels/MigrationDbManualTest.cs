using System;

namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // ManualTestResults
    public class MigrationDbManualTest : MigrationDbRecord
    {
        public string Source { get; set; }
        public int? ManualTestTypeId { get; set; }
        public int? SampleTypeId { get; set; }
        public string Result { get; set; }
        public DateTime? TestDate { get; set; }
    }
}
