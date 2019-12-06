using System.Collections.Generic;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.SeedData
{
    public class SampleTypes
    {
        public static IEnumerable<SampleType> GetSampleTypes()
        {
            return new List<SampleType>
            {
                new SampleType { SampleTypeId = (int)SampleTypeId.BronchialWashings, Description = "Bronchial washings", Category = "Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.BronchoscopySample, Description = "Bronchoscopy sample", Category = "Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.LungBronchialTreeTissue, Description = "Lung bronchial tree tissue", Category = "Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.SputumInduced, Description = "Sputum (induced)", Category = "Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.SputumSpontaneous, Description = "Sputum (spontaneous)", Category = "Respiratory"},

                new SampleType { SampleTypeId = (int)SampleTypeId.Blood, Description = "Blood", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.BoneAndJoint, Description = "Bone and joint", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Cns, Description = "CNS", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Csf, Description = "CSF", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Faeces, Description = "Faeces", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.GastricWashings, Description = "Gastric washings", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Gastrointestinal, Description = "Gastrointestinal", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Genitourinary, Description = "Genitourinary", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Gynaecological, Description = "Gynaecological", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.LymphNode, Description = "Lymph node", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.PeritonealFluid, Description = "Peritoneal fluid", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Pleural, Description = "Pleural", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.PleuralFluidOrBiopsy, Description = "Pleural fluid or biopsy", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Pus, Description = "Pus", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Skin, Description = "Skin", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.Urine, Description = "Urine", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.OtherTissues, Description = "Other tissues", Category = "Non-Respiratory"},
                new SampleType { SampleTypeId = (int)SampleTypeId.NotKnown, Description = "Not known", Category = "Non-Respiratory"}
            };
        }
    }
}
