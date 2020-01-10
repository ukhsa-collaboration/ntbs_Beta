using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class ManualTestResult : IHasRootEntity
    {
        // Even for values which are non-nullable in db, we make them a nullable runtime type so 
        // the Required attribute can be applied properly, producing correct error messages

        public int ManualTestResultId { get; set; }
        public int NotificationId { get; set; }
        // We are not including a navigation property to Notification, otherwise it gets validated
        // on every TryValidateModel(manualTestResult)

        [Display(Name = "Test date")]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [ValidDateRange(ValidDates.EarliestClinicalDate)]
        [AssertThat(@"TestDateAfterDob", ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? TestDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Result")]
        public Result? Result { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Test type")]
        public int? ManualTestTypeId { get; set; }
        public virtual ManualTestType ManualTestType { get; set; }

        [RequiredIf("TestHasSampleTypes", ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Sample type")]
        [AssertThat("TestAndSampleTypesMatch", ErrorMessage = ValidationMessages.InvalidTestAndSampleTypeCombination)]
        public int? SampleTypeId { get; set; }
        public virtual SampleType SampleType { get; set; }

        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        public DateTime? Dob { get; set; } = null;

        [NotMapped]
        public bool TestDateAfterDob => Dob == null || TestDate >= Dob;


        [NotMapped]
        public bool TestAndSampleTypesMatch =>
            // Either the navigation properties are not loaded yet, or...
            ManualTestType == null ||
            // ... the entities and sample match
            ManualTestType.ManualTestTypeSampleTypes
                .Any(ts => ts.SampleType == SampleType);

        [NotMapped]
        public bool TestHasSampleTypes =>
            ManualTestType != null
            && ManualTestType.ManualTestTypeSampleTypes.Any();

        string IHasRootEntity.RootEntityType => RootEntities.Notification;
        string IHasRootEntity.RootId => NotificationId.ToString();
    }
}
