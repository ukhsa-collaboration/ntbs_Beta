using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IAlertRepository
    {
        Task<Alert> GetAlertByIdAsync(int? alertId);
        Task<Alert> GetAlertByNotificationIdAndTypeAsync(int? alertId, AlertType alertType);
        Task<Alert> GetOpenAlertByNotificationIdAndTypeAsync(int? notificationId, AlertType alertType);
        Task<IList<Alert>> GetAlertsForNotificationAsync(int notificationId);
        Task<IList<Alert>> GetAlertsByTbServiceCodesAsync(IEnumerable<string> tbServices);
        Task<IList<UnmatchedLabResultAlert>> GetAllOpenUnmatchedLabResultAlertsAsync();
        Task AddAlertAsync(Alert alert);
        Task AddAlertRangeAsync(IEnumerable<Alert> alerts);

        Task CloseAlertsByIdsAsync(IEnumerable<int> toBeClosedAlertIds);
        Task CloseUnmatchedLabResultAlertsForSpecimenIdAsync(string specimenId);
        Task SaveAlertChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited);
    }

    public class AlertRepository : IAlertRepository
    {
        private readonly NtbsContext _context;

        public AlertRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task<Alert> GetAlertByIdAsync(int? alertId)
        {
            return await GetBaseAlertIQueryable()
                .SingleOrDefaultAsync(m => m.AlertId == alertId);
        }

        public async Task<Alert> GetAlertByNotificationIdAndTypeAsync(int? notificationId, AlertType alertType)
        {
            return await _context.Alert
                .SingleOrDefaultAsync(m => m.NotificationId == notificationId && m.AlertType == alertType);
        }

        public async Task<Alert> GetOpenAlertByNotificationIdAndTypeAsync(int? notificationId, AlertType alertType)
        {
            return await GetBaseAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .Where(a => a.AlertType == alertType)
                .Where(a => a.AlertStatus == AlertStatus.Open)
                .SingleOrDefaultAsync();
        }

        public async Task<IList<Alert>> GetAlertsByTbServiceCodesAsync(IEnumerable<string> tbServices)
        {
            return await GetBaseAlertIQueryable()
                .Where(a => tbServices.Contains(a.TbServiceCode))
                .Where(a => a.NotificationId == null || a.Notification.NotificationStatus != NotificationStatus.Draft || a.AlertType == AlertType.DataQualityDraft)
                .OrderByDescending(a => a.CreationDate)
                .ToListAsync();
        }

        public async Task<IList<UnmatchedLabResultAlert>> GetAllOpenUnmatchedLabResultAlertsAsync()
        {
            return await GetBaseAlertIQueryable()
                .OfType<UnmatchedLabResultAlert>()
                .ToListAsync();
        }

        public async Task<IList<Alert>> GetAlertsForNotificationAsync(int notificationId)
        {
            return await GetBaseAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .ToListAsync();
        }

        private IQueryable<Alert> GetBaseAlertIQueryable()
        {
            return _context.Alert
                .Where(n => n.AlertStatus != AlertStatus.Closed)
                .Include(n => n.TbService)
                    .ThenInclude(s => s.PHEC)
                .Include(n => n.CaseManager);
        }


        public async Task AddAlertAsync(Alert alert)
        {
            _context.Alert.Add(alert);
            await SaveAlertChangesAsync();
        }

        public async Task AddAlertRangeAsync(IEnumerable<Alert> alerts)
        {
            _context.Alert.AddRange(alerts);
            await SaveAlertChangesAsync();
        }

        public async Task CloseAlertsByIdsAsync(IEnumerable<int> toBeClosedAlertIds)
        {
            foreach (var alert in toBeClosedAlertIds.Select(alertId => new UnmatchedLabResultAlert {AlertId = alertId}))
            {
                _context.Attach(alert);
                alert.AlertStatus = AlertStatus.Closed;
            }

            await _context.SaveChangesAsync();
        }

        public async Task CloseUnmatchedLabResultAlertsForSpecimenIdAsync(string specimenId)
        {
            var alertIdsToClose = await _context.Alert.OfType<UnmatchedLabResultAlert>()
                .Where(alert => alert.AlertStatus == AlertStatus.Open && alert.SpecimenId == specimenId)
                .Select(alert => alert.AlertId)
                .ToListAsync();

            await CloseAlertsByIdsAsync(alertIdsToClose);
        }

        public async Task SaveAlertChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }
    }
}
