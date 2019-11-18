using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class TestData : ModelBase
    {
        [RequiredIf("ShouldValidateFull", ErrorMessage = ValidationMessages.TestCarriedOutIsRequired)]
        public bool? HasTestCarriedOut { get; set; }
    }
}
