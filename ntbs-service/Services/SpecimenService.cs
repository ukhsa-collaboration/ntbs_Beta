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

        public SpecimenService(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("reporting");
        }

        public async Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            string query = "SELECT * FROM [dbo].[ufnGetMatchedSpecimen] (@notificationId)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var rawResult = await connection.QueryAsync(query, new { notificationId });
                return rawResult.Select(AsSpecimen);
            }
        }

        private static Specimen AsSpecimen(dynamic rawResult)
        {
            return new Specimen
            {
                NotificationId = rawResult.NotificationID,
                ReferenceLaboratoryNumber = rawResult.ReferenceLaboratoryNumber,
                SpecimenDate = rawResult.SpecimenDate,
                SpecimenTypeCode = rawResult.SpecimenTypeCode,
                LaboratoryName = rawResult.LaboratoryName,
                ReferenceLaboratory = rawResult.ReferenceLaboratory,
                Species = rawResult.Species,
                Isoniazid = rawResult.Isoniazid,
                Rifampicin = rawResult.Rifampicin,
                Pyrazinamide = rawResult.Pyrazinamide,
                Ethambutol = rawResult.Ethambutol,
                Aminoglycocide = rawResult.Aminoglycocide,
                Quinolone = rawResult.Quinolone,
                MDR = rawResult.MDR,
                XDR = rawResult.XDR,
                PatientNhsNumber = rawResult.PatientNhsNumber,
                PatientBirthDate = rawResult.PatientBirthDate,
                PatientName = rawResult.PatientName,
                PatientSex = rawResult.PatientSex,
                PatientAddress = rawResult.PatientAddress,
                PatientPostcode = rawResult.PatientPostcode,
            };
        }
    }
}