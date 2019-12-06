using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.ReferenceEntities
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
