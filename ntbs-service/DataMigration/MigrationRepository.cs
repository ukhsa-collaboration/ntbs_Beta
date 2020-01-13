using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public interface IMigrationRepository
    {
        Task MarkNotificiationsAsImportedAsync(IEnumerable<Notification> notifications);
        Task<IEnumerable<dynamic>> GetNotificationsById(IEnumerable<string> legacyIds);
        Task<IEnumerable<dynamic>> GetNotificationsByDate(DateTime rangeStartDate, DateTime endStartDate);
        Task<IEnumerable<dynamic>> GetNotificationSites(IEnumerable<string> legacyIds);
    }

    public class MigrationRepository : IMigrationRepository
    {
        const string NotificationsQuery = @"
            SELECT *,
            	trvl.Country1 AS travel_Country1,
                trvl.Country2 AS travel_Country2,
                trvl.Country3 AS travel_Country3,
                trvl.TotalNumberOfCountries AS travel_TotalNumberOfCountries,
                trvl.StayLengthInMonths1 AS travel_StayLengthInMonths1,
                trvl.StayLengthInMonths2 AS travel_StayLengthInMonths2,
                trvl.StayLengthInMonths3 AS travel_StayLengthInMonths3,
                vstr.Country1 AS visitor_Country1,
                vstr.Country2 AS visitor_Country2,
                vstr.Country3 AS visitor_Country3,
                vstr.TotalNumberOfCountries AS visitor_TotalNumberOfCountries,
                vstr.StayLengthInMonths1 AS visitor_StayLengthInMonths1,
                vstr.StayLengthInMonths2 AS visitor_StayLengthInMonths2,
                vstr.StayLengthInMonths3 AS visitor_StayLengthInMonths3
            FROM Notifications n 
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.OldNotificationId
            LEFT JOIN Demographics dmg ON dmg.OldNotificationId = n.OldNotificationId
            LEFT JOIN DeathDates dd ON dd.OldNotificationId = n.OldNotificationId
            LEFT JOIN VisitorHistory vstr ON vstr.OldNotificationId = n.OldNotificationId
            LEFT JOIN TravelHistory trvl ON trvl.OldNotificationId = n.OldNotificationId
            LEFT JOIN ClinicalDates clncl ON clncl.OldNotificationId = n.OldNotificationId
            LEFT JOIN Comorbidities cmrbd ON cmrbd.OldNotificationId = n.OldNotificationId
            LEFT JOIN ImmunoSuppression immn ON immn.OldNotificationId = n.OldNotificationId
            WHERE GroupId IN (
                SELECT GroupId
                FROM Notifications n 
                {0}
            )";
        readonly string NotificationsByIdQuery = string.Format(NotificationsQuery, "WHERE n.OldNotificationId IN @Ids OR n.GroupId IN @Ids");
        readonly string NotificationsByDateQuery = string.Format(NotificationsQuery, @"WHERE n.NotificationDate > @StartDate AND n.NotificationDate < @EndDate");

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

        public async Task MarkNotificiationsAsImportedAsync(IEnumerable<Notification> notifications)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var importedAt = DateTime.Now.ToString("s");

                var query = _importHelper.GetInsertImportedNotificationQuery();
                foreach (var notification in notifications)
                {
                    await connection.QueryAsync(query, new { notification.LegacyId, ImportedAt = importedAt });
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
            if (legacyIds.Count() == 0)
            {
                return new List<dynamic>();
            }
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync(NotificationSitesQuery, new { Ids = legacyIds });
            }
        }
    }
}