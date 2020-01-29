using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityClusterAlert : Alert
    {
        public override string Action => "Data quality issue - cluster";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.EditPatientDetails);

        public DataQualityClusterAlert()
        {
            AlertType = AlertType.DataQualityCluster;
        }
    }
}
