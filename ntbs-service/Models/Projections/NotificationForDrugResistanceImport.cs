using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Projections
{
    public interface INotificationForDrugResistanceImport
    {
        int NotificationId { get; }
        DrugResistanceProfile DrugResistanceProfile { get; }
        MDRDetails MDRDetails { get; }
        bool IsMdr { get; }
        bool IsMBovis { get; }
        bool IsMBovisQuestionnaireComplete { get; }
    }

    public class NotificationForDrugResistanceImport : INotificationForDrugResistanceImport
    {
        public Notification Notification { get; set; }

        public DrugResistanceProfile DrugResistanceProfile { get; set; }
        public TreatmentRegimen? TreatmentRegimen { get; set; }
        public Status? ExposureToKnownMdrCaseStatus { get; set; }
        public MDRDetails MDRDetails { get; set; }
        public MBovisDetails MBovisDetails { get; set; }

        public int NotificationId => Notification.NotificationId;
        public bool IsMdr => DrugResistanceHelper.IsMdr(DrugResistanceProfile, TreatmentRegimen, ExposureToKnownMdrCaseStatus);
        public bool IsMBovis => DrugResistanceHelper.IsMbovis(DrugResistanceProfile);
        public bool IsMBovisQuestionnaireComplete =>
            DrugResistanceHelper.IsMBovisQuestionnaireComplete(MBovisDetails);
    }
}
