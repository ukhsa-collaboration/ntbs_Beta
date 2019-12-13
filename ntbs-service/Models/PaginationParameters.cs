using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class PaginationParameters
    {
        public int PageSize;
        public int PageIndex;
        public int? NtbsOffset;
        public int? LegacyOffset;
        public int? PreviousNtbsOffset;
        public int? PreviousLegacyOffset;
        public int NumberOfNtbsNotificationsToFetch => NtbsOffset != null ? PageSize : PageIndex * PageSize;
    }
}
