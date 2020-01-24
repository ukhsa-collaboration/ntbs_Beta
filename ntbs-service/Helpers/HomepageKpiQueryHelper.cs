using System.Collections.Generic;
using ntbs_service.Models.Entities;

namespace ntbs_service.Helpers
{
    public static class HomepageKpiQueryHelper
    {
        public static string GetKpiForPhecQuery { get; } = $@"
            SELECT [{nameof(HomepageKpi.Code)}],
                   [{nameof(HomepageKpi.Name)}],
		           [{nameof(HomepageKpi.PercentPositive)}],
		           [{nameof(HomepageKpi.PercentResistant)}],
		           [{nameof(HomepageKpi.PercentHivOffered)}],
		           [{nameof(HomepageKpi.PercentTreatmentDelay)}]
            FROM [dbo].[ufnGetKPIforPhec] (@param)
        ";

        public static string GetKpiForServiceQuery { get; } = $@"
            SELECT [{nameof(HomepageKpi.Code)}],
                   [{nameof(HomepageKpi.Name)}],
		           [{nameof(HomepageKpi.PercentPositive)}],
		           [{nameof(HomepageKpi.PercentResistant)}],
		           [{nameof(HomepageKpi.PercentHivOffered)}],
		           [{nameof(HomepageKpi.PercentTreatmentDelay)}]
            FROM [dbo].[ufnGetKPIforService] (@param)
        ";

        public static string FormatEnumerableParams(IEnumerable<string> enumerable) =>
            enumerable != null ? string.Join(',', enumerable) : string.Empty;
    }
}
