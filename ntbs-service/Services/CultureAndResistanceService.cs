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
            connectionString = _configuration.GetConnectionString("reporting");
        }

        public async Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId)
        {
            string query = "SELECT * FROM [dbo].[ufnGetNotificationCultureAndResistanceSummary] (@notificationId)";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var rawResult = await connection.QueryAsync<CultureAndResistance>(query, new { notificationId });
                return rawResult.FirstOrDefault();
            }
        }
    }
}