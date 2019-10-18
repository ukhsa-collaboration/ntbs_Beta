using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace ntbs_service.Models
{
    public class LocalAuthorityToPHEC
    {
        [MaxLength(50)]
        public string PHECCode { get; set; }
        public virtual PHEC PHEC { get; set; }

        [MaxLength(50)]
        public string LocalAuthorityCode { get; set; }
        public virtual LocalAuthority LocalAuthority { get; set; }
    }
}