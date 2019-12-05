using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class ManualTestResult
    {
        // Even for values which are non-nullable in db, we make them a nullable runtime type so 
        // the Required attribute can be applied properly, producing correct error messages

        public int ManualTestResultId { get; set; }
        public int NotificationId { get; set; }
        // We are not including a navigation property to Notification, otherwise it gets validated
        // on every TryValidateModel(manualTestResult)

        [DisplayName("Test date")]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        [AssertThat(@"TestDateBeforeDob", ErrorMessage = ValidationMessages.NotificationDateShouldBeLaterThanDob)]
        public DateTime? TestDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [DisplayName("Result")]
        public Result? Result { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [DisplayName("Test type")]
        public int? ManualTestTypeId { get; set; }
        public virtual ManualTestType ManualTestType { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [DisplayName("Sample type")]
        [AssertThat("TestAndSampleTypesMatch", ErrorMessage = ValidationMessages.InvalidTestAndSampleTypeCombination)]
        public int? SampleTypeId { get; set; }
        public virtual SampleType SampleType { get; set; }

        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        public DateTime? Dob { get; set; } = null;

        [NotMapped]
        public bool TestDateBeforeDob => Dob == null || TestDate >= Dob;

        [NotMapped]
        public bool TestAndSampleTypesMatch =>
            // Either the navigation properties are not loaded yet, or...
            ManualTestType == null ||
            // ... the entities and sample match
            ManualTestType.ManualTestTypeSampleTypes
                .Any(ts => ts.SampleType == SampleType);
    }
}
