using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
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

        private delegate Task<int> GetNotificationsEligibleForDqAlertsCount();
        private delegate Task<IList<Notification>> GetMultipleNotificationsEligibleForDataQualityDraftAlerts(
            int countPerBatch, 
            int offset);

        private async Task CreateDraftAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityDraftAlert>(
            _dataQualityRepository.GetNotificationsEligibleForDqDraftAlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqDraftAlertsAsync
        );
        
        private async Task CreateBirthCountryAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityBirthCountryAlert>(
            _dataQualityRepository.GetNotificationsEligibleForDqBirthCountryAlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqBirthCountryAlertsAsync
        );

        private async Task CreateClinicalDatesAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityClinicalDatesAlert>(
            _dataQualityRepository.GetNotificationsEligibleForDqClinicalDatesAlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqClinicalDatesAlertsAsync
        );

        private async Task CreateClusterAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityClusterAlert>(
            _dataQualityRepository.GetNotificationsEligibleForDqClusterAlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqClusterAlertsAsync
        );
        
        private async Task CreateDotVotAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityDotVotAlert>(
            _dataQualityRepository.GetNotificationsEligibleForDqDotVotAlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqDotVotAlertsAsync
        );

        private async Task CreateTreatmentOutcome12MonthAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityTreatmentOutcome12>(
            _dataQualityRepository.GetNotificationsEligibleForDqTreatmentOutcome12AlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqTreatmentOutcome12AlertsAsync
        );
        
        private async Task CreateTreatmentOutcome24MonthAlertsInBulk() => await CreateAlertsInBulkAsync<DataQualityTreatmentOutcome24>(
            _dataQualityRepository.GetNotificationsEligibleForDqTreatmentOutcome24AlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqTreatmentOutcome24AlertsAsync
        );

        private async Task CreateTreatmentOutcome36MonthAlertsInBulk()  => await CreateAlertsInBulkAsync<DataQualityTreatmentOutcome36>(
            _dataQualityRepository.GetNotificationsEligibleForDqTreatmentOutcome36AlertsCountAsync,
            _dataQualityRepository.GetMultipleNotificationsEligibleForDqTreatmentOutcome36AlertsAsync
        );
        
        private async Task CreateAlertsInBulkAsync<T>(
            GetNotificationsEligibleForDqAlertsCount getCount,
            GetMultipleNotificationsEligibleForDataQualityDraftAlerts getNotifications) where T : Alert
        {
            var notificationsForAlertsCount = await getCount();
            var offset = 0;
            while (offset < notificationsForAlertsCount)
            {
                var notificationsForAlerts = await getNotifications(CountPerBatch, offset);
                
                List<Alert> dqAlerts = notificationsForAlerts
                    .Select(n => {
                        T alert = (T)Activator.CreateInstance(typeof(T));
                        alert.NotificationId = n.NotificationId;
                        if (alert.CaseManagerUsername == null && alert.AlertType != AlertType.TransferRequest)
                        {
                            alert.CaseManagerUsername = n?.HospitalDetails?.CaseManagerUsername;
                        }

                        if (alert.TbServiceCode == null)
                        {
                            alert.TbServiceCode = n?.HospitalDetails?.TBServiceCode;
                        }

                        return (Alert) alert;
                    })
                    .ToList();

                await _alertService.AddAlertsRangeAsync(dqAlerts);
                offset += CountPerBatch;
            }
        }
        
        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting data quality alerts job");
            await CreateDraftAlertsInBulk();
            await CreateBirthCountryAlertsInBulk();
            await CreateClinicalDatesAlertsInBulk();
            await CreateClusterAlertsInBulk();
            await CreateDotVotAlertsInBulk();
            await CreateTreatmentOutcome12MonthAlertsInBulk();
            await CreateTreatmentOutcome24MonthAlertsInBulk();
            await CreateTreatmentOutcome36MonthAlertsInBulk();
            
            var possibleDuplicateNotificationIds =
                await _dataQualityRepository.GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync();
            foreach (var notification in possibleDuplicateNotificationIds)
            {
                var alert = new DataQualityPotentialDuplicateAlert
                {
                    NotificationId = notification.NotificationId, 
                    DuplicateId = notification.DuplicateId
                };
                await _alertService.AddUniquePotentialDuplicateAlertAsync(alert);
            }

            Log.Information($"Finished data quality alerts job");
        }
    }
}
