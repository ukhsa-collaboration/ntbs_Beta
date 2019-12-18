using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum HIVTestStatus
    {
        [Display(Name = "HIV status already known")]
        HIVStatusKnown,
        [Display(Name = "Not offered")]
        NotOffered,
        [Display(Name = "Offered and done")]
        Offered,
        [Display(Name = "Offered but not done")]
        OfferedButNotDone,
        [Display(Name = "Offered but refused")]
        OfferedButRefused
    }
}
