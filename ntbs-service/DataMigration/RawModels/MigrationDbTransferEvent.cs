using System;

namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationTransferEventsView
    public class MigrationDbTransferEvent : MigrationDbRecord
    {
        public DateTime? EventDate { get; set; }
        public string TreatmentEventType { get; set; }
        public Guid? HospitalId { get; set; }
        public string CaseManager { get; set; }
        public string Notes { get; set; }
    }
}
