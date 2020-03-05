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
        Task<T> GetAlertByNotificationIdAndTypeAsync<T>(int notificationId) where T : Alert;
        Task<T> GetOpenAlertByNotificationId<T>(int notificationId) where T : Alert;
        Task<IList<Alert>> GetAlertsForNotificationAsync(int notificationId);
        Task<IList<Alert>> GetAlertsByTbServiceCodesAsync(IEnumerable<string> tbServices);
        Task<IList<UnmatchedLabResultAlert>> GetAllOpenUnmatchedLabResultAlertsAsync();
        Task AddAlertAsync(Alert alert);
        Task AddAlertRangeAsync(IEnumerable<Alert> alerts);

        Task CloseAlertRangeAsync(IEnumerable<Alert> alerts);
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

        public async Task<T> GetAlertByNotificationIdAndTypeAsync<T>(int notificationId) where T : Alert 
        {
            return await _context.Alert
                .OfType<T>()
                .SingleOrDefaultAsync(m => m.NotificationId == notificationId);
        }

        public async Task<T> GetOpenAlertByNotificationId<T>(int notificationId) where T : Alert
        {
            return await GetBaseAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .OfType<T>()
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

        public async Task CloseAlertRangeAsync(IEnumerable<Alert> alerts)
        {
            foreach (var alert in alerts)
            {
                alert.AlertStatus = AlertStatus.Closed;
            }

            await _context.SaveChangesAsync();
        }

        public async Task CloseUnmatchedLabResultAlertsForSpecimenIdAsync(string specimenId)
        {
            var alertsToClose = await _context.Alert.OfType<UnmatchedLabResultAlert>()
                .Where(alert => alert.AlertStatus == AlertStatus.Open && alert.SpecimenId == specimenId)
                .ToListAsync();

            await CloseAlertRangeAsync(alertsToClose);
        }

        public async Task SaveAlertChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }
    }
}
