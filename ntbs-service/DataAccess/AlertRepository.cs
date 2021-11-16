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
        Task<UnmatchedLabResultAlert> GetUnmatchedLabResultAlertForNotificationAndSpecimenAsync(int notificationId,
            string specimenId);
        Task<T> GetOpenAlertByNotificationId<T>(int notificationId) where T : Alert;
        Task<List<Alert>> GetAllOpenAlertsByNotificationId(int notificationId);
        Task<TransferAlert> GetOpenTransferAlertByNotificationId(int notificationId);

        Task<DataQualityPotentialDuplicateAlert> GetDuplicateAlertByNotificationIdAndDuplicateId(int notificationId,
            int duplicateId);

        Task<IList<AlertWithTbServiceForDisplay>> GetOpenAlertsForNotificationAsync(int notificationId);
        Task<List<AlertWithTbServiceForDisplay>> GetOpenAlertsByTbServiceCodesAsync(IEnumerable<string> tbServices);
        Task<IList<UnmatchedLabResultAlert>> GetAllOpenUnmatchedLabResultAlertsAsync();
        Task<DataQualityDraftAlert> GetOpenDraftAlertForNotificationAsync(int notificationId);
        Task AddAlertAsync(Alert alert);
        Task AddAlertRangeAsync(IEnumerable<Alert> alerts);

        Task CloseAlertRangeAsync(IEnumerable<Alert> alerts);
        Task CloseUnmatchedLabResultAlertsForSpecimenIdAsync(string specimenId);
        Task CloseUnmatchedLabResultAlertForSpecimenAndNotificationAsync(string specimenId, int notificationId);
        Task SaveAlertChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited);
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

        public async Task<UnmatchedLabResultAlert> GetUnmatchedLabResultAlertForNotificationAndSpecimenAsync(
            int notificationId,
            string specimenId)
        {
            return await _context.Alert
                .OfType<UnmatchedLabResultAlert>()
                .SingleOrDefaultAsync(a => a.NotificationId == notificationId && a.SpecimenId == specimenId);
        }

        public async Task<T> GetOpenAlertByNotificationId<T>(int notificationId) where T : Alert
        {
            return await GetBaseOpenAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .OfType<T>()
                .SingleOrDefaultAsync();
        }

        public async Task<List<Alert>> GetAllOpenAlertsByNotificationId(int notificationId)
        {
            return await GetBaseOpenAlertIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .ToListAsync();
        }

        public async Task<TransferAlert> GetOpenTransferAlertByNotificationId(int notificationId)
        {
            return await GetBaseOpenAlertIQueryable()
                .OfType<TransferAlert>()
                .Include(a => a.TbService.PHEC)
                .Include(a => a.CaseManager)
                .Where(a => a.NotificationId == notificationId)
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

        public async Task<List<AlertWithTbServiceForDisplay>> GetOpenAlertsByTbServiceCodesAsync(IEnumerable<string> tbServices)
        {
            return await GetOpenAlertWithTbServiceForDisplayIQueryable()
                .Where(a => tbServices.Contains(a.TbServiceCode))
                .OrderByDescending(a => a.CreationDate)
                .ToListAsync();
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

        public async Task<IList<AlertWithTbServiceForDisplay>> GetOpenAlertsForNotificationAsync(int notificationId)
        {
            return await GetOpenAlertWithTbServiceForDisplayIQueryable()
                .Where(a => a.NotificationId == notificationId)
                .ToListAsync();
        }

        private IQueryable<Alert> GetBaseOpenAlertIQueryable()
        {
            return _context.Alert
                .Where(n => n.AlertStatus != AlertStatus.Closed);
        }

        private IQueryable<AlertWithTbServiceForDisplay> GetOpenAlertWithTbServiceForDisplayIQueryable()
        {
            return GetBaseOpenAlertIQueryable()
                .Select(alert =>
                    new AlertWithTbServiceForDisplay
                    {
                        AlertId = alert.AlertId,
                        CreationDate = alert.CreationDate,
                        NotificationId = alert.NotificationId,
                        TbServiceCode = alert is TransferAlert
                            ? ((TransferAlert)alert).TbServiceCode
                            : alert.Notification.HospitalDetails.TBServiceCode,
                        TbServiceName = alert is TransferAlert
                            ? ((TransferAlert)alert).TbService.Name
                            : alert.Notification.HospitalDetails.TBService.Name,
                        CaseManagerName = alert is TransferAlert
                            ? ((TransferAlert)alert).CaseManager.DisplayName
                            : alert.Notification.HospitalDetails.CaseManager.DisplayName,
                        AlertType = alert.AlertType,
                        Action = alert.Action,
                        ActionLink = alert.ActionLink,
                        NotDismissable = alert.NotDismissable,
                    }
                ).AsNoTracking();
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

        public async Task CloseUnmatchedLabResultAlertForSpecimenAndNotificationAsync(string specimenId,
            int notificationId)
        {
            var alertToClose = await _context.Alert.OfType<UnmatchedLabResultAlert>()
                .SingleOrDefaultAsync(alert => alert.AlertStatus == AlertStatus.Open
                                      && alert.SpecimenId == specimenId
                                      && alert.NotificationId == notificationId);

            if (alertToClose != null)
            {
                alertToClose.AlertStatus = AlertStatus.Closed;
                await _context.SaveChangesAsync();
            }
        }

        public async Task SaveAlertChangesAsync(NotificationAuditType auditType = NotificationAuditType.Edited)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }
    }
}
