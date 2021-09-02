using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts

{
    public class DataQualityPotentialDuplicateAlert : Alert
    {
        public int DuplicateId { get; set; }

        public bool NhsNumberMismatch { get; set; }

        public const int MinNumberDaysNotifiedForAlert = 45;

        public override string Action => NhsNumberMismatch ? $"This record may be a duplicate of {DuplicateId}, however the NHS numbers do not match. Please contact the case manager to discuss."
            : $"This record may be a duplicate of {DuplicateId}. Please contact the case manager to discuss.";

        public override string ActionLink => RouteHelper.GetNotificationPath(
            NotificationId.Value,
            NotificationSubPaths.Overview);

        public DataQualityPotentialDuplicateAlert()
        {
            AlertType = AlertType.DataQualityPotientialDuplicate;
        }
    }
}
