using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum TreatmentOutcomeType
    {
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Cured")]
        Cured,
        [Display(Name = "Died")]
        Died,
        [Display(Name = "Lost to follow-up")]
        Lost,
        [Display(Name = "Not evaluated")]
        NotEvaluated,
        [Display(Name = "Treatment stopped")]
        TreatmentStopped
    }
}
