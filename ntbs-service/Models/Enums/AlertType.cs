using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum AlertType
    {
        [Display(Name = "Enhanced surveillance - RR/MDR/XDR-TB")]
        EnhancedSurveillanceMDR,
        [Display(Name = "Enhanced surveillance - M. bovis")]
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
        [Display(Name = "Data quality issue - missing 12 month treatment outcome")]
        DataQualityTreatmentOutcome12,
        [Display(Name = "Data quality issue - missing 24 month treatment outcome")]
        DataQualityTreatmentOutcome24,
        [Display(Name = "Data quality issue - missing 36 month treatment outcome")]
        DataQualityTreatmentOutcome36,
        [Display(Name = "Data quality issue - inconsistent values for social risk factors and DOT")]
        DataQualityDotVotAlert,
        SocialContext,
        [Display(Name = "Test Alert")]
        Test
    }
}
