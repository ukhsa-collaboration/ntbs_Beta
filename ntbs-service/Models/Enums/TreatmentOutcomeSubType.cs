using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum TreatmentOutcomeSubType
    {
        [Display(Name = "TB caused death")]
        TbCausedDeath,
        [Display(Name = "TB contributed to death")]
        TbContributedToDeath,
        [Display(Name = "TB was incidental to death")]
        TbIncidentalToDeath,
        [Display(Name = "Unknown")]
        Unknown,
        [Display(Name = "Standard therapy")]
        StandardTherapy,
        [Display(Name = "Multi-drug resistant regimen")]
        MdrRegimen,
        [Display(Name = "Other")]
        Other,
        [Display(Name = "Patient left UK")]
        PatientLeftUk,
        [Display(Name = "Patient has not left UK")]
        PatientNotLeftUk,
        [Display(Name = "Transferred abroad")]
        TransferredAbroad,
        [Display(Name = "Still on treatment")]
        StillOnTreatment,
        [Display(Name = "Culture positive")]
        CulturePositive,
        [Display(Name = "Additional resistance")]
        AdditionalResistance,
        [Display(Name = "Adverse reaction")]
        AdverseReaction
    }
}
