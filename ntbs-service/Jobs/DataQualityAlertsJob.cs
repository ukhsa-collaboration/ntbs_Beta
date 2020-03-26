using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
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

            var notificationsForClusterAlertsCount = await _dataQualityRepository.GetNotificationsEligibleForDataQualityClusterAlertsCount();
            var x = 0;
            while (x < notificationsForClusterAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForClusterAlerts =
                    await _dataQualityRepository.GetUpTo500NotificationsEligibleForDataQualityClusterAlertsWithOffset(x);
                foreach (var notification in notificationsForClusterAlerts)
                {
                    
                    var alert = new DataQualityClusterAlert {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert));   
                    }
                    
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
            
            var notificationsForTreatmentOutcome12Alerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts();
            foreach (var notification in notificationsForTreatmentOutcome12Alerts)
            {
                var alert = new DataQualityTreatmentOutcome12 {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }

            var notificationsForTreatmentOutcome24Alerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts();
            foreach (var notification in notificationsForTreatmentOutcome24Alerts)
            {
                var alert = new DataQualityTreatmentOutcome24 {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }

            var notificationsForTreatmentOutcome36Alerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts();
            foreach (var notification in notificationsForTreatmentOutcome36Alerts)
            {
                var alert = new DataQualityTreatmentOutcome36 {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }

            var notificationsForDotVotAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDotVotAlerts();
            foreach (var notification in notificationsForDotVotAlerts) 
            {
                var alert = new DataQualityDotVotAlert {NotificationId = notification.NotificationId};
                await _alertService.AddUniqueAlertAsync(alert);
            }

            Log.Information($"Finished data quality alerts job");
        }
    }
}
