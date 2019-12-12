using System.ComponentModel;

namespace ntbs_service.Models.Enums
{
    public enum HIVTestStatus
    {
        [DisplayName("HIV status already known")]
        HIVStatusKnown,
        [DisplayName("Not offered")]
        NotOffered,
        [DisplayName("Offered and done")]
        Offered,
        [DisplayName("Offered but not done")]
        OfferedButNotDone,
        [DisplayName("Offered but refused")]
        OfferedButRefused
    }
}
