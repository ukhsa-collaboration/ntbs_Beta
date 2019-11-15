using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IAlertRepository
    {
        Task<Alert> GetAlertByIdAsync(int? alertId);
        Task<Alert> GetAlertByNotificationIdAndTypeAsync(int? alertId, AlertType alertType);
        Task AddExampleTbServiceAlertAsync(ExampleTbServiceAlert alert);
    }

    public class AlertRepository : IAlertRepository
    {
        private readonly NtbsContext _context;

        public AlertRepository(NtbsContext context)
        {
            this._context = context;
        }

        public async Task AddExampleTbServiceAlertAsync(ExampleTbServiceAlert alert)
        {
            _context.Alert.Add(alert);
            await _context.SaveChangesAsync();
        }

        public async Task<Alert> GetAlertByIdAsync(int? alertId)
        {
            return await _context.Alert
                .FirstOrDefaultAsync(m => m.AlertId == alertId);
        }

        public async Task<Alert> GetAlertByNotificationIdAndTypeAsync(int? notificationId, AlertType alertType)
        {
            return await _context.Alert
                .FirstOrDefaultAsync(m => m.NotificationId == notificationId && m.AlertType == alertType);
        }
        
    }
}