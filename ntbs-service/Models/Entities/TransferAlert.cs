using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class TransferAlert : Alert
    {
        public TransferReason TransferReason { get; set; }
        public string OtherReasonDescription { get; set; }
        public string TransferRequestNote { get; set; }
        public override string Action => "I'm an action";
        public override string ActionLink => "link to somewhere";
    }
}