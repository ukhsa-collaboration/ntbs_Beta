using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Jobs;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
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
        public async Task OnRun_WithEligibleNotifications_CallsExpectedMethodWithEligibleNotifications()
        {
            // Arrange
            var testNotificationList = Task.FromResult((IList<Notification>)new List<Notification>
            {
                new Notification {NotificationId = 1}
            });

            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityDraftAlerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityBirthCountryAlerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityClinicalDatesAlerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityClusterAlerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts())
                .Returns(testNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDotVotAlerts())
                .Returns(testNotificationList);
            
            // Act
            await _dataQualityAlertsJob.Run(JobCancellationToken.Null);

            // Assert
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityDraftAlert>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityBirthCountryAlert>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityClinicalDatesAlert>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityClusterAlert>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityTreatmentOutcome12>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityTreatmentOutcome24>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityTreatmentOutcome36>()));
            _mockAlertService.Verify(s => s.AddUniqueAlertAsync(It.IsAny<DataQualityDotVotAlert>()));
        }
        
        [Fact]
        public async Task OnRun_WithNoEligibleNotifications_DoesNotCallAlertCreationMethod()
        {
            // Arrange
            var emptyNotificationList = Task.FromResult((IList<Notification>)new List<Notification>());

            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityDraftAlerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityBirthCountryAlerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityClinicalDatesAlerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityClusterAlerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityTreatmentOutcome12Alerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityTreatmentOutcome24Alerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDataQualityTreatmentOutcome36Alerts())
                .Returns(emptyNotificationList);
            _mockDataQualityRepository.Setup(r => r.GetNotificationsEligibleForDotVotAlerts())
                .Returns(emptyNotificationList);
            
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
        }
    }
}
