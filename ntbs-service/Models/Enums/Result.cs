using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Enums
{
    public enum Result
    {
        // Tb X-ray results
        [Display(Name="Consistent with TB - cavities")]
        ConsistentWithTbCavities,
        [Display(Name="Consistent with TB - other")]
        ConsistentWithTbOther,
        [Display(Name="Not consistent with TB")]
        NotConsistentWithTb,
        // Other test types
        [Display(Name="Positive")]
        Positive,
        [Display(Name="Negative")]
        Negative,
        // Universal
        [Display(Name="Awaiting")]
        Awaiting
    }
    
    public static class ResultHelper {
        public static bool IsValidForTestType(this Result result, int testTypeId)
        {
            switch (result)
            {
                case Result.Awaiting:
                    return true;
                case Result.ConsistentWithTbCavities:
                case Result.ConsistentWithTbOther:
                case Result.NotConsistentWithTb:
                    return testTypeId == (int)ManualTestTypeId.ChestXRay;
                case Result.Positive:
                case Result.Negative:
                    return testTypeId != (int)ManualTestTypeId.ChestXRay;
                default: return false;
            }
        }
    }
}
