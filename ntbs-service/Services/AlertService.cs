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
        Task<bool> CreateExampleTbServiceAlertAsync(ExampleTbServiceAlert alert);
        Task DeleteAlertAsync(int alertId, string userId);

    }

    public class AlertService : IAlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly NtbsContext _context;

        public AlertService(
            IAlertRepository alertRepository,
            NtbsContext context)
        {
            this._alertRepository = alertRepository;
            this._context = context;
        }

        public async Task DeleteAlertAsync(int alertId, string userId)
        {
            var alert = await _alertRepository.GetAlertByIdAsync(alertId);
            alert.ClosingUserId = userId;
            alert.ClosureDate = DateTime.Now;

            alert.AlertStatus = AlertStatus.Closed;
            await UpdateDatabaseAsync(AuditType.Deleted);
        }

        public async Task<bool> CreateExampleTbServiceAlertAsync(ExampleTbServiceAlert alert)
        {
            var optionalAlert = await _alertRepository.GetAlertByNotificationIdAndTypeAsync(alert.NotificationId, alert.AlertType);
            if(optionalAlert == null)
            {
                return false;
            }
            await _alertRepository.AddExampleTbServiceAlertAsync(alert);
            return true;
        }

        private async Task UpdateDatabaseAsync(AuditType auditType = AuditType.Edit)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }
    }
}