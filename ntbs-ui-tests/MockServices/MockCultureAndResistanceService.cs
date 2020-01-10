using System;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using ntbs_ui_tests.Helpers;

namespace ntbs_ui_tests.MockService
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