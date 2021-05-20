using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ntbs_service.DataAccess;
using ntbs_service.Properties;

namespace ntbs_service.DataMigration
{
    /// <summary>
    /// The need for this helper arises from the fact that we are using the same migration db for multiple environments.
    /// For the most part, NTBS treats the migration db as a read-only data repository, for the purposes of searching
    /// through, and migrating in legacy notifications. This allows us to have this single migration db.
    /// 
    /// The only exception to this rule is marking which cases have already been migrated in, or "imported".
    /// This is a duplication of info that exists in the ntbs db (since each imported notification also stores its
    /// legacy ids) - we do this so that we can so that we can exclude already imported records from further searches
    /// and import attempts.
    ///
    /// But, as the different environments import data independently, we need a separate store of this information for
    /// each of them. This helper attempts to shield the rest of the codebase from this complexity, by
    /// being the only class that understands it and exposing ready to consume SQL strings.
    /// </summary>
    public interface INotificationImportHelper
    {
        string InsertImportedNotificationQuery { get; }
        string BulkInsertImportedNotificationsQuery { get; }
        string SelectImportedNotificationWhereIdEquals(string idSelector);
    }

    public class NotificationImportHelper : INotificationImportHelper
    {
        private readonly string _ntbsDb;
        private readonly string _ntbsEnvironment;

        public NotificationImportHelper(IOptions<MigrationConfig> migrationConfig, NtbsContext ntbsContext)
        {
            var ntbsEnvironment = migrationConfig.Value.NtbsEnvironment;
            // We only allow alphanumeric values in the environment name, as a basic guard against SQL injection.
            // (In practice it's hard to imagine a scenario where a malicious actor is able to set environment variables
            // for our app, and yet unable to mutate the database directly, however, it's prudent to put this in).
            _ntbsEnvironment = string.Concat(ntbsEnvironment.Where(char.IsLetterOrDigit));

            // The BulkInsertImportedNotificationsQueryTemplate query needs the name of the app
            // database - see its doc for more info
            try
            {
                _ntbsDb = ntbsContext.Database.GetDbConnection().Database;
            }
            catch (InvalidOperationException)
            {
                // In test, we mock out the db, so it's possible to get the db name from the context - nor would calling
                // the methods that rely on it make sense.
                _ntbsDb = "";
            }
        }

        public string InsertImportedNotificationQuery => $@"
            INSERT INTO ImportedNotifications (LegacyId, ImportedAt, NtbsEnvironment)
            VALUES (@LegacyId, @ImportedAt, '{_ntbsEnvironment}');
        ";

        /// <summary>
        /// This query does something not done elsewhere in the app, by referencing tables from both the app db and
        /// the migration db. As such, it relies on the fact that the two databases are hosted in such a way that
        /// cross-querying is possible. Therefore, it won't work in dev.
        /// It also means that we need to feed it the app db name which, of course, differs by environment. We get it
        /// (in the constructor) from the ntbs context, so we are at least as far as possible leaning on the same
        /// mechanism as the rest of the application.
        /// </summary>
        public string BulkInsertImportedNotificationsQuery => $@" 
            WITH ntbsNotifications AS (
	            SELECT ETSID AS LegacyId
	            FROM [{_ntbsDb}]..Notification 
	            WHERE ETSID IS NOT NULL
	            UNION
	            SELECT DISTINCT LTBRID AS LegacyId
	            FROM [{_ntbsDb}]..Notification 
	            WHERE LTBRID IS NOT NULL
            )
            INSERT INTO ImportedNotifications
	            SELECT LegacyId, GETDATE(), '{_ntbsEnvironment}' FROM ntbsNotifications
	            WHERE NOT EXISTS (
                    SELECT 1 FROM ImportedNotifications
                    WHERE ImportedNotifications.LegacyId = ntbsNotifications.LegacyId
                )
        ";

        public string SelectImportedNotificationWhereIdEquals(string idSelector) => $@"
            SELECT *
            FROM ImportedNotifications impNtfc
            WHERE impNtfc.LegacyId = {idSelector}
                AND impNtfc.NtbsEnvironment = '{_ntbsEnvironment}'";
    }
}
