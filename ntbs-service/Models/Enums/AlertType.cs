using System.ComponentModel;

namespace ntbs_service.Models.Enums
{
    public enum AlertType
    {
        [DisplayName("Enhanced surveillance - MDR")]
        EnhancedSurveillanceMDR,
        EnhancedSurveillanceMBovis,
        MissingTreatmentOutcome,
        UnmatchedLabResult,
        [DisplayName("Transfer Request")]
        TransferRequest,
        TransferRejected,
        DataQualityIssue,
        SocialContext,
        [DisplayName("Test Alert")]
        Test
    }
}
