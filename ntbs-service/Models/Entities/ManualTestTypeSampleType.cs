namespace ntbs_service.Models.Entities
{
    public class ManualTestTypeSampleType
    {
        public int ManualTestTypeId { get; set; }
        public virtual ManualTestType ManualTestType { get; set; }

        public int SampleTypeId { get; set; }
        public virtual SampleType SampleType { get; set; }
    }
}
