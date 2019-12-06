using System.Collections.Generic;

namespace ntbs_service.Models
{
    public class SampleType
    {
        public int SampleTypeId { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ManualTestTypeSampleType> ManualTestTypeSampleTypes { get; set; }
    }

    public enum SampleTypeId
    {
        BronchialWashings = 1,
        BronchoscopySample = 2,
        LungBronchialTreeTissue = 3,
        SputumInduced = 4,
        SputumSpontaneous = 5,
        Blood = 6,
        BoneAndJoint = 7,
        Cns = 8,
        Csf = 9,
        Faeces = 10,
        GastricWashings = 11,
        Gastrointestinal = 12,
        Genitourinary = 13,
        Gynaecological = 14,
        LymphNode = 15,
        PeritonealFluid = 16,
        Pleural = 17,
        PleuralFluidOrBiopsy = 18,
        Pus = 19,
        Skin = 20,
        Urine = 21,
        OtherTissues = 22,
        NotKnown = 23
    }
}
