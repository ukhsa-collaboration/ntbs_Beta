using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class MBovisExposureToKnownCase : ModelBase, IHasRootEntityForAuditing
    {
        public int MBovisExposureToKnownCaseId { get; set; }
        public int NotificationId { get; set; }
        
        [Required]
        [AssertThat(nameof(YearOfExposureAfterBirth), ErrorMessage = ValidationMessages.DateShouldBeLaterThanDobYear)]
        [AssertThat(nameof(YearOfExposureNotInFuture), ErrorMessage = ValidationMessages.BeforeCurrentYear)]
        [Range(1900, 2100, ErrorMessage = ValidationMessages.InvalidYearForAttribute)]
        [Display(Name = "Year of exposure")]
        public int? YearOfExposure { get; set; }
        
        public bool YearOfExposureAfterBirth => !DobYear.HasValue || YearOfExposure >= DobYear;
        public bool YearOfExposureNotInFuture => YearOfExposure <= DateTime.Now.Year;
        
        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Exposure setting")]
        public ExposureSetting? ExposureSetting { get; set; }
        
        [Required]
        [AssertThat(nameof(ExposureNotificationIdIsDifferentToNotificationId), ErrorMessage = ValidationMessages.RelatedNotificationIdCannotBeSameAsNotificationId)]
        [Display(Name = "NTBS ID")]
        public int? ExposureNotificationId { get; set; }

        public bool ExposureNotificationIdIsDifferentToNotificationId => 
            !ExposureNotificationId.HasValue 
            || NotificationId == default 
            || ExposureNotificationId.Value != NotificationId;
        
        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [Display(Name = "Other details")]
        public string OtherDetails { get; set; }
        
        // For validation purposes only
        [NotMapped]
        public int? DobYear { get; set; }
        
        
        string IHasRootEntityForAuditing.RootEntityType => RootEntities.Notification;
        string IHasRootEntityForAuditing.RootId => NotificationId.ToString();
    }
}
