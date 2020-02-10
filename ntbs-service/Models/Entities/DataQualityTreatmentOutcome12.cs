using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityTreatmentOutcome12 : Alert
    {
        public override string Action => 
            "No treatment outcome at 12 months can be found, please provide treatment outcome with appropriate date.";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditPatientDetails);

        public DataQualityTreatmentOutcome12()
        {
            AlertType = AlertType.DataQualityTreatmentOutcome12;
        }
    }
}
