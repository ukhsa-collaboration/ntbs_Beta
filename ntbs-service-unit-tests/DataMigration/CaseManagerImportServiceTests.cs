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
        private const string CASE_MANAGER_USERNAME = "TestUser@nhs.net";
        private static readonly Guid HOSPITAL_GUID = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A");

        private readonly NtbsContext _context;
        private readonly CaseManagerImportService _caseManagerImportService;
        private readonly Mock<IMigrationRepository> _migrationRepositoryMock = new Mock<IMigrationRepository>();

        private Dictionary<string, IEnumerable<MigrationDbNotification>> _idToNotificationDict;
        private Dictionary<string, MigrationLegacyUser> _usernameToLegacyUserDict;
        private Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> _usernameToLegacyUserHospitalDict;

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
            
            var notification = GivenLegacyNotificationWithTbServiceCode("TBS00TEST");
            GivenLegacyUserWithName("John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbService("TBS00TEST");

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManager(notification, null, "test-request-1");

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
            var notification = GivenLegacyNotificationWithTbServiceCode("TBS00TEST");
            GivenLegacyUserWithName("John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbService("TBS11FAKE");

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManager(notification, null, "test-request-1");

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
            var notification = GivenLegacyNotificationWithTbServiceCode("TBS00TEST");
            GivenLegacyUserWithName("John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbService("TBS99HULL");
            await GivenUserExistsInNtbsWithName("Jon", "Jonston");

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManager(notification, null, "test-request-1");

            // Assert
            var updatedUser = _context.User.Single();
            Assert.NotNull(updatedUser);
            Assert.Equal("John", updatedUser.GivenName);
            Assert.Equal("Johnston", updatedUser.FamilyName);
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

        private Notification GivenLegacyNotificationWithTbServiceCode(string TbServiceCode)
        {
            _idToNotificationDict = new Dictionary<string, IEnumerable<MigrationDbNotification>>
            {
                {
                    NOTIFICATION_ID,
                    new List<MigrationDbNotification> {new MigrationDbNotification {CaseManager = CASE_MANAGER_USERNAME}}
                }
            };
            return new Notification
            {
                IsLegacy = true,
                LTBRID = NOTIFICATION_ID,
                HospitalDetails = new HospitalDetails {TBServiceCode = TbServiceCode}
            };
        }

        private void GivenLegacyUserWithName(string givenName, string familyName)
        {
            _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>
            {
                {
                    CASE_MANAGER_USERNAME,
                    new MigrationLegacyUser {Username = CASE_MANAGER_USERNAME, GivenName = givenName, FamilyName = familyName}
                }
            };
        }

        private async Task GivenLegacyUserHasPermissionsForTbService(string tbServiceCode)
        {
            _usernameToLegacyUserHospitalDict = new Dictionary<string, IEnumerable<MigrationLegacyUserHospital>>
            {
                {
                    CASE_MANAGER_USERNAME,
                    new List<MigrationLegacyUserHospital>
                    {
                        new MigrationLegacyUserHospital {HospitalId = HOSPITAL_GUID}
                    }
                }
            };
            await _context.Hospital.AddAsync(new Hospital{HospitalId = HOSPITAL_GUID, TBService = new TBService{Code = tbServiceCode}});
            await _context.SaveChangesAsync();
        }

        private async Task GivenUserExistsInNtbsWithName(string givenName, string familyName)
        {
            await _context.User.AddAsync(
                new User {GivenName = givenName, FamilyName = familyName, Username = CASE_MANAGER_USERNAME, IsActive = true});
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
