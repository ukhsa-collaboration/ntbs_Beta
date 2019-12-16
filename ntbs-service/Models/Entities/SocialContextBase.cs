using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public abstract class SocialContextBase : ModelBase
    {
        public int NotificationId { get; set; }
        // We are not including a navigation property to Notification, otherwise it gets validated
        // on every TryValidateModel

        [MaxLength(150)]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [DisplayName("Address")]
        public string Address { get; set; }

        [RequiredIf("PostcodeIsRequired", ErrorMessage =  ValidationMessages.RequiredEnter)]
        [RegularExpression(ValidationRegexes.PostcodeValidation, ErrorMessage = ValidationMessages.PostcodeIsNotValid)]
        [DisplayName("Postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [AssertThat(@"DateFromAfterDob", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [ValidDateRange(ValidDates.EarliestBirthDate)]
        [DisplayName("From")]
        public DateTime? DateFrom { get ; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [AssertThat(@"DateToAfterDob", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        [AssertThat(@"DateFrom == null || DateTo > DateFrom", ErrorMessage = ValidationMessages.VenueDateToShouldBeLaterThanDateFrom)]
        [ValidDateRange(ValidDates.EarliestBirthDate)]
        [DisplayName("To")]
        public DateTime? DateTo { get ; set; }


        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        public DateTime? Dob { get; set; } = null;

        public bool DateFromAfterDob => Dob == null || DateFrom >= Dob;
        public bool DateToAfterDob => Dob == null || DateTo >= Dob;

        public string FormattedDateFrom => DateFrom.ConvertToString();
        public string FormattedDateTo => DateTo.ConvertToString();

        public abstract bool PostcodeIsRequired { get; }

        public abstract int Id { set; }
    }
}