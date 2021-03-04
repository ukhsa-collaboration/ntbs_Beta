using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    [NotMapped]
    public class UserPermissionsFilter
    {
        public List<string> IncludedTBServiceCodes { get; set; } = new List<string>();
        public List<string> IncludedPHECCodes { get; set; } = new List<string>();
        public UserType Type { get; set; }

        public bool FilterByTBService => Type == UserType.NhsUser;
        public bool FilterByPHEC => Type == UserType.PheUser;
    }
}