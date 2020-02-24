using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public class MockSpecimenService : ISpecimenService
    {
        private readonly int _notificationIdWithResults;
        private readonly string _tbServiceWithResults;
        private readonly string _phecWithResults;

        public static readonly int MockSpecimenNotificationId1 = 10100;
        public static readonly int MockSpecimenNotificationId2 = 10101;
        public static readonly int MockSpecimenNotificationId3 = 10102;
        public static readonly int MockSpecimenNotificationId4 = 10103;

        public static readonly UnmatchedSpecimen MockUnmatchedSpecimenForTbService = new UnmatchedSpecimen
        {
            ReferenceLaboratoryNumber = "A55B90955",
            SpecimenDate = new DateTime(2011, 1, 1),
            LabSex = "F",
            Species = "M. tuberculosis",
            LabName = "KONNIK Agnieska",
            LabBirthDate = new DateTime(2011, 1, 1),
            SpecimenTypeCode = "Bronchoscopy Sample",
            LabNhsNumber = "1234567890",
            LabAddress = "1 Angel Court London",
            LabPostcode = "N1 1AA",
            PotentialMatches = new List<SpecimenPotentialMatch>
            {
                new SpecimenPotentialMatch
                {
                    NotificationId = MockSpecimenNotificationId1,
                    NtbsNhsNumber = "1234567890",
                    NotificationDate = new DateTime(2011, 10, 1),
                    NtbsName = "KONNIK Agnieska",
                    NtbsBirthDate = new DateTime(2011, 1, 1),
                    NtbsSex = "Female",
                    NtbsPostcode = "N1 1AA",
                    NtbsAddress = "1 Angel Court London"
                },
                new SpecimenPotentialMatch
                {
                    NotificationId = MockSpecimenNotificationId2,
                    NtbsNhsNumber = "1234567890",
                    NotificationDate = new DateTime(2011, 10, 1),
                    NtbsName = "KONNIK Agnieska",
                    NtbsBirthDate = new DateTime(2011, 1, 1),
                    NtbsSex = "Male",
                    NtbsPostcode = "N1 1AB",
                    NtbsAddress = "2 Angel Court London"
                }
            }
        };

        public static readonly UnmatchedSpecimen MockUnmatchedSpecimenForPhec = new UnmatchedSpecimen
        {
            ReferenceLaboratoryNumber = "B55B90955",
            SpecimenDate = new DateTime(2011, 1, 1),
            LabSex = "F",
            Species = "M. tuberculosis",
            LabName = "KONNIK Agnieska",
            LabBirthDate = new DateTime(2011, 1, 1),
            SpecimenTypeCode = "Bronchoscopy Sample",
            LabNhsNumber = "1234567890",
            LabAddress = "1 Angel Court London",
            LabPostcode = "N1 1AA",
            PotentialMatches = new List<SpecimenPotentialMatch>
            {
                new SpecimenPotentialMatch
                {
                    NotificationId = MockSpecimenNotificationId3,
                    NtbsNhsNumber = "1234567890",
                    NotificationDate = new DateTime(2011, 10, 1),
                    NtbsName = "KONNIK Agnieska",
                    NtbsBirthDate = new DateTime(2011, 1, 1),
                    NtbsSex = "Female",
                    NtbsPostcode = "N1 1AC",
                    NtbsAddress = "1 Angel Court London"
                },
                new SpecimenPotentialMatch
                {
                    NotificationId = MockSpecimenNotificationId4,
                    NtbsNhsNumber = "1234567890",
                    NotificationDate = new DateTime(2011, 10, 1),
                    NtbsName = "KONNIK Agnieska",
                    NtbsBirthDate = new DateTime(2011, 1, 1),
                    NtbsSex = "Male",
                    NtbsPostcode = "N1 1AD",
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
            var specimens = new List<UnmatchedSpecimen>
            {
                MockUnmatchedSpecimenForTbService, MockUnmatchedSpecimenForPhec
            };
            return await Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForTbServicesAsync(
            IEnumerable<string> tbServiceCodes)
        {
            var specimens = new List<UnmatchedSpecimen>();
            if (tbServiceCodes.Contains(_tbServiceWithResults))
            {
                specimens.Add(MockUnmatchedSpecimenForTbService);
            }

            return await Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);
        }

        public async Task<IEnumerable<UnmatchedSpecimen>> GetUnmatchedSpecimensDetailsForPhecsAsync(
            IEnumerable<string> phecCodes)
        {
            var specimens = new List<UnmatchedSpecimen>();
            if (phecCodes.Contains(_phecWithResults))
            {
                specimens.Add(MockUnmatchedSpecimenForPhec);
            }

            return await Task.FromResult((IEnumerable<UnmatchedSpecimen>)specimens);
        }

        public Task<IEnumerable<SpecimenMatchPairing>> GetAllSpecimenPotentialMatchesAsync()
        {
            return Task.FromResult((IEnumerable<SpecimenMatchPairing>)new List<SpecimenMatchPairing>());
        }

        public Task<bool> UnmatchSpecimenAsync(int notificationId, string labReferenceNumber, string userName)
        {
            return Task.FromResult(true);
        }

        public Task<bool> MatchSpecimenAsync(int notificationId, string labReferenceNumber, string userName)
        {
            return Task.FromResult(true);
        }
    }
}
