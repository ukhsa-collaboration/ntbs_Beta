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
        private readonly string _phecWithResults;

        private readonly UnmatchedSpecimen _mockUnmatchedSpecimen = new UnmatchedSpecimen
        {
            ReferenceLaboratoryNumber = "A55B90955",
            SpecimenDate = new DateTime(2011, 1, 1),
            LabSex = "F",
            Species = "M. tuberculosis",
            LabName = "KONNIK Agnieska",
            LabBirthDate = new DateTime(2011, 1, 1),
            SpecimenTypeCode = "Bronchoscopy Sample",
            LabNhsNumber = "123456789",
            LabAddress = "1 Angel Court London",
            LabPostcode = "N1 1AA",
            PotentialMatches = new List<SpecimenPotentialMatch>
            {
                new SpecimenPotentialMatch
                {
                    NotificationId = 1,
                    NtbsNhsNumber = "123456789",
                    NotificationDate = new DateTime(2011, 10, 1),
                    NtbsName = "KONNIK Agnieska",
                    NtbsBirthDate = new DateTime(2011, 1, 1),
                    NtbsSex = "Female",
                    NtbsPostcode = "N1 1AA",
                    NtbsAddress = "1 Angel Court London"
                },
                new SpecimenPotentialMatch
                {
                    NotificationId = 2,
                    NtbsNhsNumber = "123456789",
                    NotificationDate = new DateTime(2011, 10, 1),
                    NtbsName = "KONNIK Agnieska",
                    NtbsBirthDate = new DateTime(2011, 1, 1),
                    NtbsSex = "Male",
                    NtbsPostcode = "N1 1AB",
                    NtbsAddress = "2 Angel Court London"
                }
            }
        };

        public MockSpecimenService(
            int notificationIdWithResults, 
            string tbServiceWithResults, 
            string phecWithResults)
        {
            _notificationIdWithResults = notificationIdWithResults;
            _tbServiceWithResults = tbServiceWithResults;
            _phecWithResults = phecWithResults;
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

        public async Task<IEnumerable<UnmatchedSpecimen>> GetAllUnmatchedSpecimensAsync()
        {
            var specimens = new List<UnmatchedSpecimen> {_mockUnmatchedSpecimen};
            return await Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCodes)
        {
            var specimens = new List<UnmatchedSpecimen>();
            if (tbServiceCodes.Contains(_tbServiceWithResults))
            {
                specimens.Add(_mockUnmatchedSpecimen);
            }
            return await Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForPhecsAsync(
            IEnumerable<string> phecCodes)
        {
            var specimens = new List<UnmatchedSpecimen>();
            if (phecCodes.Contains(_phecWithResults))
            {
                specimens.Add(_mockUnmatchedSpecimen);
            }
            return await Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);

        }

        public Task UnmatchSpecimen(int notificationId, string labReferenceNumber)
        {
            return Task.CompletedTask;
        }
    }
}
