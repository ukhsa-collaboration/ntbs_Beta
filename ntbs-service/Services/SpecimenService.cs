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

        Task<IEnumerable<UnmatchedSpecimen>> GetAllUnmatchedSpecimensAsync();

        Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCodes);

        Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForPhecsAsync(
            IEnumerable<string> phecCodes);

        Task UnmatchSpecimen(int notificationId, string labReferenceNumber);
    }

    public class SpecimenService : ISpecimenService
    {
        private readonly string _reportingDbConnectionString;
        private readonly string _specimenMatchingDbConnectionString;

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
                    SpecimenQueryHelper.GetMatchedSpecimensForNotificationQuery,
                    new {param = notificationId});
            }
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetAllUnmatchedSpecimensAsync()
        {
            var query = SpecimenQueryHelper.GetAllUnmatchedSpecimensQuery;
            var unmatchedQueryResultRows = await ExecuteUnmatchedSpecimenQuery(query);
            return GroupUnmatchedSpecimenRowsByReferenceNumber(unmatchedQueryResultRows);
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCodes)
        {
            var query = SpecimenQueryHelper.GetUnmatchedSpecimensForTbServicesQuery;
            var formattedTbServiceCodes = SpecimenQueryHelper.FormatEnumerableParams(tbServiceCodes);
            var unmatchedQueryResultRows = await ExecuteUnmatchedSpecimenQuery(query, formattedTbServiceCodes);
            return GroupUnmatchedSpecimenRowsByReferenceNumber(unmatchedQueryResultRows);
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForPhecsAsync(
            IEnumerable<string> phecCodes)
        {
            var query = SpecimenQueryHelper.GetUnmatchedSpecimensForPhecsQuery;
            var formattedPhecCodes = SpecimenQueryHelper.FormatEnumerableParams(phecCodes);
            var unmatchedQueryResultRows = await ExecuteUnmatchedSpecimenQuery(query, formattedPhecCodes);
            return GroupUnmatchedSpecimenRowsByReferenceNumber(unmatchedQueryResultRows);
        }

        private async Task<IEnumerable<UnmatchedQueryResultRow>> ExecuteUnmatchedSpecimenQuery(
            string query,
            string param = null)
        {
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<SpecimenBase, SpecimenPotentialMatch, UnmatchedQueryResultRow>(
                    query,
                    (specimen, potentialMatch) => new UnmatchedQueryResultRow
                    {
                        SpecimenBase = specimen, SpecimenPotentialMatch = potentialMatch
                    },
                    splitOn: nameof(SpecimenPotentialMatch.NotificationId),
                    param: new {param});
            }
        }

        private static IEnumerable<UnmatchedSpecimen> GroupUnmatchedSpecimenRowsByReferenceNumber(
            IEnumerable<UnmatchedQueryResultRow> unmatchedResultRows)
        {
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
