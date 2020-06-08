using System;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ntbs_service.DataAccess;
using ntbs_service.Properties;

namespace ntbs_service.DataMigration
{
    public interface INotificationImportHelper
    {
        Task CreateTableIfNotExists();
        string InsertImportedNotificationQuery { get; }
        string BulkInsertImportedNotificationsQuery { get; }
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

        private readonly string _appDb;

        private const string InsertImportedNotificationTemplate = @"
            INSERT INTO {0} (LegacyId, ImportedAt)
            VALUES (@LegacyId, @ImportedAt);
        ";

        /// <summary>
        /// This query does something not done elsewhere in the app, by referencing tables from both the app db and
        /// the migration db. As such, it relies on the fact that the two databases are hosted in such a way that
        /// cross-querying is possible. Therefore, it won't work in dev.
        /// It also means that we need to feed it the app db name which, of course, differs by environment. This
        /// complexity is typically handled by the EF context, so let's try to lean on that approach as far as
        /// possible:      
        /// </summary>
        private string BulkInsertImportedNotificationsQueryTemplate() => $@" 
            WITH ntbsNotifications AS (
	            SELECT ETSID AS LegacyId
	            FROM {_appDb}..Notification 
	            WHERE ETSID IS NOT NULL
	            UNION
	            SELECT DISTINCT LTBRID AS LegacyId
	            FROM {_appDb}..Notification 
	            WHERE LTBRID IS NOT NULL
            )
            INSERT INTO {0}
	            SELECT LegacyId, GETDATE() FROM ntbsNotifications
	            WHERE NOT EXISTS (SELECT 1 FROM {0} where {0}.LegacyId = ntbsNotifications.LegacyId)
        ";

        private const string SelectImportedNotificationByIdTemplate = @"
            SELECT *
            FROM {0} impNtfc
            WHERE impNtfc.LegacyId = n.PrimaryNotificationId";

        public NotificationImportHelper(IConfiguration configuration, IOptions<MigrationConfig> migrationConfig, NtbsContext ntbsContext)
        {
            connectionString = configuration.GetConnectionString("migration");
            var tablePrefix = migrationConfig.Value.TablePrefix;
            importedNotificationsTableName = $"{tablePrefix}ImportedNotifications";
            
            // The BulkInsertImportedNotificationsQueryTemplate query needs the name of the app database
            // - see its doc for more info
            _appDb = ntbsContext.Database.GetDbConnection().Database;
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

        public string InsertImportedNotificationQuery =>
            string.Format(InsertImportedNotificationTemplate, importedNotificationsTableName);

        public string BulkInsertImportedNotificationsQuery =>
            string.Format(BulkInsertImportedNotificationsQueryTemplate(), importedNotificationsTableName);

        public string SelectImportedNotificationByIdQuery =>
            string.Format(SelectImportedNotificationByIdTemplate, importedNotificationsTableName);
    }
}
