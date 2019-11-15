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
        Task<Alert> CreateExampleTbServiceAlertAsync(ExampleTbServiceAlert alert);
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
            _alertRepository = alertRepository;
            _context = context;
        }

        public async Task DeleteAlertAsync(int alertId, string userId)
        {
            var alert = await _alertRepository.GetAlertAsync(alertId);
            alert.ClosingUserId = userId;
            alert.ClosureDate = DateTime.Now;

            alert.AlertStatus = AlertStatus.Closed;
            await UpdateDatabaseAsync(AuditType.Deleted);
        }

        public async Task<Alert> CreateExampleTbServiceAlertAsync(ExampleTbServiceAlert alert)
        {
            await _alertRepository.AddExampleTbServiceAlertAsync(alert);
            return alert;
        }

        private async Task UpdateDatabaseAsync(AuditType auditType = AuditType.Edit)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }
    }
}