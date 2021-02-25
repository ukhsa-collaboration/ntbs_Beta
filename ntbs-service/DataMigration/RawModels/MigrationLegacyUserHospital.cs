using System;

namespace ntbs_service.DataMigration.RawModels
{
    public class MigrationLegacyUserHospital
    {
        public string Username { get; set; }
        
        public Guid? HospitalId { get; set; }
    }
}
