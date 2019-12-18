namespace ntbs_service.Models.Entities
{
    public class SocialContextAddress : SocialContextBase
    {
        public int SocialContextAddressId { get; set; }

        public override bool PostcodeIsRequired => true;
        public override bool DateToIsRequired => true;

        public override int Id
        {
            set {
                SocialContextAddressId = value;
            }
        }
    }
}
