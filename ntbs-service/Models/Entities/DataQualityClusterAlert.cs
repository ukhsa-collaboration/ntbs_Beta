using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityClusterAlert : Alert
    {
        public override string Action => "Notification is in a Cluster, please review social context information.";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditSocialContextAddresses);

        public DataQualityClusterAlert()
        {
            AlertType = AlertType.DataQualityCluster;
        }
    }
}
