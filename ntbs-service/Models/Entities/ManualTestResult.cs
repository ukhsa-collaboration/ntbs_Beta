using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Display(Name = "Test result")]
    public class ManualTestResult : ModelBase, IHasRootEntityForAuditing
    {
        // Even for values which are non-nullable in db, we make them a nullable runtime type so 
        // the Required attribute can be applied properly, producing correct error messages

        public int ManualTestResultId { get; set; }
        public int NotificationId { get; set; }
        // We are not including a navigation property to Notification, otherwise it gets validated
        // on every TryValidateModel(manualTestResult)

        [Display(Name = "Test date")]
        [Required(ErrorMessage = ValidationMessages.RequiredEnter)]
        [ValidClinicalDate]
        [AssertThat(nameof(TestDateAfterDob), ErrorMessage = ValidationMessages.DateShouldBeLaterThanDob)]
        public DateTime? TestDate { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [AssertThat(nameof(ResultMatchesTestType), ErrorMessage = "Select a result that matches test type")]
        [Display(Name = "Result")]
        public Result? Result { get; set; }

        [RegularExpression(ValidationRegexes.CharacterValidationLocalPatientId, ErrorMessage = ValidationMessages.InvalidCharacter)]
        [MaxLength(30, ErrorMessage = ValidationMessages.MaximumTextLength)]
        [Display(Name = "Reason for no result")]
        public string NoResultReason { get; set; }

        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Test type")]
        public int? ManualTestTypeId { get; set; }
        public virtual ManualTestType ManualTestType { get; set; }

        [RequiredIf(nameof(TestHasSampleTypes), ErrorMessage = ValidationMessages.RequiredSelect)]
        [Display(Name = "Sample type")]
        [AssertThat(nameof(TestAndSampleTypesMatch), ErrorMessage = ValidationMessages.InvalidTestAndSampleTypeCombination)]
        public int? SampleTypeId { get; set; }
        public virtual SampleType SampleType { get; set; }

        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        public DateTime? Dob { get; set; }

        [NotMapped]
        public bool TestDateAfterDob => Dob == null || TestDate >= Dob;

        [NotMapped]
        public bool? ResultMatchesTestType => Result?.IsValidForTestType(ManualTestTypeId.GetValueOrDefault());

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

        [NotMapped]
        public string ResultDisplayString => Result.GetDisplayName() + "\n" +
                                             (Result == Enums.Result.NoResult && !string.IsNullOrEmpty(NoResultReason)
                                                 ? $" - {NoResultReason}"
                                                 : string.Empty);

        string IHasRootEntityForAuditing.RootEntityType => RootEntities.Notification;
        string IHasRootEntityForAuditing.RootId => NotificationId.ToString();
    }
}
