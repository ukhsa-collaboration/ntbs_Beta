using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums 
{
    public enum AlertType 
    {
        EnhancedSurveillanceMDR,
        EnhancedSurveillanceMBovis,
        MissingTreatmentOutcome,
        UnmatchedLabResult,
        TransferRequest,
        TransferRejected,
        DataQualityIssue,
        SocialContext
    }
}