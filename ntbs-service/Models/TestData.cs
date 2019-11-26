using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ExpressiveAnnotations.Attributes;
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
        [AssertThat("ResultAddedIfTestCarriedOut", ErrorMessage = ValidationMessages.NoTestResult)]
        [AssertThat("NoImpliesEmptyCollection", ErrorMessage = ValidationMessages.RemoveTestResultsBeforeSayingNoSample)]
        public bool? HasTestCarriedOut { get; set; }
        public virtual ICollection<ManualTestResult> ManualTestResults { get; set; }

        [NotMapped]
        public bool ResultAddedIfTestCarriedOut => !ShouldValidateFull || ManualTestResults == null || ManualTestResults.Any();
        [NotMapped]
        public bool NoImpliesEmptyCollection => HasTestCarriedOut == true || ManualTestResults == null || !ManualTestResults.Any();
    }
}
