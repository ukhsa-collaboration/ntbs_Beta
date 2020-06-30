using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class PaginationParametersBase
    {
        public int PageSize;
        public int PageIndex;
        public int? Offset;
    }
}
