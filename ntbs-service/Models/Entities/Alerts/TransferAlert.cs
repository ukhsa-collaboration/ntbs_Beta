using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ntbs_service.Helpers;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Entities.Alerts

{
    public class TransferAlert : Alert, IHasRootEntityForAuditing
    {
        public TransferAlert()
        {
            AlertType = AlertType.TransferRequest;
        }
        
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
        public override string ActionLink => RouteHelper.GetNotificationPath(NotificationId.GetValueOrDefault(), NotificationSubPaths.ActionTransferRequest);
        public override bool NotDismissable => true;
        public string TransferReasonString => TransferReason.GetDisplayName() + 
            (TransferReason == TransferReason.Other ? $" - {OtherReasonDescription}" : "");

        public string RootEntityType => RootEntities.Notification;
        public string RootId => NotificationId.ToString();
        [Display(Name = "TB Service")]
        public string TbServiceCode { get; set; }
        public virtual TBService TbService { get; set; }
        [Display(Name = "Case Manager")]
        public string CaseManagerUsername { get; set; }
        public virtual User CaseManager { get; set; }
    }
}
