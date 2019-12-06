using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models.ReferenceEntities
{
    public class TBService
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ServiceAdGroup { get; set; }

        [MaxLength(50)]
        public string PHECCode { get; set; }
        public virtual PHEC PHEC { get; set; }

        public virtual ICollection<CaseManagerTbService> CaseManagerTbServices { get; set; }
    }
}
