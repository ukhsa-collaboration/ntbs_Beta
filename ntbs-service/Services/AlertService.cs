using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAlertService
    {
        Task<bool> AddUniqueAlertAsync(Alert alert);
        Task<bool> AddUniqueOpenAlertAsync(Alert alert);
        Task DismissAlertAsync(int alertId, string userId);
        Task DismissMatchingAlertAsync(int notificationId, AlertType alertType);
        Task<IList<Alert>> GetAlertsForNotificationAsync(int notificationId, ClaimsPrincipal user);
    }

    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IAuthorizationService _authorizationService;

        public AlertService(
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository,
            IAuthorizationService authorizationService)
        {
            _alertRepository = alertRepository;
            _notificationRepository = notificationRepository;
            _authorizationService = authorizationService;
        }

        public async Task DismissAlertAsync(int alertId, string userId)
        {
            var alert = await _alertRepository.GetAlertByIdAsync(alertId);

            alert.ClosingUserId = userId;
            alert.ClosureDate = DateTime.Now;
            alert.AlertStatus = AlertStatus.Closed;

            await _alertRepository.UpdateAlertAsync();
        }

        public async Task<bool> AddUniqueAlertAsync(Alert alert)
        {
            var matchingAlert = await _alertRepository.GetAlertByNotificationIdAndTypeAsync(alert.NotificationId, alert.AlertType);
            if ((matchingAlert != null && (matchingAlert != null ? matchingAlert?.AlertType != AlertType.TransferRequest : true)) 
                || (matchingAlert.AlertType == AlertType.TransferRequest && matchingAlert.AlertStatus == AlertStatus.Open))
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

        public async Task<bool> AddUniqueOpenAlertAsync(Alert alert)
        {
            var matchingAlert = await _alertRepository.GetOpenAlertByNotificationIdAndTypeAsync(alert.NotificationId, alert.AlertType);
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

        public async Task<IList<Alert>> GetAlertsForNotificationAsync(int notificationId, ClaimsPrincipal user)
        {
            var alerts = await _alertRepository.GetAlertsForNotificationAsync(notificationId);
            var filteredAlerts = await _authorizationService.FilterTransferAlertsFromListOfAlertsByUserAsync(user, alerts);

            return filteredAlerts;
        }
    }
}
