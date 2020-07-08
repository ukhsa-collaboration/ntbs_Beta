using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [AtLeastOneProperty(
        nameof(VenueTypeId),
        nameof(Name),
        nameof(Address),
        nameof(Postcode),
        nameof(Details),
        ErrorMessage = ValidationMessages.SupplyOneOfTheVenueFields)]
    [Display(Name = "Social Context Venue")]
    public partial class SocialContextVenue : SocialContextBase
    {
        public int SocialContextVenueId { get; set; }

        [Display(Name = "Venue type")]
        public int? VenueTypeId { get; set; }
        public virtual VenueType VenueType { get; set; }

        [MaxLength(40)]
        [Display(Name = "Venue name")]
        public string Name { get; set; }

        [Display(Name = "Frequency")]
        public Frequency? Frequency { get; set; }

        public override int Id
        {
            set
            {
                SocialContextVenueId = value;
            }
        }
    }
}
