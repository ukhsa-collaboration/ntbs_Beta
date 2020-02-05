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
        [Display(Name = "Data quality issue - draft record")]
        DataQualityDraft,
        [Display(Name = "Data quality issue - unknown country of birth")]
        DataQualityBirthCountry,
        [Display(Name = "Data quality issue - clinical dates")]
        DataQualityClinicalDates,
        [Display(Name = "Data quality issue - cluster")]
        DataQualityCluster,
        SocialContext,
        [Display(Name = "Test Alert")]
        Test
    }
}
