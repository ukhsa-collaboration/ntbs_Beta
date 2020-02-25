using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class MBovisAnimalExposure : ModelBase, IHasRootEntityForAuditing
    {
        public int MBovisAnimalExposureId { get; set; }
        public int NotificationId { get; set; }

        [Required]
        [AssertThat(nameof(YearOfExposureAfterBirth), ErrorMessage = ValidationMessages.DateShouldBeLaterThanDobYear)]
        [AssertThat(nameof(YearOfExposureNotInFuture), ErrorMessage = ValidationMessages.BeforeCurrentYear)]
        [Range(1900, 2100, ErrorMessage = ValidationMessages.InvalidYearForAttribute)]
        [Display(Name = "Year of exposure")]
        public int? YearOfExposure { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Animal type")]
        public AnimalType? AnimalType { get; set; }
        
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [MaxLength(35)]
        [Display(Name = "Animal")]
        public string Animal { get; set; }
        
        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Animal TB status")]
        public AnimalTbStatus? AnimalTbStatus { get; set; }

        [Required]
        [Range(1, 99)]
        [Display(Name = "Duration (years)")]
        public int? ExposureDuration { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [Display(Name = "Other details")]
        public string OtherDetails { get; set; }

        public bool YearOfExposureAfterBirth => !DobYear.HasValue || YearOfExposure >= DobYear;
        public bool YearOfExposureNotInFuture => YearOfExposure <= DateTime.Now.Year;

        // For validation purposes only
        [NotMapped]
        public int? DobYear { get; set; }

        string IHasRootEntityForAuditing.RootEntityType => RootEntities.Notification;
        string IHasRootEntityForAuditing.RootId => NotificationId.ToString();
    }
}
