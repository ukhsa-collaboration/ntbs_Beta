using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Interfaces;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class SocialContextVenue : ModelBase
    {
        public int SocialContextVenueId { get; set; }

        public int NotificationId { get; set; }
        // We are not including a navigation property to Notification, otherwise it gets validated
        // on every TryValidateModel(venue)

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [DisplayName("Venue type")]
        public int? VenueTypeId { get; set; }
        public virtual VenueType VenueType { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [DisplayName("Venue name")]
        public string Name { get; set; }

        [MaxLength(150)]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
            ErrorMessage = ValidationMessages.StringWithNumbersAndForwardSlashFormat)]
        [DisplayName("Address")]
        public string Address { get; set; }

        [RegularExpression(ValidationRegexes.PostcodeValidation, ErrorMessage = ValidationMessages.PostcodeIsNotValid)]
        [DisplayName("Postcode")]
        public string Postcode { get; set; }

        [DisplayName("Frequency")]
        public Frequency? Frequency { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [AssertThat(@"DateFromAfterDob", ErrorMessage = ValidationMessages.VenueDateShouldBeLaterThanDob)]
        [ValidDateRange(ValidDates.EarliestBirthDate)]
        [DisplayName("From")]
        public DateTime? DateFrom { get ; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [AssertThat(@"DateToAfterDob", ErrorMessage = ValidationMessages.VenueDateShouldBeLaterThanDob)]
        [AssertThat(@"DateFrom == null || DateTo > DateFrom", ErrorMessage = ValidationMessages.VenueDateToShouldBeLaterThanDateFrom)]
        [ValidDateRange(ValidDates.EarliestBirthDate)]
        [DisplayName("To")]
        public DateTime? DateTo { get ; set; }

        [MaxLength(100)]
        [RegularExpression(
            ValidationRegexes.CharacterValidation,
            ErrorMessage = ValidationMessages.StandardStringFormat)]
        [DisplayName("Comments")]
        public string Details { get; set; }

        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        public DateTime? Dob { get; set; } = null;

        public bool DateFromAfterDob => Dob == null || DateFrom >= Dob;
        public bool DateToAfterDob => Dob == null || DateTo >= Dob;

        public string FormattedDateFrom => DateFrom.ConvertToString();
        public string FormattedDateTo => DateTo.ConvertToString();
    }
}
