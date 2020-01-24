using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum Status
    {
        [Display(Name = "Yes")]
        Yes,
        [Display(Name = "No")]
        No,
        [Display(Name = "Unknown")]
        Unknown
    }
}
