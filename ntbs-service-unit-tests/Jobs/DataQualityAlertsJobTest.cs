using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Jobs;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Jobs
{
    public class DataQualityAlertsJobTest
    {
        private readonly Mock<IAlertService> _mockAlertService;
        private readonly Mock<IDataQualityRepository> _mockDataQualityRepository;

        private readonly DataQualityAlertsJob _dataQualityAlertsJob;

        public DataQualityAlertsJobTest()
        {
            _mockAlertService = new Mock<IAlertService>();
            _mockDataQualityRepository = new Mock<IDataQualityRepository>();
            
            _dataQualityAlertsJob = new DataQualityAlertsJob(
                _mockAlertService.Object,
                _mockDataQualityRepository.Object);
        }

        [Fact]
        public async Task OnRun_WithNoEligibleNotifications_DoesNotCallAlertCreationMethod()
        {
            // Arrange
            var emptyNotificationList = Task.FromResult((IList<Notification>)new List<Notification>());
            var emptyDuplicateNotificationIdsList = 
                Task.FromResult((IList<DataQualityRepository.NotificationAndDuplicateIds>)new List<DataQualityRepository.NotificationAndDuplicateIds>());

            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqDraftAlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqBirthCountryAlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqClinicalDatesAlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqClusterAlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqTreatmentOutcome12AlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqTreatmentOutcome24AlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqTreatmentOutcome36AlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDqDotVotAlertsAsync())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationIdsEligibleForDqPotentialDuplicateAlertsAsync())
                .Returns(emptyDuplicateNotificationIdsList);
            
            // Act
            await _dataQualityAlertsJob.Run(JobCancellationToken.Null);

            // Assert
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityDraftAlert>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityBirthCountryAlert>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityClinicalDatesAlert>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityClusterAlert>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityTreatmentOutcome12>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityTreatmentOutcome24>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityTreatmentOutcome36>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityDotVotAlert>()), Times.Never);
            _mockAlertService.Verify(s => s.AddUniquePotentialDuplicateAlertAsync(It.IsAny<DataQualityPotentialDuplicateAlert>()), Times.Never);
        }
    }
}
