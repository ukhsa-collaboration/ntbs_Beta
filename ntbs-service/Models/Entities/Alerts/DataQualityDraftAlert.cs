using System;
using System.Linq.Expressions;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts

{
    public class DataQualityDraftAlert : Alert
    {
        private const int MinNumberDaysDraftForAlert = 90;

        public static readonly Expression<Func<Notification, bool>> NotificationQualifiesExpression = 
            n => n.NotificationStatus == NotificationStatus.Draft && 
                 n.CreationDate < DateTime.Now.AddDays(-MinNumberDaysDraftForAlert);

        public static readonly Func<Notification, bool> NotificationQualifies =
            NotificationQualifiesExpression.Compile();
        
        public override string Action => "Draft record has been open for more than 90 days, please review and action.";

        public override string ActionLink => RouteHelper.GetNotificationPath(
            NotificationId.GetValueOrDefault(),
            NotificationSubPaths.EditPatientDetails);

        public DataQualityDraftAlert()
        {
            AlertType = AlertType.DataQualityDraft;
        }
    }
}
