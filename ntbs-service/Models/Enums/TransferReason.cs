using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum TransferReason
    {
        [Display(Name = "Patient relocated to a new area")]
        Relocation,
        [Display(Name = "Requested by other clinic")]
        Requested,
        [Display(Name = "Other")]
        Other
    }
}
