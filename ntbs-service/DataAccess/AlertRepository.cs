using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IAlertRepository
    {
        Task<Alert> GetOpenAlertByIdAsync(int? alertId);
        Task<T> GetAlertByNotificationIdAndTypeAsync<T>(int notificationId) where T : Alert;
        Task<T> GetOpenAlertByNotificationId<T>(int notificationId) where T : Alert;

        Task<DataQualityPotentialDuplicateAlert> GetDuplicateAlertByNotificationIdAndDuplicateId(int notificationId,
            int duplicateId);
        Task<IList<Alert>> GetOpenAlertsForNotificationAsync(int notificationId);
        Task<IList<UnmatchedLabResultAlert>> GetAllOpenUnmatchedLabResultAlertsAsync();
        Task<DataQualityDraftAlert> GetOpenDraftAlertForNotificationAsync(int notificationId);
        Task AddAlertAsync(Alert alert);
        Task AddAlertRangeAsync(IEnumerable<Alert> alerts);

        Task CloseAlertRangeAsync(IEnumerable<Alert> alerts);
        Task CloseUnmatchedLabResultAlertsForSpecimenIdAsync(string specimenId);
        Task SaveAlertChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited);
        IQueryable<Alert> GetBaseOpenAlertIQueryable();
    }

    public class AlertRepository : IAlertRepository
    {
        private readonly NtbsContext _context;

        public AlertRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task<Alert> GetOpenAlertByIdAsync(int? alertId)
        {
            return await GetBaseOpenAlertIQueryable()
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
            return await GetBaseOpenAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .OfType<T>()
                .SingleOrDefaultAsync();
        }
        
        public async Task<DataQualityPotentialDuplicateAlert> GetDuplicateAlertByNotificationIdAndDuplicateId(int notificationId,
            int duplicateId)
        {
            return await _context.Alert
                .OfType<DataQualityPotentialDuplicateAlert>()
                .SingleOrDefaultAsync(
                    m => m.NotificationId == notificationId 
                         && m.DuplicateId == duplicateId);
        }

        public async Task<IList<UnmatchedLabResultAlert>> GetAllOpenUnmatchedLabResultAlertsAsync()
        {
            return await GetBaseOpenAlertIQueryable()
                .OfType<UnmatchedLabResultAlert>()
                .ToListAsync();
        }

        public async Task<DataQualityDraftAlert> GetOpenDraftAlertForNotificationAsync(int notificationId)
        {
            return await _context.Alert
                .Where(n => n.AlertStatus != AlertStatus.Closed)
                .Where(n => n.NotificationId == notificationId)
                .OfType<DataQualityDraftAlert>()
                .SingleOrDefaultAsync();
        }
        
        public async Task<IList<Alert>> GetOpenAlertsForNotificationAsync(int notificationId)
        {
            return await GetBaseOpenAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .ToListAsync();
        }

        public IQueryable<Alert> GetBaseOpenAlertIQueryable()
        {
            return _context.Alert
                .Where(n => n.AlertStatus != AlertStatus.Closed);
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
