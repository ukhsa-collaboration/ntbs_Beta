using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
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
        private const int CountPerBatch = 500;

        public DataQualityAlertsJob(
            IAlertService alertService,
            IDataQualityRepository dataQualityRepository)
        {
            _alertService = alertService;
            _dataQualityRepository = dataQualityRepository;
        }
        
        private async Task CreateDraftAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository.GetNotificationsEligibleForDataQualityDraftAlertsCount();
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDataQualityDraftAlerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityDraftAlert {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }
        
        private async Task CreateBirthCountryAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository
                .GetNotificationsEligibleForDataQualityBirthCountryAlertsCount();
            
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDataQualityBirthCountryAlerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityBirthCountryAlert {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }

        private async Task CreateClinicalDatesAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository
                .GetNotificationsEligibleForDataQualityClinicalDatesAlertsCount();

            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDataQualityClinicalDatesAlerts(CountPerBatch, x);

                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityClinicalDatesAlert {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }

        private async Task CreateClusterAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository.GetNotificationsEligibleForDataQualityClusterAlertsCount();
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDataQualityClusterAlerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityClusterAlert {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }
        
        private async Task CreateDotVotAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository.GetNotificationsEligibleForDotVotAlertsCount();
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDotVotAlerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityClusterAlert {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }

        private async Task CreateTreatmentOutcome12MonthAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository
                .GetNotificationsEligibleForDqTreatmentOutcome12AlertsCount();
            
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDqTreatmentOutcome12Alerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityTreatmentOutcome12 {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }

        private async Task CreateTreatmentOutcome24MonthAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository
                .GetNotificationsEligibleForDqTreatmentOutcome24AlertsCount();
            
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDqTreatmentOutcome24Alerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityTreatmentOutcome24 {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }

        private async Task CreateTreatmentOutcome36MonthAlertsInBulk()
        {
            var notificationsForAlertsCount = await _dataQualityRepository
                .GetNotificationsEligibleForDqTreatmentOutcome36AlertsCount();
            
            var x = 0;
            while (x < notificationsForAlertsCount)
            {
                var alertsToAdd = new List<Alert>();
                var notificationsForAlerts =
                    await _dataQualityRepository.GetMultipleNotificationsEligibleForDqTreatmentOutcome36Alerts(CountPerBatch, x);
                foreach (var notification in notificationsForAlerts)
                {
                    
                    var alert = new DataQualityTreatmentOutcome36 {NotificationId = notification.NotificationId};
                    var populatedAlert = await _alertService.CheckIfAlertExistsAndPopulateIfItDoesNot(alert, notification);

                    if (populatedAlert != null)
                    {
                        alertsToAdd.Add(populatedAlert);   
                    }
                }

                await _alertService.AddRangeAlerts(alertsToAdd);
                x += 500;
            }
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Warning($"Starting data quality alerts job");
            await CreateDraftAlertsInBulk();
            await CreateBirthCountryAlertsInBulk();
            await CreateClinicalDatesAlertsInBulk();
            await CreateClusterAlertsInBulk();
            await CreateDotVotAlertsInBulk();
            await CreateTreatmentOutcome12MonthAlertsInBulk();
            await CreateTreatmentOutcome24MonthAlertsInBulk();
            await CreateTreatmentOutcome36MonthAlertsInBulk();
            
            var possibleDuplicateNotificationIds =
                await _dataQualityRepository.GetNotificationIdsEligibleForPotentialDuplicateAlerts();
            foreach (var notification in possibleDuplicateNotificationIds)
            {
                var alert = new DataQualityPotentialDuplicateAlert {NotificationId = notification.NotificationId, DuplicateId = notification.DuplicateId};
                await _alertService.AddUniquePotentialDuplicateAlertAsync(alert);
            }

            Log.Warning($"Finished data quality alerts job");
        }
    }
}
