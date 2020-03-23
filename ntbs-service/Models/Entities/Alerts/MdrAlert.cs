using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts

{
    public class MdrAlert : Alert
    {
        public override string Action => "Please complete enhanced surveillance questionnaire";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.EditMDRDetails);

        public MdrAlert()
        {
            AlertType = AlertType.EnhancedSurveillanceMDR;
        }
    }
}
