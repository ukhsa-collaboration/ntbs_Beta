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
        private readonly MigrationRepositoryStub _migrationRepository = new MigrationRepositoryStub();

        private readonly Mock<IReferenceDataRepository> _referenceDataRepositoryMock =
            new Mock<IReferenceDataRepository>();

        private Dictionary<Guid, TBService> _hospitalToTbServiceCodeDict;

        public NotificationMapperTest()
        {
            _referenceDataRepositoryMock.Setup(repo => repo.GetTbServiceFromHospitalIdAsync(It.IsAny<Guid>()))
                .Returns((Guid guid) => Task.FromResult(_hospitalToTbServiceCodeDict[guid]));
            _referenceDataRepositoryMock.Setup(repo =>
                    repo.GetTreatmentOutcomeForTypeAndSubType(
                        TreatmentOutcomeType.Died,
                        TreatmentOutcomeSubType.Unknown))
                .ReturnsAsync(new TreatmentOutcome
                {
                    TreatmentOutcomeType = TreatmentOutcomeType.Died,
                    TreatmentOutcomeSubType = TreatmentOutcomeSubType.Unknown
                });
            
            // Needs to happen after the mocking, as the constructor uses a method from reference data repo
            _notificationMapper = new NotificationMapper(
                _migrationRepository,
                _referenceDataRepositoryMock.Object,
                new ImportLogger());
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
            SetupNotificationsInGroups(("130331", "1"));
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
            
            Assert.Equal(Status.No, notification.SocialRiskFactors.RiskFactorDrugs.Status);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorDrugs.IsCurrent);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorDrugs.InPastFiveYears);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorDrugs.MoreThanFiveYearsAgo);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorHomelessness.Status);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorHomelessness.IsCurrent);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorHomelessness.InPastFiveYears);
            Assert.Equal(null, notification.SocialRiskFactors.RiskFactorHomelessness.MoreThanFiveYearsAgo);
            Assert.Equal(Status.Yes, notification.SocialRiskFactors.RiskFactorImprisonment.Status);
            Assert.Equal(false, notification.SocialRiskFactors.RiskFactorImprisonment.IsCurrent);
            Assert.Equal(true, notification.SocialRiskFactors.RiskFactorImprisonment.InPastFiveYears);
            Assert.Equal(false, notification.SocialRiskFactors.RiskFactorImprisonment.MoreThanFiveYearsAgo);
        }

        // Data for this has been based on real regression examples, but with care taken to anonymize it
        // This is based on NTBS-1594
        [Fact]
        public async Task correctlyMaps_ContactTracingNumbers()
        {
            // Arrange
            var legacyIds = new List<string> {"235676", "237137", "241256", "242084"};
            SetupNotificationsInGroups(("235676", "2"),
                ("237137", "3"),
                ("241256", "4"),
                ("242084", "5")
            );

            const string salfordRoyalCode = "TBS0193";
            const string leedsGeneralCode = "TBS0106";
            const string frimleyParkCode = "TBS0075";
            const string colchesterGeneralCode = "TBS0049";
            _hospitalToTbServiceCodeDict = new Dictionary<Guid, TBService>
            {
                {new Guid("3ECAC202-C204-4384-B3F9-0D3FF412DC36"), new TBService {Code = salfordRoyalCode}},
                {new Guid("7E9C715D-0248-4D97-8F67-1134FC133588"), new TBService {Code = leedsGeneralCode}},
                {new Guid("44C3608F-231E-4DD7-963C-4492D804E894"), new TBService {Code = frimleyParkCode}},
                {new Guid("0EEE2EC2-1F3E-4175-BE90-85AA33F0686C"), new TBService {Code = colchesterGeneralCode}}
            };
            
            // Act
            var notifications = (await _notificationMapper.GetNotificationsGroupedByPatient(null,
                    "test-request-1",
                    legacyIds))
                .SelectMany(group => group)
                .ToList();

            // Assert
            Assert.Equal(4, notifications.Count);
            
            var notification242084 = notifications.Find(n => n.ETSID == "242084").ContactTracing;
            Assert.Equal(2, notification242084.AdultsIdentified);
            Assert.Equal(2, notification242084.AdultsScreened);
            Assert.Equal(0, notification242084.AdultsActiveTB);
            Assert.Equal(0, notification242084.AdultsLatentTB);
            Assert.Equal(null, notification242084.ChildrenIdentified);
            Assert.Equal(null, notification242084.ChildrenScreened);
            Assert.Equal(null, notification242084.ChildrenActiveTB);
            Assert.Equal(null, notification242084.ChildrenLatentTB);
                
            var notification237137 = notifications.Find(n => n.ETSID == "237137").ContactTracing;
            Assert.Equal(3, notification237137.AdultsIdentified);
            Assert.Equal(3, notification237137.AdultsScreened);
            Assert.Equal(4, notification237137.ChildrenIdentified);
            Assert.Equal(4, notification237137.ChildrenScreened);
            
            var notification235676 = notifications.Find(n => n.ETSID == "235676").ContactTracing;
            Assert.Equal(1, notification235676.AdultsIdentified);
            Assert.Equal(1, notification235676.AdultsScreened);
            Assert.Equal(0, notification235676.AdultsActiveTB);
            Assert.Equal(0, notification235676.AdultsLatentTB);
            Assert.Equal(0, notification235676.AdultsStartedTreatment);
            Assert.Equal(0, notification235676.AdultsFinishedTreatment);
            Assert.Equal(0, notification235676.ChildrenIdentified);
            Assert.Equal(0, notification235676.ChildrenScreened);
            Assert.Equal(0, notification235676.ChildrenActiveTB);
            Assert.Equal(0, notification235676.ChildrenLatentTB);
            Assert.Equal(0, notification235676.ChildrenStartedTreatment);
            Assert.Equal(0, notification235676.ChildrenFinishedTreatment);
            
            var notification241256 = notifications.Find(n => n.ETSID == "241256").ContactTracing;
            Assert.Equal(0, notification241256.AdultsIdentified);
            Assert.Equal(0, notification241256.AdultsScreened);
            Assert.Equal(0, notification241256.AdultsActiveTB);
            Assert.Equal(0, notification241256.AdultsLatentTB);
            Assert.Equal(0, notification241256.AdultsStartedTreatment);
            Assert.Equal(0, notification241256.AdultsFinishedTreatment);
            Assert.Equal(0, notification241256.ChildrenIdentified);
            Assert.Equal(0, notification241256.ChildrenScreened);
            Assert.Equal(0, notification241256.ChildrenActiveTB);
            Assert.Equal(0, notification241256.ChildrenLatentTB);
            Assert.Equal(0, notification241256.ChildrenStartedTreatment);
            Assert.Equal(0, notification241256.ChildrenFinishedTreatment);
        }
        
        // This is based on NTBS-1650
        [Fact]
        public async Task correctlyCreates_PostMortemNotification()
        {
            // Arrange
            var legacyIds = new List<string> {"132465"};
            SetupNotificationsInGroups(("132465", "6"));

            const string colchesterGeneralCode = "TBS0049";
            _hospitalToTbServiceCodeDict = new Dictionary<Guid, TBService>
            {
                {new Guid("0EEE2EC2-1F3E-4175-BE90-85AA33F0686C"), new TBService {Code = colchesterGeneralCode}}
            };
            
            // Act
            var notification = (await _notificationMapper.GetNotificationsGroupedByPatient(null,
                    "test-request-3",
                    legacyIds))
                .SelectMany(group => group)
                .Single();

            // Assert
            Assert.Equal(1, notification.TreatmentEvents.Count);
            Assert.Equal(true, notification.ClinicalDetails.IsPostMortem);
            // For post mortem cases we *only* want to import the single death event so outcomes reporting is correct
            Assert.Collection(notification.TreatmentEvents,
                te => Assert.Equal(TreatmentOutcomeType.Died, te.TreatmentOutcome.TreatmentOutcomeType));
        }

        private void SetupNotificationsInGroups(params (string, string)[] legacyIdAndLegacyGroup)
        {
            var grouped = new List<(string notificationId, string groupId)>(legacyIdAndLegacyGroup)
                .GroupBy(
                    t => t.groupId,
                    t => t.notificationId
                )
                .ToList();
            _migrationRepository.GroupedNotificationsStub = grouped;
        }
        
        private class MigrationRepositoryStub : IMigrationRepository
        {
            public Task<IEnumerable<MigrationDbNotification>> GetNotificationsById(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbNotification>("notifications", legacyIds));

            }

            public Task<IEnumerable<MigrationDbSite>> GetNotificationSites(IEnumerable<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbSite>("sites", legacyIds));
            }

            public Task<IEnumerable<MigrationDbManualTest>> GetManualTestResults(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbManualTest>("manualTestResults", legacyIds));
            }

            public Task<IEnumerable<MigrationDbSocialContextVenue>> GetSocialContextVenues(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbSocialContextVenue>("socialContextVenues", legacyIds));
            }

            public Task<IEnumerable<MigrationDbSocialContextAddress>> GetSocialContextAddresses(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbSocialContextAddress>("socialContextAddresses", legacyIds));
            }

            public Task<IEnumerable<MigrationDbTransferEvent>> GetTransferEvents(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbTransferEvent>("transferEvents", legacyIds));
            }

            public Task<IEnumerable<MigrationDbOutcomeEvent>> GetOutcomeEvents(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbOutcomeEvent>("outcomeEvents", legacyIds));
            }

            public Task<IEnumerable<MigrationDbMBovisAnimal>> GetMigrationMBovisAnimalExposure(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbMBovisAnimal>("mBovisAnimalExposure", legacyIds));
            }

            public Task<IEnumerable<MigrationDbMBovisKnownCase>> GetMigrationMBovisExposureToKnownCase(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbMBovisKnownCase>("mBovisKnownCase", legacyIds));
            }

            public Task<IEnumerable<MigrationDbMBovisOccupation>> GetMigrationMBovisOccupationExposures(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbMBovisOccupation>("mBovisOccupationExposure", legacyIds));
            }

            public Task<IEnumerable<MigrationDbMBovisMilkConsumption>> GetMigrationMBovisUnpasteurisedMilkConsumption(List<string> legacyIds)
            {
                return Task.FromResult(CvsRecords<MigrationDbMBovisMilkConsumption>("mBovisMilkConsumption", legacyIds));
            }

            public Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsById(IEnumerable<string> legacyIds)
            {
                return Task.FromResult(GroupedNotificationsStub);
            }

            public Task<IEnumerable<IGrouping<string, string>>> GetGroupedNotificationIdsByDate(DateTime rangeStartDate, DateTime endStartDate)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<(string LegacyId, string ReferenceLaboratoryNumber)>> GetReferenceLaboratoryMatches(IEnumerable<string> legacyIds)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<IGrouping<string, string>> GroupedNotificationsStub { get; set; }

            private static IEnumerable<T> CvsRecords<T>(string file, IEnumerable<string> legacyIds)
                where T : MigrationDbRecord
            {
                return CsvParser
                    .GetRecordsFromCsv<T>($"../../../TestData/MigrationDatabaseMock/{file}.csv")
                    .Where(record => legacyIds.Contains(record.OldNotificationId));
            }
        }
    }
}
