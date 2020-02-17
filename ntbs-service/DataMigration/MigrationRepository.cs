using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using MoreLinq.Extensions;
using ntbs_service.DataMigration.Exceptions;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public interface IMigrationRepository
    {
        Task MarkNotificationsAsImportedAsync(ICollection<Notification> notifications);
        Task<IEnumerable<dynamic>> GetNotificationsById(IEnumerable<string> legacyIds);
        Task<IEnumerable<dynamic>> GetNotificationsByDate(DateTime rangeStartDate, DateTime endStartDate);
        Task<IEnumerable<dynamic>> GetNotificationSites(IEnumerable<string> legacyIds);
    }

    public class MigrationRepository : IMigrationRepository
    {
        const string NotificationsQuery = @"
            SELECT *
            FROM MigrationNotificationsView n
            WHERE GroupId IN (
                SELECT GroupId
                FROM MigrationNotificationsView n 
                {0}
            )";
        readonly string NotificationsByIdQuery = string.Format(NotificationsQuery, "WHERE n.OldNotificationId IN @Ids OR n.GroupId IN @Ids");
        readonly string NotificationsByDateQuery = string.Format(NotificationsQuery, @"WHERE n.NotificationDate >= @StartDate AND n.NotificationDate < @EndDate");

        const string NotificationSitesQuery = @"
            SELECT *
            FROM NotificationSite
            WHERE OldNotificationId IN @Ids
        ";
        private readonly string connectionString;
        private readonly INotificationImportHelper _importHelper;

        public MigrationRepository(IConfiguration _configuration, INotificationImportHelper importHelper)
        {
            connectionString = _configuration.GetConnectionString("migration");
            _importHelper = importHelper;
        }

        public async Task MarkNotificationsAsImportedAsync(ICollection<Notification> notifications)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var importedAt = DateTime.Now.ToString("s");

                    foreach (var notification in notifications)
                    {
                        await connection.ExecuteAsync(
                            _importHelper.InsertImportedNotificationQuery, 
                            new {notification.LegacyId, ImportedAt = importedAt}
                        );
                    }
                }
                catch (Exception exception)
                {
                    throw new MarkingNotificationsAsImportedFailedException(notifications, exception);
                }
            }
        }

        public async Task<IEnumerable<dynamic>> GetNotificationsById(IEnumerable<string> legacyIds)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync(NotificationsByIdQuery, new { Ids = legacyIds });
            }
        }

        public async Task<IEnumerable<dynamic>> GetNotificationsByDate(DateTime rangeStartDate, DateTime endStartDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync(NotificationsByDateQuery,
                                                    new { StartDate = rangeStartDate.ToString("s"), EndDate = endStartDate.ToString("s") });
            }
        }

        public async Task<IEnumerable<dynamic>> GetNotificationSites(IEnumerable<string> legacyIds)
        {
            var sites = new List<dynamic>();

            foreach (var idsBatch in legacyIds.Batch(1000))
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var batchSites = await connection.QueryAsync(NotificationSitesQuery, new {Ids = idsBatch});
                    sites.AddRange(batchSites);
                }
            }
            return sites;
        }
    }
}
