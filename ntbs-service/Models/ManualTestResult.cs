using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class ManualTestResult
    {
        public int ManualTestResultId { get; set; }
        public int NotificationId { get; set; }
        // We are not including a navigation property to Notification, otherwise it gets validated
        // on every TryValidateModel(manualTestResult)

        [DisplayName("Test Date")]
        // [ValidDateRange(ValidDates.EarliestClinicalDate)]
        [AssertThat(@"TestDateBeforeDob", ErrorMessage = ValidationMessages.NotificationDateShouldBeLaterThanDob)]
        public DateTime TestDate { get; set; }

        [DisplayName("Result")]
        public Result Result { get; set; }

        public int ManualTestTypeId { get; set; }
        [DisplayName("Test Type")]
        public virtual ManualTestType ManualTestType { get; set; }

        public int SampleTypeId { get; set; }
        [DisplayName("Sample Type")]
        public virtual SampleType SampleType { get; set; }
        
        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        public DateTime? Dob { get; set; } = null;

        [NotMapped]
        public bool TestDateBeforeDob => Dob == null || TestDate >= Dob;
    }
}
