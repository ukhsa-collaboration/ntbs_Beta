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
        Task DeleteAlertAsync(int alertId, string userId);

    }

    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;

        public AlertService(
            IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task DeleteAlertAsync(int alertId, string userId)
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
            await _alertRepository.AddAlertAsync(alert);
            return true;
        }
    }
}