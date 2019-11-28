namespace ntbs_service.Models
{
    public class TestAlert : Alert
    {
        public override string Action => "I'm an action";
        public override string ActionLink => "link to somewhere";
    }
}
