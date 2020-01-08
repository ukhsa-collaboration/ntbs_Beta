using System;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_integration_tests.MockService
{
    public class MockCultureAndResistanceService : ICultureAndResistanceService
    {
        private readonly CultureAndResistance MockCultureAndResistance = new CultureAndResistance
        {
            NotificationId = Utilities.NOTIFIED_ID,
        };

        public Task<CultureAndResistance> GetCultureAndResistanceDetailsAsync(int notificationId)
        {
            if (notificationId == MockCultureAndResistance.NotificationId)
            {
                return Task.FromResult(MockCultureAndResistance);
            }
            return Task.FromResult<CultureAndResistance>(null);
        }
    }
}