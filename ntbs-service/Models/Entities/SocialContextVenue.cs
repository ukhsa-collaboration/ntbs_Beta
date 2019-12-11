using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class SocialContextVenue : SocialContextBase
    {
        public int SocialContextVenueId { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [DisplayName("Venue type")]
        public int? VenueTypeId { get; set; }
        public virtual VenueType VenueType { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [DisplayName("Venue name")]
        public string Name { get; set; }

        [RegularExpression(ValidationRegexes.PostcodeValidation, ErrorMessage = ValidationMessages.PostcodeIsNotValid)]
        [DisplayName("Postcode")]
        public string Postcode { get; set; }

        [DisplayName("Frequency")]
        public Frequency? Frequency { get; set; }

        [MaxLength(100)]
        [RegularExpression(
            ValidationRegexes.CharacterValidation,
            ErrorMessage = ValidationMessages.StandardStringFormat)]
        [DisplayName("Comments")]
        public string Details { get; set; }

        public override void SetModelId(int id)
        {
            SocialContextVenueId = id;
        }
    }
}