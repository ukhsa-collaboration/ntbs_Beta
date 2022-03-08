using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.ReferenceEntities
{
    public class Continent
    {
        [Key]
        public int ContinentId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}
