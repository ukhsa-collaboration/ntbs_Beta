using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityClinicalDatesAlert : Alert
    {
        public override string Action => "One or more of the clinical dates appears to be out of sequence, please review.";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(),
            NotificationSubPaths.Overview) + "#clinical-details-overview-details";

        public DataQualityClinicalDatesAlert()
        {
            AlertType = AlertType.DataQualityClinicalDates;
        }
    }
}
