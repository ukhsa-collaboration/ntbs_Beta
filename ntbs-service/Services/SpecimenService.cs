using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ISpecimenService
    {
        Task<IEnumerable<MatchedSpecimen>> GetMatchedSpecimenDetailsForNotificationAsync(int notificationId);

        Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimenDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCode);

        Task UnmatchSpecimen(int notificationId, string labReferenceNumber);
    }

    public class SpecimenService : ISpecimenService
    {
        private readonly string _reportingDbConnectionString;
        private readonly string _specimenMatchingDbConnectionString;
        private const string _unmatchSpecimenSqlProcedure = "uspUnmatchSpecimen";

        private const string _getMatchedSpecimenSqlFunction = @"
            SELECT NotificationId,
                ReferenceLaboratoryNumber,
                SpecimenTypeCode,
                SpecimenDate,
                Isoniazid,
                Rifampicin,
                Pyrazinamide,
                Ethambutol,
                Aminoglycoside,
                Quinolone,
                MDR,
                XDR,
                Species,
                LabNhsNumber,
                LabBirthDate,
                LabName,
                LabSex,
                LabAddress
            FROM [dbo].[ufnGetMatchedSpecimen] (@notificationId)
            ORDER BY SpecimenDate DESC";

        private static string _getUnmatchedSpecimenSqlFunction = $@"
            SELECT
                [{nameof(SpecimenBase.ReferenceLaboratoryNumber)}]
                ,[SpecimenDate]
                ,[SpecimenTypeCode]
                ,[LaboratoryName]
                ,[ReferenceLaboratory]
                ,[Species]
                ,[LabPatientNHSNumber]
                ,[LabPatientBirthDate]
                ,[LabPatientName]
                ,[LabPatientSex]
                ,[LabPatientAddress]
                ,[LabPatientPostcode]
                ,[TbServiceName]
                ,[NotificationID]
                ,[NotificationDate]
                ,[NtbsNHSNumber]
                ,[NtbsName]
                ,[NtbsSex]
                ,[NtbsBirthDate]
                ,[NtbsAddress]
                ,[NtbsPostcode]
                ,[ConfidenceLevel]
            FROM [dbo].[ufnGetUnmatchedSpecimensByService]
            (@tbService)";

        public SpecimenService(IConfiguration configuration)
        {
            _reportingDbConnectionString = configuration.GetConnectionString("reporting");
            _specimenMatchingDbConnectionString = configuration.GetConnectionString("specimenMatching");
        }

        public async Task<IEnumerable<MatchedSpecimen>> GetMatchedSpecimenDetailsForNotificationAsync(
            int notificationId)
        {
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<MatchedSpecimen>(
                    _getMatchedSpecimenSqlFunction,
                    new {notificationId});
            }
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimenDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCode)
        {
            IEnumerable<UnmatchedResultRow> unmatchedResultRows;
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                // TODO: Actually feed in the tbServices, figure out what format nancy wants them
                // at present it seems like a comma delimited list (but spaces/not spaces etc.)?
                unmatchedResultRows =
                    await connection.QueryAsync<SpecimenBase, SpecimenPotentialMatch, UnmatchedResultRow>(
                        _getUnmatchedSpecimenSqlFunction,
                        (specimen, potentialMatch) => new UnmatchedResultRow
                        {
                            SpecimenBase = specimen, SpecimenPotentialMatch = potentialMatch
                        },
                        new {tbServiceCode});
            }

            return unmatchedResultRows.GroupBy(
                row => row.SpecimenBase.ReferenceLaboratoryNumber,
                row => row,
                (referenceNumber, rows) =>
                {
                    var groupedRows = rows.ToList();
                    var specimenData = groupedRows.First().SpecimenBase;
                    return new UnmatchedSpecimen
                    {
                        ReferenceLaboratoryNumber = specimenData.ReferenceLaboratoryNumber,
                        SpecimenDate = specimenData.SpecimenDate,
                        SpecimenTypeCode = specimenData.SpecimenTypeCode,
                        LaboratoryName = specimenData.LaboratoryName,
                        ReferenceLaboratory = specimenData.ReferenceLaboratory,
                        Species = specimenData.Species,
                        LabNhsNumber = specimenData.LabNhsNumber,
                        LabBirthDate = specimenData.LabBirthDate,
                        LabName = specimenData.LabName,
                        LabSex = specimenData.LabSex,
                        LabAddress = specimenData.LabAddress,
                        LabPostcode = specimenData.LabPostcode,
                        PotentialMatches = groupedRows.Select(r => r.SpecimenPotentialMatch)
                    };
                });
        }

        public async Task UnmatchSpecimen(int notificationId, string referenceLaboratoryNumber)
        {
            using (var connection = new SqlConnection(_specimenMatchingDbConnectionString))
            {
                connection.Open();
                await connection.QueryAsync(
                    _unmatchSpecimenSqlProcedure,
                    new {referenceLaboratoryNumber, notificationId},
                    commandType: CommandType.StoredProcedure);
            }
        }


        private class UnmatchedResultRow
        {
            internal SpecimenBase SpecimenBase { get; set; }

            internal SpecimenPotentialMatch SpecimenPotentialMatch { get; set; }
        }
    }
}
