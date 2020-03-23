using System;
using System.Linq.Expressions;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts
{
    public class DataQualityBirthCountryAlert : Alert
    {
        public static readonly Expression<Func<Notification, bool>> NotificationQualifiesExpression = 
            n => n.PatientDetails.CountryId == Countries.UnknownId;

        public static readonly Func<Notification, bool> NotificationQualifies =
            NotificationQualifiesExpression.Compile();
        
        public override string Action => "Please review to see if more accurate information available.";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditPatientDetails);

        public DataQualityBirthCountryAlert()
        {
            AlertType = AlertType.DataQualityBirthCountry;
        }
    }
}
