using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum TreatmentEventType
    {
        [Display(Name = "Treatment restart")]
        TreatmentRestart,
        [Display(Name = "Treatment outcome")]
        TreatmentOutcome
    }
}
