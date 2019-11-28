using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AlertServiceTest
    {
        private readonly IAlertService mockAlertService;
        private readonly Mock<IAlertRepository> mockAlertRepository;
        private readonly Mock<INotificationRepository> mockNotificationRepository;

        public AlertServiceTest()
        {
            mockAlertRepository = new Mock<IAlertRepository>();
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockAlertService = new AlertService(mockAlertRepository.Object, mockNotificationRepository.Object);
        }

        [Fact]
        public async Task AddUniqueAlert_AddsIfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            var matchingAlert = Task.FromResult((Alert)new TestAlert() {AlertId = 2});
            var testAlert = new TestAlert() {NotificationId = 2, AlertType = AlertType.TransferRequest};
            mockAlertRepository.Setup(x => x.GetAlertByNotificationIdAndTypeAsync(testAlert.NotificationId, testAlert.AlertType))
                .Returns(matchingAlert);

            // Act
            var result = await mockAlertService.AddUniqueAlertAsync(testAlert);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddUniqueAlert_FailsIfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            mockAlertRepository.Setup(x => x.GetAlertByNotificationIdAndTypeAsync(It.IsAny<int>(), It.IsAny<AlertType>()))
                .Returns(Task.FromResult((Alert)null));
            var testAlert = new TestAlert() {NotificationId = 2, AlertType = AlertType.TransferRequest};

            // Act
            var result = await mockAlertService.AddUniqueAlertAsync(testAlert);

            // Assert
            Assert.True(result);
        }
    }
}
