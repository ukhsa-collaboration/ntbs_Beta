using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class CaseManagerImportServiceTests : IDisposable
    {
        private const string NOTIFICATION_ID = "11111";
        private const string CASE_MANAGER_USERNAME_1 = "TestUser@nhs.net";
        private const string CASE_MANAGER_USERNAME_2 = "MartinUser@nhs.net";
        private static readonly Guid HOSPITAL_GUID_1 = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A");
        private static readonly Guid HOSPITAL_GUID_2 = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B");

        private readonly NtbsContext _context;
        private readonly CaseManagerImportService _caseManagerImportService;
        private readonly Mock<IMigrationRepository> _migrationRepositoryMock = new Mock<IMigrationRepository>();

        private Dictionary<string, IEnumerable<MigrationDbNotification>> _idToNotificationDict;
        private Dictionary<string, MigrationLegacyUser> _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>();
        private Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> _usernameToLegacyUserHospitalDict = new Dictionary<string, IEnumerable<MigrationLegacyUserHospital>>();

        public CaseManagerImportServiceTests()
        {
            _context = SetupTestContext();
            SetupMockMigrationRepo();
            IUserRepository userRepository = new UserRepository(_context);
            IReferenceDataRepository referenceDataRepository = new ReferenceDataRepository(_context);
            var importLogger = new ImportLogger();
            _caseManagerImportService = new CaseManagerImportService(userRepository, referenceDataRepository,
                _migrationRepositoryMock.Object, importLogger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyNotificationWithCorrectPermissionsDoesNotExistInNtbs_AddsCaseManagerWithTbServices()
        {
            // Arrange
            var notification = GivenLegacyNotificationWithCaseManagerAndTbServiceCode(CASE_MANAGER_USERNAME_1, "TBS00TEST");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS00TEST", HOSPITAL_GUID_1);

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(notification, null, "test-request-1");

            // Assert
            var addedUser = _context.User.SingleOrDefault();
            Assert.NotNull(addedUser);
            Assert.Equal("John", addedUser.GivenName);
            Assert.Equal("Johnston", addedUser.FamilyName);
            Assert.False(addedUser.IsActive);
            Assert.True(addedUser.IsCaseManager);
            Assert.Contains("TBS00TEST", addedUser.CaseManagerTbServices.Select(cmtb => cmtb.TbServiceCode));
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyNotificationWithIncorrectPermissionsDoesNotExistInNtbs_AddsCaseManagerWithNoTbServices()
        {
            // Arrange
            var notification = GivenLegacyNotificationWithCaseManagerAndTbServiceCode(CASE_MANAGER_USERNAME_1, "TBS00TEST");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS11FAKE", HOSPITAL_GUID_1);

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(notification, null, "test-request-1");

            // Assert
            var addedUser = _context.User.SingleOrDefault();
            Assert.NotNull(addedUser);
            Assert.Equal("John", addedUser.GivenName);
            Assert.Equal("Johnston", addedUser.FamilyName);
            Assert.False(addedUser.IsActive);
            Assert.False(addedUser.IsCaseManager);
            Assert.DoesNotContain("TBS00TEST", addedUser.CaseManagerTbServices.Select(cmtb => cmtb.TbServiceCode));
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyNotificationExistsInNtbs_UserNotImportedButNameUpdated()
        {
            // Arrange
            var notification = GivenLegacyNotificationWithCaseManagerAndTbServiceCode(CASE_MANAGER_USERNAME_1, "TBS00TEST");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS99HULL", HOSPITAL_GUID_1);
            await GivenUserExistsInNtbsWithName("Jon", "Jonston");

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(notification, null, "test-request-1");

            // Assert
            var updatedUser = _context.User.Single();
            Assert.NotNull(updatedUser);
            Assert.Equal("John", updatedUser.GivenName);
            Assert.Equal("Johnston", updatedUser.FamilyName);
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyTreatmentEventWithCorrectPermissionsDoesNotExistInNtbs_UserImportedWithTbServices()
        {
            // Arrange
            var notification = GivenLegacyNotificationWithCaseManagerAndTbServiceCode(CASE_MANAGER_USERNAME_1, "TBS00TEST");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "Frank", "Ignored");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_2, "Martin", "Francis");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS00FAKE", HOSPITAL_GUID_1);
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_2, "TBS00TEST", HOSPITAL_GUID_2);
            notification.TreatmentEvents =
                new List<TreatmentEvent> {new TreatmentEvent {CaseManagerUsername = CASE_MANAGER_USERNAME_2, TbServiceCode = "TBS00TEST"}};

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(notification, null, "test-request-1");

            // Assert
            var addedUsers = _context.User.ToList();
            var addedUserFromTreatmentEvent = addedUsers.SingleOrDefault(u => u.Username == CASE_MANAGER_USERNAME_2);
            Assert.NotEmpty(addedUsers);
            Assert.Equal(2, addedUsers.Count);
            Assert.NotNull(addedUserFromTreatmentEvent);
            Assert.Equal("Martin", addedUserFromTreatmentEvent.GivenName);
            Assert.Equal("Francis", addedUserFromTreatmentEvent.FamilyName);
            Assert.False(addedUserFromTreatmentEvent.IsActive);
            Assert.True(addedUserFromTreatmentEvent.IsCaseManager);
            Assert.Contains("TBS00TEST",
                addedUserFromTreatmentEvent.CaseManagerTbServices.Select(cmtb => cmtb.TbServiceCode));
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyTreatmentEventWithIncorrectPermissionsDoesNotExistInNtbs_UserImportedWithNoTbServices()
        {
            // Arrange
            var notification = GivenLegacyNotificationWithCaseManagerAndTbServiceCode(CASE_MANAGER_USERNAME_1, "TBS00TEST");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "Frank", "Ignored");
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_2, "Martin", "Francis");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS00FAKE", HOSPITAL_GUID_1);
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_2, "TBS00WRONG", HOSPITAL_GUID_2);
            notification.TreatmentEvents =
                new List<TreatmentEvent> {new TreatmentEvent {CaseManagerUsername = CASE_MANAGER_USERNAME_2, TbServiceCode = "TBS00TEST"}};

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(notification, null, "test-request-1");

            // Assert
            var addedUsers = _context.User.ToList();
            var addedUserFromTreatmentEvent = addedUsers.SingleOrDefault(u => u.Username == CASE_MANAGER_USERNAME_2);
            Assert.NotEmpty(addedUsers);
            Assert.Equal(2, addedUsers.Count);
            Assert.NotNull(addedUserFromTreatmentEvent);
            Assert.Equal("Martin", addedUserFromTreatmentEvent.GivenName);
            Assert.Equal("Francis", addedUserFromTreatmentEvent.FamilyName);
            Assert.False(addedUserFromTreatmentEvent.IsActive);
            Assert.False(addedUserFromTreatmentEvent.IsCaseManager);
            Assert.DoesNotContain("TBS00TEST",
                addedUserFromTreatmentEvent.CaseManagerTbServices.Select(cmtb => cmtb.TbServiceCode));
        }

        [Fact]
        public async Task WhenNotificationHasNoCaseManager_AddsCaseManagersFromTreatmentEvents()
        {
            // Arrange
            var notification = GivenLegacyNotificationWithNoCaseManager();
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "Pietro", "Peters");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS00FAKE", HOSPITAL_GUID_1);
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_2, "Scarlett", "Violet");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_2, "TBS00RONG", HOSPITAL_GUID_2);
            notification.TreatmentEvents =
                new List<TreatmentEvent>
                {
                    new TreatmentEvent {CaseManagerUsername = CASE_MANAGER_USERNAME_1, TbServiceCode = "TBS00FAKE"},
                    new TreatmentEvent {CaseManagerUsername = CASE_MANAGER_USERNAME_2, TbServiceCode = "TBS00RONG"},
                };

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManagersFromNotificationAndTreatmentEvents(notification, null, "test-request-1");

            // Assert
            var addedUsers = _context.User.ToList();
            Assert.Equal(2, addedUsers.Count);
            Assert.Equal("Pietro", addedUsers.Single(u => u.Username == CASE_MANAGER_USERNAME_1).GivenName);
            Assert.Equal("Peters", addedUsers.Single(u => u.Username == CASE_MANAGER_USERNAME_1).FamilyName);
            Assert.Equal("Scarlett", addedUsers.Single(u => u.Username == CASE_MANAGER_USERNAME_2).GivenName);
            Assert.Equal("Violet", addedUsers.Single(u => u.Username == CASE_MANAGER_USERNAME_2).FamilyName);
        }

        private void SetupMockMigrationRepo()
        {
            _migrationRepositoryMock.Setup(repo => repo.GetLegacyUserByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserDict[username]));
            _migrationRepositoryMock.Setup(repo => repo.GetLegacyUserHospitalsByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserHospitalDict[username]));
            _migrationRepositoryMock.Setup(repo => repo.GetNotificationsById(It.IsAny<List<string>>()))
                .Returns((List<string> ids) => Task.FromResult(_idToNotificationDict[ids[0]]));
        }

        private Notification GivenLegacyNotificationWithCaseManagerAndTbServiceCode(string caseManager, string TbServiceCode)
        {
            _idToNotificationDict = new Dictionary<string, IEnumerable<MigrationDbNotification>>
            {
                {
                    NOTIFICATION_ID,
                    new List<MigrationDbNotification> {new MigrationDbNotification {CaseManager = caseManager}}
                }
            };
            return new Notification
            {
                IsLegacy = true,
                LTBRID = NOTIFICATION_ID,
                HospitalDetails = new HospitalDetails {TBServiceCode = TbServiceCode},
                TreatmentEvents = new List<TreatmentEvent>()
            };
        }

        private Notification GivenLegacyNotificationWithNoCaseManager()
        {
            _idToNotificationDict = new Dictionary<string, IEnumerable<MigrationDbNotification>>
            {
                {
                    NOTIFICATION_ID,
                    new List<MigrationDbNotification> {new MigrationDbNotification()}
                }
            };
            return new Notification
            {
                IsLegacy = true,
                LTBRID = NOTIFICATION_ID,
                HospitalDetails = new HospitalDetails(),
                TreatmentEvents = new List<TreatmentEvent>()
            };
        }

        private void GivenLegacyUserWithName(string username, string givenName, string familyName)
        {
            _usernameToLegacyUserDict.Add(
                username,
                new MigrationLegacyUser
                {
                    Username = username, GivenName = givenName, FamilyName = familyName
                }
            );
        }

        private async Task GivenLegacyUserHasPermissionsForTbServiceInHospital(string username, string tbServiceCode, Guid hospitalGuid)
        {
            _usernameToLegacyUserHospitalDict.Add(
                username,
                new List<MigrationLegacyUserHospital> {new MigrationLegacyUserHospital {HospitalId = hospitalGuid}}
            );
            await _context.Hospital.AddAsync(new Hospital{HospitalId = hospitalGuid, TBService = new TBService{Code = tbServiceCode}});
            await _context.SaveChangesAsync();
        }

        private async Task GivenUserExistsInNtbsWithName(string givenName, string familyName)
        {
            await _context.User.AddAsync(
                new User {GivenName = givenName, FamilyName = familyName, Username = CASE_MANAGER_USERNAME_1, IsActive = true});
            await _context.SaveChangesAsync();
        }

        private NtbsContext SetupTestContext()
        {
            // Generating a unique database name makes sure the database is not shared between tests.
            string dbName = Guid.NewGuid().ToString();
            return new NtbsContext(new DbContextOptionsBuilder<NtbsContext>()
                .UseInMemoryDatabase(dbName)
                .Options
            );
        }
    }
}
