using System;

namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationSocialContextVenueView
    public class MigrationDbSocialContextVenue : MigrationDbRecord
    {
        public int? VenueTypeId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Postcode { get; set; }
        public string Frequency { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public string Details { get; set; }
    }
}
