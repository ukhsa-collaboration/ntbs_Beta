using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class ClusterImportTest
    {
        private readonly IClusterImportService _clusterImportService;

        private readonly Mock<INotificationClusterRepository> _notificationClusterRepository;
        private readonly Mock<INotificationRepository> _notificationRepository;

        public ClusterImportTest()
        {
            _notificationClusterRepository = new Mock<INotificationClusterRepository>();
            _notificationRepository = new Mock<INotificationRepository>();
            _clusterImportService = new ClusterImportService(
                _notificationClusterRepository.Object,
                _notificationRepository.Object);
        }

        [Fact]
        public async Task NotificationImport_SetsCluster_WhenItExists()
        {
            // Arrange
            const string clusterId = "ABC123";
            const int etsId = 11;
            var notification = new Notification {NotificationId = 400, ETSID = etsId.ToString()};
            SetupClusterIdForEtsId(etsId, clusterId);

            // Act
            var notifications = new List<Notification> {notification};
            await _clusterImportService.UpdateClusterInformation(notifications);

            // Assert
            Assert.Equal(clusterId, notification.ClusterId);
            _notificationRepository.Verify(nr => nr.SaveChangesAsync(NotificationAuditType.SystemEdited, AuditService.AuditUserSystem), Times.Once);
        }

        [Fact]
        public async Task NotificationImport_UpdatesNotificationIdInReportingDatabase_WhenClusterExists()
        {
            // Arrange
            const string clusterId = "ABC123";
            const int etsId = 11;
            var notification = new Notification { NotificationId = 400, ETSID = etsId.ToString() };
            SetupClusterIdForEtsId(etsId, clusterId);

            // Act
            var notifications = new List<Notification> { notification };
            await _clusterImportService.UpdateClusterInformation(notifications);

            // Assert
            _notificationClusterRepository.Verify(ncr => ncr.SetNotificationClusterValue(etsId, notification.NotificationId), Times.Once);
        }

        [Fact]
        public async Task NotificationImport_DoesNotSetCluster_WhenItIsNull()
        {
            // Arrange
            const int etsId = 11;
            var notification = new Notification { NotificationId = 400, ETSID = etsId.ToString() };
            SetupClusterIdForEtsId(etsId, null);

            // Act
            var notifications = new List<Notification> { notification };
            await _clusterImportService.UpdateClusterInformation(notifications);

            // Assert
            Assert.Null(notification.ClusterId);
        }

        [Fact]
        public async Task NotificationImport_DoesNotSetCluster_WhenNoEntryIsFound()
        {
            // Arrange
            const int etsId = 11;
            var notification = new Notification { NotificationId = 400, ETSID = etsId.ToString() };
            SetupNoEntryForEtsId(etsId);

            // Act
            var notifications = new List<Notification> { notification };
            await _clusterImportService.UpdateClusterInformation(notifications);

            // Assert
            Assert.Null(notification.ClusterId);
        }

        [Theory]
        [InlineData("123A")]
        [InlineData(null)]
        public async Task NotificationImport_DoesNotSetCluster_WhenEtsIdIsNotInteger(string etsId)
        {
            // Arrange
            var notification = new Notification { NotificationId = 400, ETSID = etsId };

            // Act
            var notifications = new List<Notification> { notification };
            await _clusterImportService.UpdateClusterInformation(notifications);

            // Assert
            Assert.Null(notification.ClusterId);
        }

        [Fact]
        public async Task ClusterImport_CanProcessMultipleNotificationsAtOnce()
        {
            // Arrange
            const int etsId1 = 11;
            const int etsId2 = 22;
            const int etsId3 = 33;
            const string clusterId = "A888";
            var notification1 = new Notification { NotificationId = 400, ETSID = etsId1.ToString() };
            var notification2 = new Notification { NotificationId = 401, ETSID = etsId2.ToString() };
            var notification3 = new Notification { NotificationId = 402, ETSID = etsId3.ToString() };
            SetupClusterIdForEtsId(etsId1, clusterId);
            SetupClusterIdForEtsId(etsId2, clusterId);
            SetupNoEntryForEtsId(etsId3);

            // Act
            var notifications = new List<Notification> { notification1, notification2, notification3 };
            await _clusterImportService.UpdateClusterInformation(notifications);

            // Assert
            Assert.Equal(clusterId, notification1.ClusterId);
            Assert.Equal(clusterId, notification2.ClusterId);
            Assert.Null(notification3.ClusterId);
            _notificationClusterRepository.Verify(ncr => ncr.SetNotificationClusterValue(etsId1, notification1.NotificationId), Times.Once);
            _notificationClusterRepository.Verify(ncr => ncr.SetNotificationClusterValue(etsId2, notification2.NotificationId), Times.Once);
            _notificationRepository.Verify(nr => nr.SaveChangesAsync(NotificationAuditType.SystemEdited, AuditService.AuditUserSystem), Times.Exactly(2));
        }

        private void SetupClusterIdForEtsId(int etsId, string clusterId)
        {
            _notificationClusterRepository.Setup(ncr => ncr.GetNotificationClusterValue(etsId))
                .Returns(Task.FromResult(new NotificationClusterValue
                {
                    NotificationId = etsId,
                    ClusterId = clusterId
                }));
        }

        private void SetupNoEntryForEtsId(int etsId)
        {
            _notificationClusterRepository.Setup(ncr => ncr.GetNotificationClusterValue(etsId))
                .Returns(Task.FromResult<NotificationClusterValue>(null));
        }
    }
}
