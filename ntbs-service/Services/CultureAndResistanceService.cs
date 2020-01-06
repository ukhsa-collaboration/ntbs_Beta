using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Linq;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ICultureAndResistanceService
    {
        Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId);
    }

    public class CultureAndResistanceService : ICultureAndResistanceService
    {
        private readonly string connectionString;

        public CultureAndResistanceService(IConfiguration _configuration)
        {
            connectionString = "";
        }

        public async Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId)
        {
            string query = "SELECT * FROM [dbo].[ufnGetNotificationCultureAndResistanceSummary] (@notificationId)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var rawResult = await connection.QueryAsync(query, new { notificationId });
                return rawResult.Select(AsCultureAndResistance).FirstOrDefault();
            }
        }

        private static CultureAndResistance AsCultureAndResistance(dynamic rawResult)
        {
            return new CultureAndResistance
            {
                NotificationId = rawResult.NotificationId,
                CulturePositive = rawResult.CulturePositive,
                Species = rawResult.Species,
                DrugResistanceProfile = rawResult.DrugResistanceProfile,
                EarliestSpecimenDate = rawResult.EarliestSpecimenDate,
                Isoniazid = rawResult.Isoniazid,
                Rifampicin = rawResult.Rifampicin,
                Pyrazinamide = rawResult.Pyrazinamide,
                Ethambutol = rawResult.Ethambutol,
                Aminoglycocide = rawResult.Aminoglycocide,
                Quinolone = rawResult.Quinolone,
                MDR = rawResult.MDR,
                XDR = rawResult.XDR,
            };
        }
    }
}