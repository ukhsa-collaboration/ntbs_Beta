using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public class MockSpecimenService : ISpecimenService
    {
        private readonly int NotificationIdWithResults;

        public MockSpecimenService(int notificationIdWithResults)
        {
            NotificationIdWithResults = notificationIdWithResults;
        }

        public Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            IList<Specimen> specimens = new List<Specimen>();
            if (notificationId == NotificationIdWithResults)
            {
                specimens.Add(new Specimen
                {
                    NotificationId = NotificationIdWithResults
                });
            }
            return Task.FromResult((IEnumerable<Specimen>)specimens);
        }
    }
}
