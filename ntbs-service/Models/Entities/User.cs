using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace ntbs_service.Models.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string DisplayName { get; set; }
        public string AdGroups { get; set; }
        public bool IsActive { get; set; }
        public bool IsCaseManager { get; set; }

        public virtual ICollection<CaseManagerTbService> CaseManagerTbServices { get; set; }

        [NotMapped] public string FullName => GivenName + " " + FamilyName;
    }
}
