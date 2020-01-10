
using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using ntbs_ui_tests.Helpers;

namespace ntbs_ui_tests.MockService
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