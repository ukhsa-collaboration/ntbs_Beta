using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ICultureAndResistanceService
    {
        Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId);
        Task MigrateNotificationCultureResistanceSummary(List<Notification> notifications);
    }

    public class CultureAndResistanceService : ICultureAndResistanceService
    {
        private readonly string _connectionString;
        private readonly string _getCultureAndResistanceDetailsSqlFunction = @"
            SELECT NotificationId,
                CulturePositive,
                Species,
                EarliestSpecimenDate,
                DrugResistanceProfile,
                Isoniazid,
                Rifampicin,
                Pyrazinamide,
                Ethambutol,
                Aminoglycoside,
                Quinolone,
                MDR,
                XDR
            FROM [dbo].[ufnGetNotificationCultureAndResistanceSummary] (@notificationId)";


        public CultureAndResistanceService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString(Constants.DbConnectionStringSpecimenMatching);
        }

        public async Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var rawResult = await connection.QueryAsync<CultureAndResistance>(
                    _getCultureAndResistanceDetailsSqlFunction,
                    new { notificationId });
                return rawResult.FirstOrDefault();
            }
        }

        public async Task MigrateNotificationCultureResistanceSummary(List<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                var ntbsNotificationId = notification.NotificationId;
                if (!string.IsNullOrWhiteSpace(notification.ETSID)
                    && int.TryParse(notification.ETSID, out var etsNotificationId))
                {
                    await MigrateNotificationCultureResistanceSummary(etsNotificationId, ntbsNotificationId);
                }
            }
        }

        private async Task MigrateNotificationCultureResistanceSummary(int etsNotificationId, int ntbsNotificationId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(
                    @"EXEC [dbo].[uspMigrateNotificationCultureResistanceSummary] @etsNotificationId, @ntbsNotificationId;",
                    new { etsNotificationId, ntbsNotificationId });
            }
        }
    }
}
