using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface INotificationClusterService
    {
        Task<IEnumerable<NotificationClusterValue>> GetNotificationClusterValues();
    }
    
    public class NotificationClusterService : INotificationClusterService
    {
        private readonly string _reportingDbConnectionString;

        public NotificationClusterService(IConfiguration configuration)
        {
            _reportingDbConnectionString = configuration.GetConnectionString("reporting");
        }
        
        public async Task<IEnumerable<NotificationClusterValue>> GetNotificationClusterValues()
        {
            var query = $@"
                SELECT
                    [{nameof(NotificationClusterValue.NotificationId)}]
                    ,[{nameof(NotificationClusterValue.ClusterId)}]
                FROM [dbo].[vwNotificationClusterMatch]";

            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                return await connection.QueryAsync<NotificationClusterValue>(query);
            }
        }
    }
}
