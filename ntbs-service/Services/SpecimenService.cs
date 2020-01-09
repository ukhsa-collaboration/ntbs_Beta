using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
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
        private readonly string reportingDbConnectionString;
        private readonly string matchSpecimenDbConnectionString;

        private readonly string getMatchedSpecimenSqlFunction = @"
            SELECT NotificationId,
                ReferenceLaboratoryNumber,
                SpecimenTypeCode,
                SpecimenDate,
                Isoniazid,
                Rifampicin,
                Pyrazinamide,
                Ethambutol,
                Aminoglycocide,
                Quinolone,
                MDR,
                XDR,
                Species,
                PatientNhsNumber,
                PatientBirthDate,
                PatientName,
                PatientSex,
                PatientAddress
            FROM [dbo].[ufnGetMatchedSpecimen] (@notificationId)";

        private readonly string unmatchSpecimentSqlProcedure = @"uspUnmatchSpecimen";

        public SpecimenService(IConfiguration _configuration)
        {
            reportingDbConnectionString = _configuration.GetConnectionString("reporting");
            matchSpecimenDbConnectionString = _configuration.GetConnectionString("specimenMatching");
        }

        public async Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            using (var connection = new SqlConnection(reportingDbConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Specimen>(getMatchedSpecimenSqlFunction, new { notificationId });
            }
        }

        public async Task UnmatchSpecimen(int notificationId, string labReferenceNumber)
        {
            {
                using (var connection = new SqlConnection(matchSpecimenDbConnectionString))
                {
                    connection.Open();
                    // Add parameters when procedure is implemented properly
                    await connection.QueryAsync(unmatchSpecimentSqlProcedure, commandType: CommandType.StoredProcedure);
                }
            }
        }
    }
}