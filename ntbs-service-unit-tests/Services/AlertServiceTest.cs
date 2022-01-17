using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
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

        public AlertServiceTest()
        {
            _mockAlertRepository = new Mock<IAlertRepository>();
            _mockNotificationRepository = new Mock<INotificationRepository>();
            var mockAuthorizationService = new Mock<IAuthorizationService>();

            _alertService = new AlertService(
                _mockAlertRepository.Object,
                _mockNotificationRepository.Object,
                mockAuthorizationService.Object);
        }

        [Fact]
        public async Task AddUniqueAlert_DoesNotAddAlert_IfAlertWithSameNotificationIdAndAlertTypeExists()
        {
            // Arrange
            var matchingAlert = Task.FromResult(new TestAlert { AlertId = 2 });
            var testAlert = new TestAlert { NotificationId = 2, AlertType = AlertType.TransferRequest };
            _mockAlertRepository.Setup(x =>
                    x.GetAlertByNotificationIdAndTypeAsync<TestAlert>(testAlert.NotificationId.Value))
                .Returns(matchingAlert);

            // Act
            var result = await _alertService.AddUniqueAlertAsync(testAlert);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddUniqueAlert_AddsAlert_IfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            _mockAlertRepository.Setup(x =>
                    x.GetAlertByNotificationIdAndTypeAsync<TestAlert>(It.IsAny<int>()))
                .Returns(Task.FromResult((TestAlert)null));
            var testAlert = new TestAlert { NotificationId = 2, AlertType = AlertType.TransferRequest };

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
                    CaseManagerId = 1,
                    TBServiceCode = "AB0000001"
                }
            });
            _mockNotificationRepository
                .Setup(s => s.GetNotificationForAlertCreationAsync(It.IsAny<int>()))
                .Returns(mockNotification);

            var matchesRequiringAlerts = new List<SpecimenMatchPairing>
            {
                new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "1"}
            };

            var alertsToBeAdded = new List<Alert>();
            _mockAlertRepository
                .Setup(r => r.AddAlertAsync(It.IsAny<Alert>()))
                .Callback<Alert>(param => alertsToBeAdded.Add(param));

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
        }

        [Fact]
        public async Task DismissAllOpenAlerts_DismissesOpenAlerts()
        {
            // Arrange
            const int notificationId = 1;
            var alert1 = new DataQualityTreatmentOutcome12 { AlertId = 101, AlertStatus = AlertStatus.Open };
            var alert2 = new DataQualityClinicalDatesAlert { AlertId = 102, AlertStatus = AlertStatus.Open };
            var alerts = new List<Alert> { alert1, alert2 };

            _mockAlertRepository
                .Setup(r => r.GetAllOpenAlertsByNotificationId(notificationId))
                .Returns(Task.FromResult(alerts));

            // Act
            await _alertService.DismissAllOpenAlertsForNotification(notificationId);

            // Assert
            AssertAlertClosedRecently(alert1);
            AssertAlertClosedRecently(alert2);
            _mockAlertRepository.Verify(r => r.SaveAlertChangesAsync(NotificationAuditType.Edited), Times.Once);
        }

        [Fact]
        public async Task DismissAllOpenAlerts_DismissesDuplicateNotificationAlert()
        {
            // Arrange
            const int notificationId = 1;

            var alert1 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 101,
                AlertStatus = AlertStatus.Open,
            };
            
            _mockAlertRepository
                .Setup(s => s.GetAllOpenAlertsByNotificationId(notificationId))
                .Returns(Task.FromResult(new List<Alert> { alert1 }));

            // Act
            await _alertService.DismissAllOpenAlertsForNotification(notificationId);

            // Assert
            AssertAlertClosedRecently(alert1);

            _mockAlertRepository.Verify(r => r.SaveAlertChangesAsync(NotificationAuditType.Edited), Times.Once);
        }

        [Fact]
        public async Task DismissingDuplicateNotificationAlert_AlsoRemovesAlertOnDuplicateRecord()
        {
            // Arrange

            var alert = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 101,
                AlertStatus = AlertStatus.Open,
                NotificationId = 1,
                DuplicateId = 2,
            };
            
            var duplicateAlert = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 102,
                AlertStatus = AlertStatus.Open,
                NotificationId = 2,
                DuplicateId = 1
            };
            
            _mockAlertRepository
                .Setup(s => s.GetAllOpenAlertsByNotificationId(1))
                .Returns(Task.FromResult(new List<Alert>{alert}));
            
            _mockAlertRepository
                .Setup(s => s.GetAllOpenAlertsByNotificationId(2))
                .Returns(Task.FromResult(new List<Alert>{duplicateAlert}));

            // Act
            await _alertService.DismissAllOpenAlertsForNotification(1);
            
            // Assert
            AssertAlertClosedRecently(alert);
            AssertAlertClosedRecently(duplicateAlert);
        }
        
        [Fact]
        public async Task DismissingDuplicateNotificationAlert_OnlyRemovesRelatedNotification_OnDuplicateRecords()
        {
            // This test looks at the edge case where there are three duplicate notifications
            // pointing to one another in a triangle. Closing one notification should close the duplicate alerts
            // related to it, but the duplicate alerts between the two other notifications should remain
            
            // Arrange
            var alert1To2 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 101,
                AlertStatus = AlertStatus.Open,
                DuplicateId = 2
            };
            
            var alert1To3 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 102,
                AlertStatus = AlertStatus.Open,
                DuplicateId = 3
            };
            
            var alert2To1 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 103,
                AlertStatus = AlertStatus.Open,
                DuplicateId = 1
            };
            
            var alert2To3 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 104,
                AlertStatus = AlertStatus.Open,
                DuplicateId = 3
            };
            
            var alert3To1 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 105,
                AlertStatus = AlertStatus.Open,
                DuplicateId = 1
            };
            
            
            var alert3To2 = new DataQualityPotentialDuplicateAlert
            {
                AlertId = 106,
                AlertStatus = AlertStatus.Open,
                DuplicateId = 2
            };

            _mockAlertRepository
                .Setup(s => s.GetAllOpenAlertsByNotificationId(1))
                .Returns(Task.FromResult(new List<Alert>{alert1To2, alert1To3}));
            
            _mockAlertRepository
                .Setup(s => s.GetAllOpenAlertsByNotificationId(2))
                .Returns(Task.FromResult(new List<Alert>{alert2To1, alert2To3}));
            
            _mockAlertRepository
                .Setup(s => s.GetAllOpenAlertsByNotificationId(3))
                .Returns(Task.FromResult(new List<Alert>{alert3To1, alert3To2}));

            // Act
            await _alertService.DismissAllOpenAlertsForNotification(1);
            
            // Assert
            AssertAlertClosedRecently(alert1To2);
            AssertAlertClosedRecently(alert2To1);
            AssertAlertClosedRecently(alert1To3);
            AssertAlertClosedRecently(alert3To1);
            AssertAlertNotClosed(alert2To3);
            AssertAlertNotClosed(alert3To2);
        }

        [Fact]
        public async Task AddUniqueAlert_CreatesAlertWithCurrentTimestamp()
        {
            // Arrange
            _mockAlertRepository.Setup(x =>
                    x.GetAlertByNotificationIdAndTypeAsync<TestAlert>(It.IsAny<int>()))
                .Returns(Task.FromResult((TestAlert)null));
            var testAlert = new TestAlert { NotificationId = 2, AlertType = AlertType.TransferRequest };

            // Act
            await _alertService.AddUniqueAlertAsync(testAlert);

            // Assert
            Assert.Equal(DateTime.Now, testAlert.CreationDate, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task AddUniqueOpenAlertAsync_CreatesAlertWithCurrentTimestamp()
        {
            // Arrange
            _mockAlertRepository.Setup(x =>
                    x.GetOpenAlertByNotificationId<TestAlert>(It.IsAny<int>()))
                .Returns(Task.FromResult((TestAlert)null));
            var testAlert = new TestAlert { NotificationId = 2, AlertType = AlertType.TransferRequest };

            // Act
            await _alertService.AddUniqueOpenAlertAsync(testAlert);

            // Assert
            Assert.Equal(DateTime.Now, testAlert.CreationDate, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task AddUniquePotentialDuplicateAlertAsync_CreatesAlertWithCurrentTimestamp()
        {
            // Arrange
            _mockAlertRepository.Setup(x =>
                    x.GetDuplicateAlertByNotificationIdAndDuplicateId(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult((DataQualityPotentialDuplicateAlert)null));
            var testAlert = new DataQualityPotentialDuplicateAlert { NotificationId = 2, AlertType = AlertType.DataQualityPotientialDuplicate };

            // Act
            await _alertService.AddUniquePotentialDuplicateAlertAsync(testAlert);

            // Assert
            Assert.Equal(DateTime.Now, testAlert.CreationDate, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public async Task CreateAlertsForUnmatchedLabResults_CreatesAlertWithCurrentTimestamp()
        {
            // Arrange
            _mockNotificationRepository.Setup(x => x.GetNotificationForAlertCreationAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Notification {NotificationId = 1}));
            var pairings = new List<SpecimenMatchPairing>
            {
                new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "A123"}
            };

            // Act
            await _alertService.CreateAlertsForUnmatchedLabResults(pairings);

            // Assert
            _mockAlertRepository.Verify(x => x.AddAlertAsync(
                It.Is<Alert>(alert =>
                    alert.CreationDate < DateTime.Now
                        && alert.CreationDate > DateTime.Now.Subtract(TimeSpan.FromSeconds(1)))));
        }

        [Fact]
        public async Task CreateAlertsForUnmatchedLabResults_DoesNotCreateNewAlertIfClosedAlertAlreadyExistsForSpecimen()
        {
            // Arrange
            const int notificationId = 1;
            const string specimenId = "A123";
            _mockNotificationRepository.Setup(x => x.GetNotificationForAlertCreationAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new Notification { NotificationId = notificationId }));
            _mockAlertRepository.Setup(x =>
                    x.GetUnmatchedLabResultAlertForNotificationAndSpecimenAsync(notificationId, specimenId))
                        .Returns(Task.FromResult(new UnmatchedLabResultAlert
                        {
                            NotificationId = notificationId,
                            SpecimenId = specimenId,
                            AlertStatus = AlertStatus.Closed
                        }));
            var pairings = new List<SpecimenMatchPairing>
            {
                new SpecimenMatchPairing {NotificationId = notificationId, ReferenceLaboratoryNumber = specimenId}
            };

            // Act
            await _alertService.CreateAlertsForUnmatchedLabResults(pairings);

            // Assert
            _mockAlertRepository.Verify(x => x.AddAlertAsync(It.IsAny<Alert>()), Times.Never);
        }

        private void AssertAlertClosedRecently(Alert alert)
        {
            Assert.Equal(AlertStatus.Closed, alert.AlertStatus);
            Assert.NotNull(alert.ClosureDate);
            Assert.Equal(DateTime.Now, alert.ClosureDate.Value, TimeSpan.FromSeconds(30));
        }

        private void AssertAlertNotClosed(Alert alert)
        {
            Assert.Equal(AlertStatus.Open, alert.AlertStatus);
            Assert.Null(alert.ClosureDate);
        }
    }
}
