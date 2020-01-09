using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ISpecimenService
    {
        Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId);
    }

    public class SpecimenService : ISpecimenService
    {
        private readonly string connectionString;

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

        public SpecimenService(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("reporting");
        }

        public async Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Specimen>(getMatchedSpecimenSqlFunction, new { notificationId });
            }
        }
    }
}