using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.QueryEntities;

namespace ntbs_service.DataAccess
{
    public interface ITableCountsRepository
    {
        Task<IEnumerable<dynamic>> ExecuteUpdateTableCountsStoredProcedure();
        Task<IEnumerable<TableCounts>> GetRecentTableCounts();
    }

    public class TableCountsRepository : ITableCountsRepository
    {
        private readonly string _reportingConnectionString;

        public TableCountsRepository(IConfiguration configuration)
        {
            _reportingConnectionString = configuration.GetConnectionString(Constants.DbConnectionStringReporting);
        }

        public async Task<IEnumerable<dynamic>> ExecuteUpdateTableCountsStoredProcedure()
        {
            IEnumerable<dynamic> result;

            using (var connection = new SqlConnection(_reportingConnectionString))
            {
                connection.Open();
                result = await connection.QueryAsync(
                    "[dbo].[uspUpdateTableCounts]",
                    commandTimeout: Constants.SqlServerDefaultCommandTimeOut,
                    commandType: System.Data.CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<IEnumerable<TableCounts>> GetRecentTableCounts()
        {
            const string getRecentTableCountsQuery = @"
                SELECT TOP (2) CountTime,
		            MigrationNotificationsViewCount,
		            MigrationMBovisAnimalExposureViewCount,
		            MigrationMBovisExposureToKnownCaseViewCount,
		            MigrationMBovisOccupationExposuresViewCount,
		            MigrationMBovisUnpasteurisedMilkConsumptionViewCount,
		            MigrationSocialContextAddressViewCount,
		            MigrationSocialContextVenueViewCount,
		            TransfersViewCount,
		            TreatmentOutcomesCount,
		            EtsNotificationsCount,
		            LtbrNotificationsCount,
		            ETS_NotificationCount,
		            LTBR_DiseasePeriodCount,
		            LTBR_PatientEpisodeCount,
		            NotificationClusterMatchCount,
		            NotificationSpecimenMatchCount,
		            EtsSpecimenMatchCount
                FROM [dbo].[TableCounts]
                ORDER BY CountTime DESC";

            using (var connection = new SqlConnection(_reportingConnectionString))
            {
                connection.Open();
                return (await connection.QueryAsync<TableCounts>(getRecentTableCountsQuery));
            }
        }
    }
}
