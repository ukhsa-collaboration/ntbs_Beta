using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Entities;
using System.Data;

namespace ntbs_service.Services
{
    public interface ISpecimenService
    {
        Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId);
        Task UnmatchSpecimen(int notificationId, string labReferenceNumber);
    }

    public class SpecimenService : ISpecimenService
    {
        private readonly string _reportingDbConnectionString;
        private readonly string _specimenMatchingDbConnectionString;

        private readonly string getMatchedSpecimenSqlFunction = @"
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

        private readonly string unmatchSpecimentSqlProcedure = @"uspUnmatchSpecimen";

        public SpecimenService(IConfiguration _configuration)
        {
            _reportingDbConnectionString = _configuration.GetConnectionString("reporting");
            _specimenMatchingDbConnectionString = _configuration.GetConnectionString("specimenMatching");
        }

        public async Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Specimen>(getMatchedSpecimenSqlFunction, new { notificationId });
            }
        }

        public async Task UnmatchSpecimen(int notificationId, string referenceLaboratoryNumber)
        {
            using (var connection = new SqlConnection(_specimenMatchingDbConnectionString))
            {
                connection.Open();
                await connection.QueryAsync(unmatchSpecimentSqlProcedure,
                                            new { referenceLaboratoryNumber, notificationId },
                                            commandType: CommandType.StoredProcedure);
            }
        }
    }
}
