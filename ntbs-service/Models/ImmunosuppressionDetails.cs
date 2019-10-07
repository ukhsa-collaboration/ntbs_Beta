using Microsoft.EntityFrameworkCore;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class ImmunosuppressionDetails : ModelBase
    {
        [AssertThat(@"Status != Enums.Status.Yes
            || (HasBioTherapy || HasTransplantation || HasOther)", 
            ErrorMessage = ValidationMessages.ImmunosuppressionTypeRequired)]
        public Status? Status { get; set; }

        public bool HasBioTherapy { get; set; }
        public bool HasTransplantation { get; set; }
        public bool HasOther { get; set; }

        [RequiredIf("Status == Enums.Status.Yes && HasOther == true",
            ErrorMessage = ValidationMessages.ImmunosuppressionDetailRequired)]
        public string OtherDescription { get; set; }
    }
}