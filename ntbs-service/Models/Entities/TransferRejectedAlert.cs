using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models.Entities
{
    public class TransferRejectedAlert : Alert
    {
        public TransferRejectedAlert()
        {
            AlertType = AlertType.TransferRejected;
        }

        [MaxLength(200)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtendedWithNewLine,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        [Display(Name = "Rejection reason")]
        public string RejectionReason { get; set; }
        [MaxLength(200)]
        public string DecliningUserAndTbServiceString { get; set; }
        public override string Action => "Transfer request rejected";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.Value, NotificationSubPaths.TransferDeclined);
        public override bool NotDismissable => true;
    }
}
