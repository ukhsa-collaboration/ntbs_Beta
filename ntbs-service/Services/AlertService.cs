using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAlertService
    {
        Task<bool> AddUniqueAlertAsync(Alert alert);
        Task DismissAlertAsync(int alertId, string userId);

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
            if(matchingAlert != null)
            {
                return false;
            }
            if(alert.NotificationId != null)
            {
                var notification = await _notificationRepository.GetNotificationAsync(alert.NotificationId);
                if(alert.CaseManagerEmail == null)
                {
                    alert.CaseManagerEmail = notification.Episode?.CaseManagerEmail;
                }
                if(alert.HospitalId == null)
                {
                    alert.HospitalId = notification.Episode?.HospitalId;
                }
            } 
            await _alertRepository.AddAlertAsync(alert);
            return true;
        }
    }
}