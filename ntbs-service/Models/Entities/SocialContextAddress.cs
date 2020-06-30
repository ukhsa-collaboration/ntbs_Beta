using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Entities
{
    [Display(Name = "Social Context Address")]
    public class SocialContextAddress : SocialContextBase
    {
        public int SocialContextAddressId { get; set; }

        public override bool PostcodeIsRequired => true;
        public override bool DateToIsRequired => true;

        public override int Id
        {
            set
            {
                SocialContextAddressId = value;
            }
        }
    }
}
