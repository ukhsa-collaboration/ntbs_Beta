using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityBirthCountryAlert : Alert
    {
        public override string Action => "Please review to see if more accurate information available.";
        public override string ActionLink =>
            RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.Overview);

        public DataQualityBirthCountryAlert()
        {
            AlertType = AlertType.DataQualityBirthCountry;
        }
    }
}
