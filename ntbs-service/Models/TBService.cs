using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models
{
    public class TBService
    {
        [MaxLength(200)]
        public string Name { get; set; }

        [Key]
        public string Code { get; set; }
    }
}
