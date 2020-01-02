using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models.Entities
{
    public class CaseManager
    {
        public string Email { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }

        public virtual ICollection<CaseManagerTbService> CaseManagerTbServices { get; set; }

        [NotMapped] 
        public string FullName => GivenName + " " + FamilyName;
    }
}
