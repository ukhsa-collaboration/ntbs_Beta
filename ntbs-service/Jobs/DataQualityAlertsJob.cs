using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class DataQualityAlertsJob
    {
        private readonly IAlertService _alertService;
        private readonly IDataQualityRepository _dataQualityRepository;

        public DataQualityAlertsJob(
            IAlertService alertService,
            IDataQualityRepository dataQualityRepository)
        {
            _alertService = alertService;
            _dataQualityRepository = dataQualityRepository;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting data quality alerts job");

            var notificationsForDraftAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityDraftAlerts();
            foreach (var notification in notificationsForDraftAlerts)
            {
                var alert = new DataQualityDraftAlert {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }
            
            var notificationsForBirthCountryAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityBirthCountryAlerts();
            foreach (var notification in notificationsForBirthCountryAlerts)
            {
                var alert = new DataQualityBirthCountryAlert {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }
            
            var notificationsForClinicalDatesAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityClinicalDatesAlerts();
            foreach (var notification in notificationsForClinicalDatesAlerts)
            {
                var alert = new DataQualityClinicalDatesAlert {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }
            
            var notificationsForClusterAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityClusterAlerts();
            foreach (var notification in notificationsForClusterAlerts)
            {
                var alert = new DataQualityClusterAlert {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }
            
            Log.Information($"Finished data quality alerts job");
        }
    }
}
