using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [AtLeastOneProperty(
        nameof(Address),
        nameof(Postcode),
        nameof(Details),
        ErrorMessage = ValidationMessages.SupplyOneOfTheAddressFields)]
    [Display(Name = "Social context address")]
    public class SocialContextAddress : SocialContextBase
    {
        public int SocialContextAddressId { get; set; }

        public override int Id
        {
            set
            {
                SocialContextAddressId = value;
            }
        }
    }
}
