using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    [NotMapped]
    public class UserPermissionsFilter
    {
        public UserPermissionsFilter()
        {
            IncludedTBServiceCodes = new List<string>();
            IncludedPHECCodes = new List<string>();
        }
        public List<string> IncludedTBServiceCodes { get; set; }
        public List<string> IncludedPHECCodes { get; set; }
        public UserType Type { get; set; }

        public bool FilterByTBService => Type == UserType.NhsUser;
        public bool FilterByPHEC => Type == UserType.PheUser;
    }
}