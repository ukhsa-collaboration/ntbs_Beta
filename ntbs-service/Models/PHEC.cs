using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;

namespace ntbs_service.Models
{
    public class PHEC
    {
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        
        public virtual LocalAuthorityToPHEC LocalAuthorityToPHEC { get; set; }
    }
}