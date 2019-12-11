using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class SocialContextAddress : SocialContextBase
    {
        public int SocialContextAddressId { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [RegularExpression(ValidationRegexes.PostcodeValidation, ErrorMessage = ValidationMessages.PostcodeIsNotValid)]
        [DisplayName("Postcode")]
        public string Postcode { get; set; }

        public override void SetModelId(int id)
        {
            SocialContextAddressId = id;
        }
    }
}
