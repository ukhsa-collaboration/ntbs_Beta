using System.Collections.Generic;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.SeedData
{
    public class ManualTestTypes
    {
        public static IEnumerable<ManualTestType> GetManualTestTypes()
        {
            return new List<ManualTestType>
            {
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.Smear, Description = "Smear" },
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.Culture, Description = "Culture" },
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.Histology, Description = "Histology" },
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.ChestXRay, Description = "Chest x-ray" },
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.Pcr, Description = "PCR" },
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.LineProbeAssay, Description = "Line probe assay" },
                new ManualTestType { ManualTestTypeId = (int)ManualTestTypeId.ChestCT, Description = "Chest CT" },
            };
        }
    }
}
