using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models;

namespace ntbs_service.DataAccess
{
    public interface INotificationClusterRepository
    {
        Task<IEnumerable<NotificationClusterValue>> GetNotificationClusterValues();
        Task<NotificationClusterValue> GetNotificationClusterValue(int etsNotificationId);
        Task SetNotificationClusterValue(int etsNotificationId, int ntbsNotificationId);
    }

    public class NotificationClusterRepository : INotificationClusterRepository
    {
        private readonly string _reportingDbConnectionString;

        public NotificationClusterRepository(IConfiguration configuration)
        {
            _reportingDbConnectionString = configuration.GetConnectionString(Constants.DbConnectionStringReporting);
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

        public async Task<NotificationClusterValue> GetNotificationClusterValue(int etsNotificationId)
        {
            var query = $@"
                SELECT
                    [{nameof(NotificationClusterValue.NotificationId)}]
                    ,[{nameof(NotificationClusterValue.ClusterId)}]
                FROM [dbo].[NotificationClusterMatch]
                WHERE [{nameof(NotificationClusterValue.NotificationId)}] = @etsNotificationId";

            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<NotificationClusterValue>(query, new { etsNotificationId });
            }
        }

        public async Task SetNotificationClusterValue(int etsNotificationId, int ntbsNotificationId)
        {
            using (var connection = new SqlConnection(_reportingDbConnectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(
                    @"EXEC [dbo].[uspUpdateNotificationClusterMatchWithNtbsId] @etsNotificationId, @ntbsNotificationId;",
                    new { etsNotificationId, ntbsNotificationId });
            }
        }
    }
}
