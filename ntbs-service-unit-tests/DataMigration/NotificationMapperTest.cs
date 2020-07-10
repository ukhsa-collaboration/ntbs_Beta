using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    // This test suite attempts to test the logic of the migration mapper. The mapper is responsible for mapping the
    // DTO objects obtained through dapper from the migration db, to the entities used by the app.
    //
    // This mapping is non-trivial, due to:
    //   - the size of the objects, with ~200 fields involved
    //   - some migration logic happening at this stage e.g.
    //        - looking up hospital ids to TB services based on NTBS reference data
    //        - providing fallback values
    //
    // This test was added quite late into the migration development process. Rather than trying to recreate every
    // single edge case, it aims to provide an example of how to add more regression cases as bugs are dealt with. 
    public class NotificationMapperTest
    {
        private readonly NotificationMapper _notificationMapper;
        private readonly Mock<IMigrationRepository> _migrationRepositoryMock = new Mock<IMigrationRepository>();

        private readonly Mock<IReferenceDataRepository> _referenceDataRepositoryMock =
            new Mock<IReferenceDataRepository>();

        private Dictionary<Guid, TBService> _hospitalToTbServiceCodeDict;

        public NotificationMapperTest()
        {
            _notificationMapper = new NotificationMapper(
                _migrationRepositoryMock.Object,
                _referenceDataRepositoryMock.Object,
                new ImportLogger());


            _referenceDataRepositoryMock.Setup(repo => repo.GetTbServiceFromHospitalIdAsync(It.IsAny<Guid>()))
                .Returns((Guid guid) => Task.FromResult(_hospitalToTbServiceCodeDict[guid]));
        }

        // The data for this test is mainly sourced from the test data created for NTBS during the alpha phase
        // so there is no real patient data here
        // Bits were "stolen" from different notifications and adapted though,
        // so together it might not paint a "cohesive" picture.
        [Fact]
        public async Task correctlyCreates_basicNotification()
        {
            // Arrange
            var legacyIds = new List<string> {"130331"};
            SetupSingleNotificationInGroup(legacyIds, ("130331", "1"));
            SetupMigrationRepoMockWithCsvSourcedData(legacyIds);

            const string royalBerkshireCode = "TBS001";
            const string bristolRoyalCode = "TBS002";
            const string westonGeneralCode = "TBS003";
            _hospitalToTbServiceCodeDict = new Dictionary<Guid, TBService>
            {
                {new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A"), new TBService {Code = royalBerkshireCode}},
                {new Guid("F026FDCD-7BAF-4C96-994C-20E436CC8C59"), new TBService {Code = bristolRoyalCode}},
                {new Guid("0AC033AB-9A11-4FA6-AA1A-1FCA71180C2F"), new TBService {Code = westonGeneralCode}}
            };

            // Act
            var notificationGroup = (await _notificationMapper.GetNotificationsGroupedByPatient(null,
                    "test-request-1",
                    legacyIds))
                .ToList();

            // Assert
            var patientNotifications = notificationGroup.Single();
            var notification = patientNotifications.Single();

            Assert.Equal("130331", notification.ETSID);
            Assert.Equal("130331", notification.LegacyId);
            Assert.Equal(new DateTime(2015, 3, 31), notification.NotificationDate?.Date);
            Assert.Equal(NotificationStatus.Notified, notification.NotificationStatus);

            Assert.Equal("RG145UT", notification.PatientDetails.Postcode);
            Assert.Equal(false, notification.PatientDetails.NoFixedAbode);
            Assert.Equal("Winford", notification.PatientDetails.GivenName);
            Assert.Equal("Wongus", notification.PatientDetails.FamilyName);
            Assert.Equal("9815779000", notification.PatientDetails.NhsNumber);
            Assert.Equal(new DateTime(1981, 3, 24), notification.PatientDetails.Dob);
            Assert.Equal(2010, notification.PatientDetails.YearOfUkEntry);

            Assert.Equal(new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A"), notification.HospitalDetails.HospitalId);
            Assert.Equal(royalBerkshireCode, notification.HospitalDetails.TBServiceCode);
            Assert.Equal("Dr McGown", notification.HospitalDetails.Consultant);

            Assert.Equal(HIVTestStatus.HIVStatusKnown, notification.ClinicalDetails.HIVTestState);
            Assert.Equal("Patient did not begin course of treatment under DOT", notification.ClinicalDetails.Notes);
        }

        private void SetupSingleNotificationInGroup(List<string> legacyIds, (string, string) legacyIdAndLegacyGroup)
        {
            _migrationRepositoryMock.Setup(repo => repo.GetGroupedNotificationIdsById(legacyIds))
                .ReturnsAsync(GroupedNotificationIds(legacyIdAndLegacyGroup));
        }

        private static IEnumerable<IGrouping<string, string>> GroupedNotificationIds((string, string) idGroupIdPair)
        {
            List<IGrouping<string, string>> notificationIdGroup =
                new List<(string notificationId, string groupId)> {idGroupIdPair}
                    .GroupBy(
                        t => t.groupId,
                        t => t.notificationId
                    )
                    .ToList();
            return notificationIdGroup;
        }

        private void SetupMigrationRepoMockWithCsvSourcedData(List<string> legacyIds)
        {
            _migrationRepositoryMock.Setup(repo => repo.GetNotificationsById(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbNotification>("notifications", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetNotificationSites(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbSite>("sites", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetManualTestResults(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbManualTest>("manualTestResults", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetSocialContextVenues(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbSocialContextVenue>("socialContextVenues", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetSocialContextAddresses(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbSocialContextAddress>("socialContextAddresses", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetTransferEvents(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbTransferEvent>("transferEvents", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetOutcomeEvents(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbOutcomeEvent>("outcomeEvents", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetMigrationMBovisAnimalExposure(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbMBovisAnimal>("mBovisAnimalExposure", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetMigrationMBovisExposureToKnownCase(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbMBovisKnownCase>("mBovisKnownCase", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetMigrationMBovisOccupationExposures(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbMBovisOccupation>("mBovisOccupationExposure", legacyIds));
            _migrationRepositoryMock.Setup(repo => repo.GetMigrationMBovisUnpasteurisedMilkConsumption(legacyIds))
                .ReturnsAsync(CvsRecords<MigrationDbMBovisMilkConsumption>("mBovisMilkConsumption", legacyIds));
        }

        private static IEnumerable<T> CvsRecords<T>(string file, ICollection<string> legacyIds)
            where T : MigrationDbRecord
        {
            return CsvParser
                .GetRecordsFromCsv<T>($"../../../TestData/MigrationDatabaseMock/{file}.csv")
                .Where(record => legacyIds.Contains(record.OldNotificationId));
        }
    }
}
