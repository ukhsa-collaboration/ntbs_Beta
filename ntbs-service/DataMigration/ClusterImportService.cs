using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.DataMigration
{
    public interface IClusterImportService
    {
        Task UpdateClusterInformation(List<Notification> notifications);
    }

    public class ClusterImportService : IClusterImportService
    {
        private readonly INotificationClusterRepository _notificationClusterRepository;
        private readonly INotificationRepository _notificationRepository;

        public ClusterImportService(
            INotificationClusterRepository notificationClusterRepository,
            INotificationRepository notificationRepository)
        {
            _notificationClusterRepository = notificationClusterRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task UpdateClusterInformation(List<Notification> notifications)
        {
            foreach (var notification in notifications)
            {
                var ntbsNotificationId = notification.NotificationId;
                if (!string.IsNullOrWhiteSpace(notification.ETSID)
                    && int.TryParse(notification.ETSID, out var etsNotificationId))
                {
                    var clusterData = await _notificationClusterRepository.GetNotificationClusterValue(etsNotificationId);
                    await UpdateClusterInformation(notification, clusterData);
                    await _notificationClusterRepository.SetNotificationClusterValue(etsNotificationId, ntbsNotificationId);
                }
            }
        }

        private async Task UpdateClusterInformation(Notification notification, NotificationClusterValue clusterData)
        {
            if (clusterData != null)
            {
                notification.ClusterId = clusterData.ClusterId;
                await _notificationRepository.SaveChangesAsync(
                    NotificationAuditType.SystemEdited,
                    AuditService.AuditUserSystem);
            }
        }
    }
}
