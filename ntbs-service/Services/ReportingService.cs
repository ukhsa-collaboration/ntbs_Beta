using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface IReportingService
    {
        Task<Dictionary<int, DrugResistanceProfile>> GetDrugResistanceProfiles();
    }
    
    public class ReportingService : IReportingService
    {
        private readonly string _reportingDbConnectionString;

        public ReportingService(IConfiguration configuration)
        {
            _reportingDbConnectionString = configuration.GetConnectionString("reporting");
        }

        public async Task<Dictionary<int, DrugResistanceProfile>> GetDrugResistanceProfiles()
        {
            var query = $@"
                SELECT [ReusableNotificationId] AS NotificationId,
                    [DrugResistanceProfile],
                    [Species]
                FROM [dbo].[ReusableNotification]";

            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                var results = await connection.QueryAsync(query);
                
                return results.ToDictionary(
                    t => (int) t.ReusableNotificationId, 
                    t => new DrugResistanceProfile
                        {
                            DrugResistanceProfileString = t.DrugResistanceProfile,
                            Species = t.Species
                        });
            }
        }
    }
}
