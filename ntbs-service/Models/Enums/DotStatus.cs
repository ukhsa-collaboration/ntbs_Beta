using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum DotStatus
    {
        [Display(Name = "DOT received")]
        DotReceived,
        [Display(Name = "DOT refused")]
        DotRefused,
        [Display(Name = "Unknown")]
        Unknown
    }
}
