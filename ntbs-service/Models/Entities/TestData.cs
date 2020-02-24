using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    // Unlike most of these classes, TestData is not owned - this is purely due to ef limitations around
    // owned entities being principal sides of non-ownership relationships
    // We use the latter to simplify modeling the validation for ''ManualTestResults' presence
    public class TestData : ModelBase, IOwnedEntityForAuditing
    {
        public int NotificationId { get; set; }

        [Display(Name = "Test carried out")]
        [RequiredIf(nameof(ShouldValidateFull), ErrorMessage = ValidationMessages.Mandatory)]
        [AssertThat(nameof(ResultAddedIfTestCarriedOut), ErrorMessage = ValidationMessages.NoTestResult)]
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

        // Only used to inform validation, much like the `ShouldValidateFull` property
        [NotMapped]
        public bool ProceedingToAdd { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
