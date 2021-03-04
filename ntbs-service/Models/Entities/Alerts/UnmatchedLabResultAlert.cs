using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts
{
    public class UnmatchedLabResultAlert : Alert
    {
        public UnmatchedLabResultAlert()
        {
            AlertType = AlertType.UnmatchedLabResult;
        }

        public string SpecimenId { get; set; }

        public override string Action => "Please review lab specimens which potentially match to this notification";
        public override string ActionLink => RouteHelper.GetUnmatchedSpecimenPath(SpecimenId);
        public override bool NotDismissable => true;
    }
}
