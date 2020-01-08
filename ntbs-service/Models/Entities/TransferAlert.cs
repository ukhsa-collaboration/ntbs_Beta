using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public class TransferAlert : Alert
    {
        public TransferReason TransferReason { get; set; }

        [MaxLength(200)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [Display(Name = "Other description")]
        public string OtherReasonDescription { get; set; }

        [MaxLength(200)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [Display(Name = "Optional note")]
        public string TransferRequestNote { get; set; }
        public override string Action => "Transfer request to your TB service";
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.TransferRequest);

        public TransferAlert()
        {
            AlertType = AlertType.TransferRequest;
        }
    }
}