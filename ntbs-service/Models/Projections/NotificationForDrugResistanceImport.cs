using ntbs_service.Helpers;
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
        public Notification Notification { get; set; }

        public DrugResistanceProfile DrugResistanceProfile { get; set; }
        public TreatmentRegimen? TreatmentRegimen { get; set; }
        public Status? ExposureToKnownCaseStatus { get; set; }

        public int NotificationId => Notification.NotificationId;
        public bool IsMdr => DrugResistanceHelper.IsMdr(DrugResistanceProfile, TreatmentRegimen, ExposureToKnownCaseStatus);

        public bool IsMBovis => DrugResistanceHelper.IsMbovis(DrugResistanceProfile);
    }
}
