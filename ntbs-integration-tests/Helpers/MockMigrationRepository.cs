using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataMigration;
using ntbs_service.DataMigration.RawModels;

namespace ntbs_integration_tests.Helpers
{
    public class MockMigrationRepository : IMigrationRepository
    {
        public static readonly string MockNhsNumberDuplicate = "9500699141";
        public static readonly string MockNhsNumberDuplicateLegacyId = "1001";
        public static readonly string MockNhsNumberDuplicateLegacySource = "ETS";

        public Task<IEnumerable<MigrationDbNhsNumberMatch>> GetLegacyNotificationNhsNumberMatches(string nhsNumber)
        {
            IEnumerable<MigrationDbNhsNumberMatch> matches;
            if (nhsNumber == MockNhsNumberDuplicate)
            {
                matches = new List<MigrationDbNhsNumberMatch>
                {
                    new MigrationDbNhsNumberMatch {OldNotificationId = MockNhsNumberDuplicateLegacyId, Source = MockNhsNumberDuplicateLegacySource}
                };
            }
            else
            {
                matches = new List<MigrationDbNhsNumberMatch>();
            }
            return Task.FromResult(matches);
        }

        public Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsById(IEnumerable<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsByDate(DateTime rangeStartDate, DateTime endStartDate)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbNotification>> GetNotificationsById(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbSite>> GetNotificationSites(IEnumerable<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbManualTest>> GetManualTestResults(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbSocialContextVenue>> GetSocialContextVenues(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbSocialContextAddress>> GetSocialContextAddresses(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbTransferEvent>> GetTransferEvents(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbOutcomeEvent>> GetOutcomeEvents(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbMBovisAnimal>> GetMigrationMBovisAnimalExposure(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbMBovisKnownCase>> GetMigrationMBovisExposureToKnownCase(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbMBovisOccupation>> GetMigrationMBovisOccupationExposures(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationDbMBovisMilkConsumption>> GetMigrationMBovisUnpasteurisedMilkConsumption(List<string> legacyIds)
        {
            throw new NotImplementedException();
        }

        public Task<MigrationLegacyUser> GetLegacyUserByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MigrationLegacyUserHospital>> GetLegacyUserHospitalsByUsername(string username)
        {
            throw new NotImplementedException();
        }
    }
}
