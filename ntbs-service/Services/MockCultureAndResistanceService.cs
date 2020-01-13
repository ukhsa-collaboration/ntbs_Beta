using System.Threading.Tasks;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public class MockCultureAndResistanceService : ICultureAndResistanceService
    {
        private readonly int NotificationIdWithResults;

        public MockCultureAndResistanceService(int notificationIdWithResults)
        {
            NotificationIdWithResults = notificationIdWithResults;
        }

        public Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId)
        {
            if (notificationId == NotificationIdWithResults)
            {
                return Task.FromResult(new CultureAndResistance
                {
                    NotificationId = NotificationIdWithResults,
                });
            }
            return Task.FromResult<CultureAndResistance>(null);
        }
    }
}
