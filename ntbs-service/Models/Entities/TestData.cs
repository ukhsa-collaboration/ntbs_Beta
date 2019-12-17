using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    // Unlike most of these classes, TestData is not owned - this is purely due to ef limitations around
    // owned entities being principal sides of non-ownership relationships
    // We use the latter to simplify modelling the validation for ManualTestReults' presence
    public class TestData : ModelBase
    {
        public int NotificationId { get; set; }

        [Display(Name = "Test carried out")]
        [RequiredIf("ShouldValidateFull", ErrorMessage = ValidationMessages.Mandatory)]
        [AssertThat("ResultAddedIfTestCarriedOut", ErrorMessage = ValidationMessages.NoTestResult)]
        public bool? HasTestCarriedOut { get; set; }
        public virtual ICollection<ManualTestResult> ManualTestResults { get; set; }

        [NotMapped]
        public bool ResultAddedIfTestCarriedOut =>
            // This test only makes sense if test are loaded
            ManualTestResults == null
            // if test is carried out, make sure there are results
            || HasTestCarriedOut == false || ManualTestResults.Any()
            // ...unless they are about to add them, in which case that's fine too
            || ProceedingToAdd;

        [NotMapped]
        public bool NoImpliesEmptyCollection =>
            // This test only makes sense if test are loaded
            ManualTestResults == null
            // if test is marked not carried out, make sure there are no results
            || HasTestCarriedOut == true || !ManualTestResults.Any();

        // Only used to inform validation, much like the `ShouldValidateFull` property
        [NotMapped]
        public bool ProceedingToAdd { get; set; }
    }
}
