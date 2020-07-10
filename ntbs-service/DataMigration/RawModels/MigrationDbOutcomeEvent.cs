using System;

namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // MigrationTreatmentOutcomeEventsView
    public class MigrationDbOutcomeEvent : MigrationDbRecord
    {
        public DateTime? EventDate { get; set; }
        public string TreatmentEventType { get; set; }
        public int? TreatmentOutcomeId { get; set; }
        public string Note { get; set; }
        public string CaseManager { get; set; }
        public Guid? NtbsHospitalId { get; set; }
    }
}
