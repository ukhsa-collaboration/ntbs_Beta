using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityBirthCountryAlert : Alert
    {
        public override string Action => "Data quality issue - unknown country of birth";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.EditPatientDetails);

        public DataQualityBirthCountryAlert()
        {
            AlertType = AlertType.DataQualityBirthCountry;
        }
    }
}
