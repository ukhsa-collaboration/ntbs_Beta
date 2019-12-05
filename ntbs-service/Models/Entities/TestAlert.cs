namespace ntbs_service.Models.Entities
{
    public class TestAlert : Alert
    {
        public override string Action => "I'm an action";
        public override string ActionLink => "link to somewhere";
    }
}
