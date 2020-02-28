using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum TreatmentRegimen
    {
        [Display(Name = "Standard therapy")]
        StandardTherapy,
        [Display(Name = "RR/MDR/XDR treatment")]
        MdrTreatment,
        [Display(Name = "Other")]
        Other
    }
}
