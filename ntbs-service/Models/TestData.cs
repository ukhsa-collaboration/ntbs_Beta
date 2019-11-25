using System.ComponentModel;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    [Owned]
    public class TestData : ModelBase
    {
        [DisplayName("Test carried out")]
        [RequiredIf("ShouldValidateFull", ErrorMessage = ValidationMessages.Mandatory)]
        public bool? HasTestCarriedOut { get; set; }
    }
}
