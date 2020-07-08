using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Display(Name = "Social Context Address")]
    public class SocialContextAddress : SocialContextBase
    {
        public int SocialContextAddressId { get; set; }

        public override int Id
        {
            set
            {
                SocialContextAddressId = value;
            }
        }
    }
}
