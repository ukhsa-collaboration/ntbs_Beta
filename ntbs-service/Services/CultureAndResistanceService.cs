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
        private readonly string getCultureAndResistanceDetailsSqlFunction = @"
            SELECT NotificationId,
                CulturePositive,
                Species,
                EarliestSpecimenDate,
                DrugResistanceProfile,
                Isoniazid,
                Rifampicin,
                Pyrazinamide,
                Ethambutol,
                Aminoglycocide,
                Quinolone,
                MDR,
                XDR 
            FROM [dbo].[ufnGetNotificationCultureAndResistanceSummary] (@notificationId)";

        public CultureAndResistanceService(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("reporting");
        }

        public async Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var rawResult = await connection.QueryAsync<CultureAndResistance>(getCultureAndResistanceDetailsSqlFunction, new { notificationId });
                return rawResult.FirstOrDefault();
            }
        }
    }
}