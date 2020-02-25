using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum AnimalTbStatus
    {
        [Display(Name = "Suspected TB")]
        SuspectedTb,
        [Display(Name = "Confirmed TB")]
        ConfirmedTb,
        [Display(Name = "Unknown")]
        Unknown
    }
}
