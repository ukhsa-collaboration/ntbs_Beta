using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;

namespace ntbs_service.DataMigration
{
    public interface INotificationImportHelper
    {
        Task CreateTableIfNotExists();
        string GetImportedNotificationsTableName();
    }

    public class NotificationImportHelper : INotificationImportHelper
    {
        private readonly string connectionString;
        private readonly string importedNotificationsTableName;
        private readonly string createImportedNotificationsTableQuery = @"
            IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'))
            CREATE TABLE {0} (
                LegacyId varchar(255) NOT NULL PRIMARY KEY,
                ImportedAt datetime NOT NULL
            )";

        public NotificationImportHelper(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("migration");
            importedNotificationsTableName = $"{configuration.GetSection("Environment")?["Name"]}ImportedNotifications";
        }

        public async Task CreateTableIfNotExists()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = String.Format(createImportedNotificationsTableQuery, importedNotificationsTableName);
                await connection.QueryAsync(query);
            }
        }

        public string GetImportedNotificationsTableName() => importedNotificationsTableName;
    }
}
