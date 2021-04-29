using System.Collections.Generic;
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
                    CulturePositive = "No",
                    Species = "No result",
                    DrugResistanceProfile = "No result",
                    EarliestSpecimenDate = null,
                    Isoniazid = "No result",
                    Rifampicin = "No result",
                    Pyrazinamide = "No result",
                    Ethambutol = "No result",
                    Aminoglycoside = "No result",
                    Quinolone = "No result",
                    MDR = "No result",
                    XDR = "No result"
                });
            }
            return Task.FromResult<CultureAndResistance>(null);
        }

        public Task MigrateNotificationCultureResistanceSummary(List<Notification> notifications)
        {
            return Task.CompletedTask;
        }
    }
}
