using System;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models
{
    public class ManualTestResult
    {
        public int ManualTestResultId { get; set; }
        public DateTime TestDate { get; set; }
        public int NotificationId { get; set; }
        public int ManualTestTypeId { get; set; }
        public int SampleTypeId { get; set; }
        public Result Result { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual ManualTestType ManualTestType { get; set; }
        public virtual SampleType SampleType { get; set; }
    }
}
