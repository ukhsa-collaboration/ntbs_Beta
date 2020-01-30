using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityClusterAlert : Alert
    {
        public override string Action => "Notification is in a Cluster, please review social context information.";
        public override string ActionLink =>
            RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(),
                NotificationSubPaths.Overview) + "#social-context-addresses-overview-details";

        public DataQualityClusterAlert()
        {
            AlertType = AlertType.DataQualityCluster;
        }
    }
}
