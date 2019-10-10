using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models
{
    public class TBService
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ServiceAdGroup { get; internal set; }
        public string PHECAdGroup { get; internal set; }
    }
}
