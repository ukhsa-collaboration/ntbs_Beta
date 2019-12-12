using System;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAlertService
    {
        Task<bool> AddUniqueAlertAsync(Alert alert);
        Task DismissAlertAsync(int alertId, string userId);
        Task DismissMatchingAlertAsync(int notificationId, AlertType alertType);
    }

    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly INotificationRepository _notificationRepository;

        public AlertService(
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository)
        {
            _alertRepository = alertRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task DismissAlertAsync(int alertId, string userId)
        {
            var alert = await _alertRepository.GetAlertByIdAsync(alertId);

            alert.ClosingUserId = userId;
            alert.ClosureDate = DateTime.Now;
            alert.AlertStatus = AlertStatus.Closed;

            await _alertRepository.UpdateAlertAsync(AuditType.Deleted);
        }

        public async Task<bool> AddUniqueAlertAsync(Alert alert)
        {
            var matchingAlert = await _alertRepository.GetAlertByNotificationIdAndTypeAsync(alert.NotificationId, alert.AlertType);
            if (matchingAlert != null)
            {
                return false;
            }
            if (alert.NotificationId != null)
            {
                var notification = await _notificationRepository.GetNotificationAsync(alert.NotificationId.Value);
                alert.CreationDate = DateTime.Now;
                if (alert.CaseManagerEmail == null)
                {
                    alert.CaseManagerEmail = notification?.Episode?.CaseManagerEmail;
                }
                if (alert.TbServiceCode == null)
                {
                    alert.TbServiceCode = notification?.Episode?.TBServiceCode;
                }
            }
            await _alertRepository.AddAlertAsync(alert);
            return true;
        }

        public async Task DismissMatchingAlertAsync(int notificationId, AlertType alertType)
        {
            var matchingAlert = await _alertRepository.GetAlertByNotificationIdAndTypeAsync(notificationId, alertType);
            if (matchingAlert != null)
            {
                await DismissAlertAsync(matchingAlert.AlertId, "System");
            }
        }
    }
}
