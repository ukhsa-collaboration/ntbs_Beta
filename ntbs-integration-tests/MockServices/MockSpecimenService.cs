
using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_integration_tests.MockService
{
    public class MockSpecimenService : ISpecimenService
    {
        private readonly Specimen mockSpecimen = new Specimen
        {
            NotificationId = Utilities.NOTIFIED_ID,
        };

        public Task<IEnumerable<Specimen>> GetSpecimenDetailsAsync(int notificationId)
        {
            IEnumerable<Specimen> specimens = new List<Specimen>();
            if (notificationId == mockSpecimen.NotificationId)
            {
                specimens = new List<Specimen> { mockSpecimen };
            }
            return Task.FromResult(specimens);
        }
    }
}