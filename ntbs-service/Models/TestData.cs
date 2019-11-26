using System.Collections.Generic;
using System.ComponentModel;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    // Unlike most of these classes, TestData is not owned - this is purely due to ef limitations around
    // owned entities being principal sides of non-ownership relationships
    // We use the latter to simplify modelling the validation for ManualTestReults' presence
    public class TestData : ModelBase
    {
        public int NotificationId {get; set; }

        [DisplayName("Test carried out")]
        [RequiredIf("ShouldValidateFull", ErrorMessage = ValidationMessages.Mandatory)]
        public bool? HasTestCarriedOut { get; set; }
        public virtual ICollection<ManualTestResult> ManualTestResults { get; set; }
    }
}
