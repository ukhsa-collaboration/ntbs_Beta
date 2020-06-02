using System;
using System.Linq;
using System.Linq.Expressions;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts
{
    public class DataQualityClusterAlert : Alert
    {
        public static readonly Expression<Func<Notification, bool>> NotificationQualifiesExpression =
            n => n.ClusterId != null
                 && !n.SocialContextAddresses.Any()
                 && !n.SocialContextVenues.Any();

        public static readonly Func<Notification, bool> NotificationQualifies =
            NotificationQualifiesExpression.Compile();
        
        public override string Action => "Please review social context information.";

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
