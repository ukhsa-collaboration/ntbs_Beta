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
        private readonly Mock<NtbsContext> mockContext;

        public AlertServiceTest()
        {
            mockAlertRepository = new Mock<IAlertRepository>();
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockContext = new Mock<NtbsContext>();
            mockAlertService = new AlertService(mockAlertRepository.Object, mockNotificationRepository.Object);

            mockContext.Setup(context => context.SetValues(It.IsAny<Object>(), It.IsAny<Object>()));
        }

        [Fact]
        public async Task AddUniqueAlert_AddsIfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            var y = new TestAlert() {AlertId = 2};
            mockAlertRepository.Setup(x => x.GetAlertByNotificationIdAndTypeAsync(It.IsAny<int>(), It.IsAny<AlertType>()))
                .Returns(Task.FromResult((Alert)y));

            // Act
            var result = await mockAlertService.AddUniqueAlertAsync(new TestAlert() {NotificationId = 2, AlertType = AlertType.TransferRequest});

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task AddUniqueAlert_FailsIfNoAlertWithSameNotificationIdAndAlertType()
        {
            // Arrange
            mockAlertRepository.Setup(x => x.GetAlertByNotificationIdAndTypeAsync(It.IsAny<int>(), It.IsAny<AlertType>()))
                .Returns(Task.FromResult((Alert)null));

            // Act
            var result = await mockAlertService.AddUniqueAlertAsync(new TestAlert() {NotificationId = 2, AlertType = AlertType.TransferRequest});

            // Assert
            Assert.True(result);
        }
    }
}
