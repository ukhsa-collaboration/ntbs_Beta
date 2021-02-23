using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public class MockNotificationClusterService : INotificationClusterService
    {
        private readonly List<NotificationClusterValue> _notificationClusterValues;

        public MockNotificationClusterService(List<NotificationClusterValue> notificationClusterValues)
        {
            _notificationClusterValues = notificationClusterValues;
        }

        public Task<IEnumerable<NotificationClusterValue>> GetNotificationClusterValues()
        {
            return Task.FromResult(_notificationClusterValues as IEnumerable<NotificationClusterValue>);
        }

        public Task<NotificationClusterValue> GetNotificationClusterValue(int etsNotificationId)
        {
            return Task.FromResult(_notificationClusterValues.SingleOrDefault(ncv => ncv.NotificationId == etsNotificationId));
        }

        public Task SetNotificationClusterValue(int etsNotificationId, int ntbsNotificationId)
        {
            return Task.CompletedTask;
        }
    }
}
