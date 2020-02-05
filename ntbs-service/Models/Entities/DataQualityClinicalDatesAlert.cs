using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityClinicalDatesAlert : Alert
    {
        public override string Action =>
            "One or more of the clinical dates appears to be out of sequence, please review.";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditClinicalDetails);

        public DataQualityClinicalDatesAlert()
        {
            AlertType = AlertType.DataQualityClinicalDates;
        }
    }
}
