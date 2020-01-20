using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum DotStatus
    {
        [Display(Name = "Dot received")]
        DotReceived,
        [Display(Name = "Dot refused")]
        DotRefused,
        [Display(Name = "Unknown")]
        Unknown
    }
}
