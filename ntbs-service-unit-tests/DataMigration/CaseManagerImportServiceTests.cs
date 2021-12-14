using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using ntbs_service_unit_tests.TestHelpers;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class CaseManagerImportServiceTests : IDisposable
    {
        private const int BATCH_ID = 56789;
        private const string NOTIFICATION_ID = "11111";
        private const string CASE_MANAGER_USERNAME_1 = "TestUser@nhs.net";
        private const string CASE_MANAGER_USERNAME_2 = "MartinUser@nhs.net";
        private static readonly Guid HOSPITAL_GUID_1 = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A");
        private static readonly Guid HOSPITAL_GUID_2 = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7B");

        private readonly NtbsContext _context;
        private readonly CaseManagerImportService _caseManagerImportService;
        private readonly Mock<IMigrationRepository> _migrationRepositoryMock = new Mock<IMigrationRepository>();
        private readonly Mock<IOptionsMonitor<AdOptions>> _adOptionMock = new Mock<IOptionsMonitor<AdOptions>>();

        private Dictionary<string, MigrationLegacyUser> _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>();
        private Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> _usernameToLegacyUserHospitalDict = new Dictionary<string, IEnumerable<MigrationLegacyUserHospital>>();

        public CaseManagerImportServiceTests()
        {
            _context = SetupTestContext();
            ContextHelper.DisableAudits();
            SetupMockMigrationRepo();
            _adOptionMock.Setup(s => s.CurrentValue).Returns(new AdOptions{ReadOnlyUserGroup = "TestReadOnly"});
            IUserRepository userRepository = new UserRepository(_context, _adOptionMock.Object);
            IReferenceDataRepository referenceDataRepository = new ReferenceDataRepository(_context);
            Mock<INotificationImportRepository> mockNotificationImportRepository = new Mock<INotificationImportRepository>();

            var importLogger = new ImportLogger(mockNotificationImportRepository.Object);
            _caseManagerImportService = new CaseManagerImportService(userRepository, referenceDataRepository,
                _migrationRepositoryMock.Object, importLogger);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyNotificationDoesNotExistInNtbs_AddsInactiveUserWhoIsNotACaseManager()
        {
            // Arrange
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS00TEST", HOSPITAL_GUID_1);

            // Act
            await _caseManagerImportService.ImportOrUpdateLegacyUser(CASE_MANAGER_USERNAME_1, "TBS00TEST", null, BATCH_ID);

            // Assert
            var addedUser = _context.User.SingleOrDefault();
            Assert.NotNull(addedUser);
            Assert.Equal("John", addedUser.GivenName);
            Assert.Equal("Johnston", addedUser.FamilyName);
            Assert.False(addedUser.IsActive);
            Assert.False(addedUser.IsCaseManager);
            Assert.Empty(addedUser.CaseManagerTbServices);
        }

        [Fact]
        public async Task WhenInactiveCaseManagerForLegacyNotificationExistsInNtbs_UserNotImportedButNameUpdated()
        {
            // Arrange
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "John", "Johnston");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS99HULL", HOSPITAL_GUID_1);
            await GivenUserExistsInNtbsWithName("Jon", "Jonston", isActive: false);

            // Act
            await _caseManagerImportService.ImportOrUpdateLegacyUser(CASE_MANAGER_USERNAME_1, "TBS99HULL", null, BATCH_ID);

            // Assert
            var updatedUser = _context.User.Single();
            Assert.NotNull(updatedUser);
            Assert.Equal("John", updatedUser.GivenName);
            Assert.Equal("Johnston", updatedUser.FamilyName);
            Assert.Equal("John Johnston", updatedUser.DisplayName);
        }

        [Fact]
        public async Task WhenActiveCaseManagerForLegacyNotificationExistsInNtbs_UserNotImportedAndNameNotUpdated()
        {
            // Arrange
            GivenLegacyUserWithName(CASE_MANAGER_USERNAME_1, "Jon", "O'Cara");
            await GivenLegacyUserHasPermissionsForTbServiceInHospital(CASE_MANAGER_USERNAME_1, "TBS99HULL", HOSPITAL_GUID_1);
            await GivenUserExistsInNtbsWithName("Ben", "Kingsly", isActive: true);

            // Act
            await _caseManagerImportService.ImportOrUpdateLegacyUser(CASE_MANAGER_USERNAME_1, "TBS99HULL", null, BATCH_ID);

            // Assert
            var updatedUser = _context.User.Single();
            Assert.NotNull(updatedUser);
            Assert.Equal("Ben", updatedUser.GivenName);
            Assert.Equal("Kingsly", updatedUser.FamilyName);
            Assert.Equal("Ben Kingsly", updatedUser.DisplayName);
        }

        private void SetupMockMigrationRepo()
        {
            _migrationRepositoryMock.Setup(repo => repo.GetLegacyUserByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserDict[username]));
            _migrationRepositoryMock.Setup(repo => repo.GetLegacyUserHospitalsByUsername(It.IsAny<string>()))
                .Returns((string username) => Task.FromResult(_usernameToLegacyUserHospitalDict[username]));
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

        private async Task GivenUserExistsInNtbsWithName(string givenName, string familyName, bool isActive = true)
        {
            await _context.User.AddAsync(
                new User {GivenName = givenName, FamilyName = familyName, DisplayName = $"{givenName} {familyName}", Username = CASE_MANAGER_USERNAME_1, IsActive = isActive});
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
