using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AlertServiceTest
    {
        private readonly IAlertService _alertService;
        private readonly Mock<IAlertRepository> _mockAlertRepository;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;

        public AlertServiceTest()
        {
            _mockAlertRepository = new Mock<IAlertRepository>();
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();

            _alertService = new AlertService(
                _mockAlertRepository.Object,
                _mockNotificationRepository.Object,
                _mockAuthorizationService.Object);
        }

        [Fact]
        public async Task AddUniqueAlert_AddsIfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            var matchingAlert = Task.FromResult((Alert)new TestAlert() {AlertId = 2});
            var testAlert = new TestAlert() {NotificationId = 2, AlertType = AlertType.TransferRequest};
            _mockAlertRepository.Setup(x =>
                    x.GetAlertByNotificationIdAndTypeAsync(testAlert.NotificationId, testAlert.AlertType))
                .Returns(matchingAlert);

            // Act
            var result = await _alertService.AddUniqueAlertAsync(testAlert);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddUniqueAlert_FailsIfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            _mockAlertRepository.Setup(x =>
                    x.GetAlertByNotificationIdAndTypeAsync(It.IsAny<int>(), It.IsAny<AlertType>()))
                .Returns(Task.FromResult((Alert)null));
            var testAlert = new TestAlert() {NotificationId = 2, AlertType = AlertType.TransferRequest};

            // Act
            var result = await _alertService.AddUniqueAlertAsync(testAlert);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreateAlertsForUnmatchedLabResults_CallsAddAlertRangeWithExpectedAlertData()
        {
            // Arrange
            var mockNotification = Task.FromResult(new Notification
            {
                NotificationId = 1,
                HospitalDetails = new HospitalDetails
                {
                    CaseManagerUsername = "testUsername@email.com",
                    TBServiceCode = "AB0000001"
                }
            });
            _mockNotificationRepository
                .Setup(s => s.GetNotificationForAlertCreation(It.IsAny<int>()))
                .Returns(mockNotification);

            var matchesRequiringAlerts = new List<SpecimenMatchPairing>
            {
                new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "1"}
            };

            List<Alert> alertsToBeAdded = null;
            _mockAlertRepository
                .Setup(r => r.AddAlertRangeAsync(It.IsAny<IEnumerable<Alert>>()))
                .Callback<IEnumerable<Alert>>(param => alertsToBeAdded = param.ToList());
            
            // Act
            await _alertService.CreateAlertsForUnmatchedLabResults(matchesRequiringAlerts);
            
            // Assert
            Assert.Single(alertsToBeAdded);
            var alert = alertsToBeAdded.First() as UnmatchedLabResultAlert;
            
            Assert.NotNull(alert);
            Assert.Equal(AlertType.UnmatchedLabResult, alert.AlertType);
            Assert.Equal(AlertStatus.Open, alert.AlertStatus);
            Assert.Equal(1, alert.NotificationId);
            Assert.Equal("1", alert.SpecimenId);
            Assert.Equal("testUsername@email.com", alert.CaseManagerUsername);
            Assert.Equal("AB0000001", alert.TbServiceCode);
        }
    }
}
