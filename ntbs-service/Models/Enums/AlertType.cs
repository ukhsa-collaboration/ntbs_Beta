using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum AlertType
    {
        [Display(Name = "Enhanced surveillance - MDR")]
        EnhancedSurveillanceMDR,
        EnhancedSurveillanceMBovis,
        MissingTreatmentOutcome,
        [Display(Name = "Unmatched lab result")]
        UnmatchedLabResult,
        [Display(Name = "Transfer request")]
        TransferRequest,
        [Display(Name = "Transfer rejected")]
        TransferRejected,
        DataQualityIssue,
        SocialContext,
        [Display(Name = "Test Alert")]
        Test
    }
}
