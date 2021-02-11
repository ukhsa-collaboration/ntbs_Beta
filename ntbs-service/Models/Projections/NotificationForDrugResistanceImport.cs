using System;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Projections
{
    public interface INotificationForDrugResistanceImport
    {
        int NotificationId { get; }
        DrugResistanceProfile DrugResistanceProfile { get; }
        bool IsMdr { get; }
        bool IsMBovis { get; }
    }

    public class NotificationForDrugResistanceImport : INotificationForDrugResistanceImport
    {
        public int NotificationId { get; set; }
        public DrugResistanceProfile DrugResistanceProfile { get; set; }
        public TreatmentRegimen? TreatmentRegimen { get; set; }
        public Status? ExposureToKnownCaseStatus { get; set; }

        // TODO:NTBS-2034 Remove duplication with NotificationDisplay
        public bool IsMdr =>
            // If user-set treatment ... 
            TreatmentRegimen == Enums.TreatmentRegimen.MdrTreatment
            // ... or lab results indicate MDR, ...
            || DrugResistanceProfile.DrugResistanceProfileString == "RR/MDR/XDR"
            // ... or if there is any data entered in the MDR pages - otherwise we could be hiding record data
            || ExposureToKnownCaseStatus != null;

        public bool IsMBovis =>
            // If the lab results point to M. bovis species ...
            string.Equals("M. bovis", DrugResistanceProfile.Species, StringComparison.InvariantCultureIgnoreCase);
    }
}
