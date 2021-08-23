using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum TreatmentEventType
    {
        [Display(Name = "Treatment restart")]
        TreatmentRestart,
        [Display(Name = "Treatment outcome")]
        TreatmentOutcome,
        [Display(Name = "Transfer in")]
        TransferIn,
        [Display(Name = "Transfer out")]
        TransferOut,
        [Display(Name = "Treatment start")]
        TreatmentStart,
        [Display(Name = "Diagnosis made")]
        DiagnosisMade,
        [Display(Name = "Denotification")]
        Denotification
    }
}
