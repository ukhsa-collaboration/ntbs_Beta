using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums 
{
    public enum AlertType 
    {
        EnhancedSurveillanceMDR,
        EnhancedSurveillanceMBovis,
        MissingTreatmentOutcome,
        UnmatchedLabResult,
        [Display(Name = "Transfer Request")]
        TransferRequest,
        TransferRejected,
        DataQualityIssue,
        SocialContext,
        [Display(Name = "Test Alert")]
        Test
    }
}