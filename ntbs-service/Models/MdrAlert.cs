using ntbs_service.Helpers;

namespace ntbs_service.Models
{
    public class MdrAlert : Alert
    {
        public override string Action => "RR/MDR/XDR-TB identified. Please complete enhanced surveillance questionnaire";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.EditMDRDetails);
    }
}
