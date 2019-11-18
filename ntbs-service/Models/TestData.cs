using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models
{
    [Owned]
    public class TestData : ModelBase
    {
        [RequiredIf("ShouldValidateFull")]
        public bool? HasTestCarriedOut { get; set; }
    }
}
