using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class TransferAlert : Alert
    {
        public TransferReason TransferReason { get; set; }
        public string OtherReasonDescription { get; set; }
        public string TransferRequestNote { get; set; }
        public override string Action => "Transfer request to your TB service";
        public override string ActionLink => "link to somewhere";

        public TransferAlert()
        {
            AlertType = AlertType.TransferRequest;
        }
    }
}