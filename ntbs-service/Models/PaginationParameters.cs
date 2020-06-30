using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class PaginationParameters : PaginationParametersBase
    {
        public int? NtbsOffset;
        public int? LegacyOffset;
        public int? PreviousNtbsOffset;
        public int? PreviousLegacyOffset;
        public int NumberOfNtbsNotificationsToFetch => NtbsOffset != null ? PageSize : PageIndex * PageSize;
    }
}
