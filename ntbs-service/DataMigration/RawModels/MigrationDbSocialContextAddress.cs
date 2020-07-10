using System;

namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationSocialContextAddressView
    public class MigrationDbSocialContextAddress : MigrationDbRecord
    {
        public string Address { get; set; }
        public string Postcode { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Details { get; set; }
    }
}
