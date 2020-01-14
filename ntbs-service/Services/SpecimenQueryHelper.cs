using System.Collections.Generic;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public static class SpecimenQueryHelper
    {
        private static readonly string _selectUnmatchedSpecimensQuery = $@"
            SELECT
                [{nameof(UnmatchedSpecimen.ReferenceLaboratoryNumber)}]
                ,[{nameof(UnmatchedSpecimen.SpecimenDate)}]
                ,[{nameof(UnmatchedSpecimen.SpecimenTypeCode)}]
                ,[{nameof(UnmatchedSpecimen.LaboratoryName)}]
                ,[{nameof(UnmatchedSpecimen.ReferenceLaboratory)}]
                ,[{nameof(UnmatchedSpecimen.Species)}]
                ,[{nameof(UnmatchedSpecimen.LabNhsNumber)}]
                ,[{nameof(UnmatchedSpecimen.LabBirthDate)}]
                ,[{nameof(UnmatchedSpecimen.LabName)}]
                ,[{nameof(UnmatchedSpecimen.LabSex)}]
                ,[{nameof(UnmatchedSpecimen.LabAddress)}]
                ,[{nameof(UnmatchedSpecimen.LabPostcode)}]
                ,[{nameof(UnmatchedSpecimen.TbServiceName)}]
                ,[{nameof(SpecimenPotentialMatch.NotificationId)}]
                ,[{nameof(SpecimenPotentialMatch.NotificationDate)}]
                ,[{nameof(SpecimenPotentialMatch.NtbsNhsNumber)}]
                ,[{nameof(SpecimenPotentialMatch.NtbsName)}]
                ,[{nameof(SpecimenPotentialMatch.NtbsSex)}]
                ,[{nameof(SpecimenPotentialMatch.NtbsBirthDate)}]
                ,[{nameof(SpecimenPotentialMatch.NtbsAddress)}]
                ,[{nameof(SpecimenPotentialMatch.NtbsPostcode)}]
                ,[{nameof(SpecimenPotentialMatch.ConfidenceLevel)}]";

        public static readonly string GetMatchedSpecimensForNotificationQuery = $@"
            SELECT 
                ,[{nameof(MatchedSpecimen.NotificationId)}]
                ,[{nameof(MatchedSpecimen.ReferenceLaboratoryNumber)}]
                ,[{nameof(MatchedSpecimen.SpecimenTypeCode)}]
                ,[{nameof(MatchedSpecimen.SpecimenDate)}]
                ,[{nameof(MatchedSpecimen.Isoniazid)}]
                ,[{nameof(MatchedSpecimen.Rifampicin)}]
                ,[{nameof(MatchedSpecimen.Pyrazinamide)}]
                ,[{nameof(MatchedSpecimen.Ethambutol)}]
                ,[{nameof(MatchedSpecimen.Aminoglycoside)}]
                ,[{nameof(MatchedSpecimen.Quinolone)}]
                ,[{nameof(MatchedSpecimen.MDR)}]
                ,[{nameof(MatchedSpecimen.XDR)}]
                ,[{nameof(MatchedSpecimen.Species)}]
                ,[{nameof(MatchedSpecimen.LabNhsNumber)}]
                ,[{nameof(MatchedSpecimen.LabBirthDate)}]
                ,[{nameof(MatchedSpecimen.LabName)}]
                ,[{nameof(MatchedSpecimen.LabSex)}]
                ,[{nameof(MatchedSpecimen.LabAddress)}]
            FROM [dbo].[ufnGetMatchedSpecimen] (@param)";

        public static string GetAllUnmatchedSpecimensQuery =>
            string.Join(
                ' ',
                _selectUnmatchedSpecimensQuery,
                "FROM [dbo].[ufnGetAllUnmatchedSpecimens] ()");

        public static string GetUnmatchedSpecimensForTbServicesQuery =>
            string.Join(
                ' ',
                _selectUnmatchedSpecimensQuery,
                "FROM [dbo].[ufnGetUnmatchedSpecimensByService] (@param)");

        public static string GetUnmatchedSpecimensForPhecsQuery =>
            string.Join(
                ' ',
                _selectUnmatchedSpecimensQuery,
                "FROM [dbo].[ufnGetUnmatchedSpecimensByPhec] (@param)");

        public static string FormatEnumerableParams(IEnumerable<string> enumerable) =>
            string.Join(',', enumerable);
    }
}
