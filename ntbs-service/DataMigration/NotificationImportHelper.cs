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
        string GetInsertImportedNotificationQuery();
        string GetSelectImportedNotificationByIdQuery();
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

        private const string InsertImportedNotificationsQuery = @"
            INSERT INTO {0} (LegacyId, ImportedAt)
            VALUES (@LegacyId, @ImportedAt);
        ";

        private const string SelectImportedNotificationByIdQuery = @"
            SELECT *
            FROM {0} impNtfc
            WHERE impNtfc.LegacyId = n.OldNotificationId";

        public NotificationImportHelper(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("migration");
            var tablePrefix = configuration.GetSection("Migration")?["TablePrefix"];
            importedNotificationsTableName = $"{tablePrefix}ImportedNotifications";
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

        public string GetInsertImportedNotificationQuery() => string.Format(InsertImportedNotificationsQuery, importedNotificationsTableName);

        public string GetImportedNotificationsTableName() => importedNotificationsTableName;

        public string GetSelectImportedNotificationByIdQuery() => string.Format(SelectImportedNotificationByIdQuery, importedNotificationsTableName);
    }
}
