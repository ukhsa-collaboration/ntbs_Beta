using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service.Jobs
{
    public class DataQualityAlertsJob
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IAlertService _alertService;
        private readonly IDataQualityRepository _dataQualityRepository;

        public DataQualityAlertsJob(
            IAlertRepository alertRepository,
            IAlertService alertService,
            IDataQualityRepository dataQualityRepository)
        {
            _alertRepository = alertRepository;
            _alertService = alertService;
            _dataQualityRepository = dataQualityRepository;
        }

        public async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting data quality alerts job");

            var notificationsForDraftAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityDraftAlerts();
            // do some stuff
            var notificationsForBirthCountryAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityBirthCountryAlerts();
            //do some stuff
            var notificationsForClinicalDatesAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityClinicalDatesAlerts();
            //do some stuff
            var notificationsForClusterAlerts =
                await _dataQualityRepository.GetNotificationsEligibleForDataQualityClusterAlerts();
            //do some stuff
        }
    }
}
