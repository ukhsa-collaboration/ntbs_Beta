using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities.Alerts

{
    public class TransferRejectedAlert : Alert, IHasRootEntityForAuditing
    {
        public TransferRejectedAlert()
        {
            AlertType = AlertType.TransferRejected;
        }

        [MaxLength(200)]
        [ContainsNoTabs]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasicWithNewline,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        [Display(Name = "Rejection reason")]
        public string RejectionReason { get; set; }
        [MaxLength(200)]
        public string DecliningUserAndTbServiceString { get; set; }
        public override string Action => "Transfer request rejected";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.Value, NotificationSubPaths.TransferDeclined);
        public override bool NotDismissable => true;

        public string RootEntityType => RootEntities.Notification;
        public string RootId => NotificationId.ToString();
    }
}
