using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.ReferenceEntities
{
    public class PHEC
    {
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(64)]
        public string AdGroup { get; set; }
    }
}
