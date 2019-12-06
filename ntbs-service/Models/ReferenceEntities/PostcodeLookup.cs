using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.ReferenceEntities
{
    public class PostcodeLookup
    {
        [MaxLength(10)]
        public string Postcode { get; set; } 
        
        public string LocalAuthorityCode { get; set; }
        public virtual LocalAuthority LocalAuthority { get; set; }

        public string CountryCode { get; set; }
    }
}
