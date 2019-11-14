using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models
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
