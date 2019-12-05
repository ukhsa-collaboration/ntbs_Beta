using System.Collections.Generic;

namespace ntbs_service.Models.Entities
{
    public class ManualTestType
    {
        public int ManualTestTypeId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ManualTestTypeSampleType> ManualTestTypeSampleTypes { get; set; }
    }

    public enum ManualTestTypeId
    {
        Smear = 1,
        Culture = 2,
        Histology = 3,
        ChestXRay = 4,
        Pcr = 5,
        LineProbeAssay = 6
    }
}
