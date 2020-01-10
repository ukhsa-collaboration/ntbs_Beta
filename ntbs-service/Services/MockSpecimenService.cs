using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public class MockSpecimenService : ISpecimenService
    {
        private readonly int _notificationIdWithResults;
        private readonly string _tbServiceWithResults;

        public MockSpecimenService(int notificationIdWithResults, string tbServiceWithResults)
        {
            _notificationIdWithResults = notificationIdWithResults;
            _tbServiceWithResults = tbServiceWithResults;
        }

        public Task<IEnumerable<MatchedSpecimen>> GetMatchedSpecimenDetailsForNotificationAsync(
            int notificationId)
        {
            var specimens = new List<MatchedSpecimen>();
            if (notificationId == _notificationIdWithResults)
            {
                specimens.Add(new MatchedSpecimen {NotificationId = _notificationIdWithResults});
            }

            return Task.FromResult((IEnumerable<MatchedSpecimen>)specimens);
        }

        public Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimenDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCode)
        {
            var specimens = new List<UnmatchedSpecimen>();
            if (tbServiceCode.Contains(_tbServiceWithResults))
            {
                specimens.Add(new UnmatchedSpecimen
                {
                    ReferenceLaboratoryNumber = "H190380285",
                    LaboratoryName = "NMRS-South",
                    SpecimenDate = new DateTime(2019, 12, 25),
                    SpecimenTypeCode = "Bronchial washings",
                    ReferenceLaboratory = "NMRL",
                    Species = "M. tuberculosis",
                    LabNhsNumber = "2055188800",
                    LabBirthDate = new DateTime(2000, 1, 1),
                    LabName = "Nicolas Simpson",
                    LabSex = "M",
                    LabAddress = "Tinsel Street",
                    LabPostcode = "N1 1AA",
                    TbServiceName = "South-London",
                    PotentialMatches = new List<SpecimenPotentialMatch>
                    {
                        new SpecimenPotentialMatch {NotificationId = _notificationIdWithResults}
                    }
                });
            }

            return Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);
        }

        public Task UnmatchSpecimen(int notificationId, string labReferenceNumber)
        {
            return Task.CompletedTask;
        }
    }
}
