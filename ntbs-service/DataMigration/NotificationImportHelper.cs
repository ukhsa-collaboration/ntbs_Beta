using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.DataMigration
{
    public interface INotificationImportHelper
    {
        Task CreateTableIfNotExists();
        string InsertImportedNotificationQuery { get; }
        string SelectImportedNotificationByIdQuery { get; }
    }

    public class NotificationImportHelper : INotificationImportHelper
    {
        private readonly string connectionString;
        private readonly string importedNotificationsTableName;

        // Dapper does not support dynamic table names as parameters therefore we are using string format
        // This is relatively safe as the parameter is passed from environment variable rather that user input
        private readonly string createImportedNotificationsTableQuery = @"
            IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'))
            CREATE TABLE {0} (
                LegacyId varchar(255) NOT NULL PRIMARY KEY,
                ImportedAt datetime NOT NULL
            )";

        private const string InsertImportedNotificationTemplate = @"
            INSERT INTO {0} (LegacyId, ImportedAt)
            VALUES (@LegacyId, @ImportedAt);
        ";

        private const string SelectImportedNotificationByIdTemplate = @"
            SELECT *
            FROM {0} impNtfc
            WHERE impNtfc.LegacyId = n.PrimaryNotificationId";

        public NotificationImportHelper(IConfiguration configuration, IOptions<MigrationConfig> migrationConfig)
        {
            connectionString = configuration.GetConnectionString("migration");
            var tablePrefix = migrationConfig.Value.TablePrefix;
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

        public string InsertImportedNotificationQuery => string.Format(InsertImportedNotificationTemplate, importedNotificationsTableName);
        public string SelectImportedNotificationByIdQuery => string.Format(SelectImportedNotificationByIdTemplate, importedNotificationsTableName);
    }
}
