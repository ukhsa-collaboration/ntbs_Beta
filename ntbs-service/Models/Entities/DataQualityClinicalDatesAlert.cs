using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityClinicalDatesAlert : Alert
    {
        public override string Action => "Data quality issue - clinical dates";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.EditPatientDetails);

        public DataQualityClinicalDatesAlert()
        {
            AlertType = AlertType.DataQualityClinicalDates;
        }
    }
}
