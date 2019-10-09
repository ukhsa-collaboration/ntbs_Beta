using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class PaginationParameters
    {
        public int PageSize;
        public int PageIndex;
    }
}