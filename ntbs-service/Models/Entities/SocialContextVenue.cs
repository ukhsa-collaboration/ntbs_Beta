using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class SocialContextVenue : SocialContextBase
    {
        public int SocialContextVenueId { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Venue type")]
        public int? VenueTypeId { get; set; }
        public virtual VenueType VenueType { get; set; }

        [MaxLength(40)]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [Display(Name = "Venue name")]
        public string Name { get; set; }

        [Display(Name = "Frequency")]
        public Frequency? Frequency { get; set; }

        public override bool PostcodeIsRequired => false;
        public override bool DateToIsRequired => false;

        public override int Id
        {
            set
            {
                SocialContextVenueId = value;
            }
        }
    }
}
