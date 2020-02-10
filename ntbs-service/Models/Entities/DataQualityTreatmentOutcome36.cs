using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityTreatmentOutcome36 : Alert
    {
        public override string Action => 
            "No treatment outcome at 36 months can be found, please provide treatment outcome with appropriate date";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditPatientDetails);

        public DataQualityTreatmentOutcome36()
        {
            AlertType = AlertType.DataQualityTreatmentOutcome36;
        }
    }
}
