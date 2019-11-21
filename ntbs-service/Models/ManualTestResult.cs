using System;
using System.ComponentModel;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    public class ManualTestResult
    {
        public int ManualTestResultId { get; set; }
        [DisplayName("Test Date")]
        public DateTime TestDate { get; set; }
        public int NotificationId { get; set; }
        public int ManualTestTypeId { get; set; }
        public int SampleTypeId { get; set; }
        [DisplayName("Result")]
        public Result Result { get; set; }

        public virtual Notification Notification { get; set; }
        [DisplayName("Test Type")]
        public virtual ManualTestType ManualTestType { get; set; }
        [DisplayName("Sample Type")]
        public virtual SampleType SampleType { get; set; }
    }
}
