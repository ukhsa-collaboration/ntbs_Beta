using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataMigration.Exceptions;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration
{
    public interface IMigrationRepository
    {
        Task MarkNotificationsAsImportedAsync(ICollection<Notification> notifications);

        /// <returns>Groups of notifications, indexed by group id, or notification id for singletons</returns>
        Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsById(IEnumerable<string> legacyIds);

        /// <returns>Groups of notifications, indexed by group id, or notification id for singletons</returns>
        Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsByDate(DateTime rangeStartDate,
            DateTime endStartDate);

        Task<IEnumerable<dynamic>> GetNotificationsById(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetNotificationSites(IEnumerable<string> legacyIds);
        Task<IEnumerable<dynamic>> GetManualTestResults(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetSocialContextVenues(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetSocialContextAddresses(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetTransferEvents(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetOutcomeEvents(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetMigrationMBovisAnimalExposure(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetMigrationMBovisExposureToKnownCase(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetMigrationMBovisOccupationExposures(List<string> legacyIds);
        Task<IEnumerable<dynamic>> GetMigrationMBovisUnpasteurisedMilkConsumption(List<string> legacyIds);

        Task<IEnumerable<(string LegacyId, string ReferenceLaboratoryNumber)>> GetReferenceLaboratoryMatches(
            IEnumerable<string> legacyIds);
    }

    public class MigrationRepository : IMigrationRepository
    {
        const string NotificationIdsWithGroupIdsByIdQuery =
            @"SELECT n.OldNotificationId, n.GroupId 
            FROM MigrationNotificationsView n
            WHERE n.OldNotificationId IN @Ids OR n.GroupId IN @Ids";

        const string NotificationsIdsWithGroupIdsByDateQuery =
            @"SELECT n.OldNotificationId, n.GroupId 
            FROM MigrationNotificationsView n
            WHERE n.NotificationDate >= @StartDate AND n.NotificationDate < @EndDate";

        const string NotificationsByIdQuery = @"
            SELECT *
            FROM MigrationNotificationsView n
            WHERE n.OldNotificationId IN @Ids";

        const string NotificationSitesQuery = @"
            SELECT *
            FROM NotificationSite
            WHERE OldNotificationId IN @Ids
        ";

        const string ManualTestResultsQuery = @"
            SELECT *
            FROM ManualTestResults
            WHERE OldNotificationId IN @Ids
        ";

        const string SocialContextVenuesQuery = @"
            SELECT *
            FROM MigrationSocialContextVenueView
            WHERE OldNotificationId IN @Ids
        ";

        const string SocialContextAddressesQuery = @"
            SELECT *
            FROM MigrationSocialContextAddressView
            WHERE OldNotificationId IN @Ids
        ";

        const string TransferEventsQuery = @"
            SELECT *
            FROM MigrationTransferEventsView
            WHERE OldNotificationId IN @Ids
        ";

        const string OutcomeEventsQuery = @"
            SELECT *
            FROM MigrationTreatmentOutcomeEventsView
            WHERE OldNotificationId IN @Ids
        ";

        const string MigrationMBovisAnimalExposureQuery = @"
            SELECT *
            FROM MigrationMBovisAnimalExposureView
            WHERE OldNotificationId IN @Ids
        ";

        const string MigrationMBovisExposureToKnownCaseQuery = @"
            SELECT *
            FROM MigrationMBovisExposureToKnownCaseView
            WHERE OldNotificationId IN @Ids
        ";

        const string MigrationMBovisOccupationExposuresQuery = @"
            SELECT *
            FROM MigrationMBovisOccupationExposuresView
            WHERE OldNotificationId IN @Ids
        ";

        const string MigrationMBovisUnpasteurisedMilkConsumptionQuery = @"
            SELECT *
            FROM MigrationMBovisUnpasteurisedMilkConsumptionView
            WHERE OldNotificationId IN @Ids
        ";


        const string ReferenceLaboratoryMatchesQuery = @"
            SELECT LegacyId, ReferenceLaboratoryNumber
            FROM EtsLaboratoryResultsView
            WHERE LegacyId IN @Ids
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

        public async Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsById(
            IEnumerable<string> legacyIds)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return (await connection.QueryAsync<(string notificationId, string groupId)>(
                        NotificationIdsWithGroupIdsByIdQuery,
                        new {Ids = legacyIds}))
                    .GroupBy(t => t.groupId ?? t.notificationId, t => t.notificationId);
            }
        }

        public async Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsByDate(
            DateTime rangeStartDate,
            DateTime endStartDate)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return (await connection.QueryAsync<(string notificationId, string groupId)>(
                        NotificationsIdsWithGroupIdsByDateQuery,
                        new {StartDate = rangeStartDate.ToString("s"), EndDate = endStartDate.ToString("s")}))
                    .GroupBy(t => t.groupId ?? t.notificationId, t => t.notificationId);
            }
        }

        public async Task<IEnumerable<dynamic>> GetNotificationsById(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(NotificationsByIdQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetNotificationSites(IEnumerable<string> legacyIds)
        {
            return await ExecuteByIdQuery(NotificationSitesQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetManualTestResults(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(ManualTestResultsQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetSocialContextVenues(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(SocialContextVenuesQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetSocialContextAddresses(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(SocialContextAddressesQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetTransferEvents(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(TransferEventsQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetOutcomeEvents(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(OutcomeEventsQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetMigrationMBovisAnimalExposure(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(MigrationMBovisAnimalExposureQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetMigrationMBovisExposureToKnownCase(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(MigrationMBovisExposureToKnownCaseQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetMigrationMBovisOccupationExposures(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(MigrationMBovisOccupationExposuresQuery, legacyIds);
        }

        public async Task<IEnumerable<dynamic>> GetMigrationMBovisUnpasteurisedMilkConsumption(List<string> legacyIds)
        {
            return await ExecuteByIdQuery(MigrationMBovisUnpasteurisedMilkConsumptionQuery, legacyIds);
        }

        public async Task<IEnumerable<(string LegacyId, string ReferenceLaboratoryNumber)>>
            GetReferenceLaboratoryMatches(IEnumerable<string> legacyIds)
        {
            // The table we're referencing here has legacyIds stored as INTs (since they are all ETS ids)
            // Therefore we need to convert to and from strings
            var intIds = legacyIds
                .Select(id => int.TryParse(id, out var intId) ? intId : (int?)null)
                .Where(id => id.HasValue)
                .Select((id => id.Value));
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return (await connection.QueryAsync<(int LegacyId, string ReferenceLaboratoryNumber)>(
                        ReferenceLaboratoryMatchesQuery,
                        new {Ids = intIds}))
                    .Select(tuple =>
                    {
                        string legacyId = tuple.LegacyId.ToString();
                        return (LegacyId: legacyId, tuple.ReferenceLaboratoryNumber);
                    });
            }
        }

        private async Task<IEnumerable<dynamic>> ExecuteByIdQuery(string query, IEnumerable<string> legacyIds)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return await connection.QueryAsync(query, new {Ids = legacyIds});
            }
        }
    }
}
