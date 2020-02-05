using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityDraftAlert : Alert
    {
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
