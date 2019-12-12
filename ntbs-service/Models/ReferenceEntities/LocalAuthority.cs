using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.ReferenceEntities
{
    public class LocalAuthority
    {
        [MaxLength(50)]
        public string Code { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public virtual List<PostcodeLookup> PostcodeLookups { get; set; }

        public virtual LocalAuthorityToPHEC LocalAuthorityToPHEC { get; set; }
    }
}
