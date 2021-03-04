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
    public class CaseManagerImportServiceTests : IClassFixture<CaseManagerImportServiceTests.DatabaseFixture>
    {
        private readonly CaseManagerImportService _caseManagerImportService;
        private readonly Mock<IMigrationRepository> _migrationRepositoryMock = new Mock<IMigrationRepository>();

        private Dictionary<string, IEnumerable<MigrationDbNotification>> _idToNotificationDict;
        private Dictionary<string, MigrationLegacyUser> _usernameToLegacyUserDict;
        private Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> _usernameToLegacyUserHospitalDict;
        private readonly NtbsContext _context;

        public CaseManagerImportServiceTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            SetupMockMigrationRepo();
            IUserRepository userRepository = new UserRepository(_context);
            IReferenceDataRepository referenceDataRepository = new ReferenceDataRepository(_context);
            var importLogger = new ImportLogger();
            _caseManagerImportService = new CaseManagerImportService(userRepository, referenceDataRepository,
                _migrationRepositoryMock.Object, importLogger);
        }

        [Fact]
        public async Task WhenCaseManagerForLegacyNotificationDoesNotExistInNtbs_AddsCaseManager()
        {
            // Arrange
            var notification = new Notification
            {
                IsLegacy = true,
                LTBRID = "11111",
                HospitalDetails = new HospitalDetails {TBServiceCode = "TBS00TEST"}
            };
            _idToNotificationDict = new Dictionary<string, IEnumerable<MigrationDbNotification>>
            {
                {
                    "11111",
                    new List<MigrationDbNotification> {new MigrationDbNotification {CaseManager = "TestUser@nhs.net"}}
                }
            };
            _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>
            {
                {
                    "TestUser@nhs.net",
                    new MigrationLegacyUser {Username = "TestUser@nhs.net", GivenName = "John", FamilyName = "Johnston"}
                }
            };
            _usernameToLegacyUserHospitalDict = new Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> 
                {{ "TestUser@nhs.net", new List<MigrationLegacyUserHospital>() }};

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
        public async Task WhenCaseManagerForLegacyNotificationDoesNotExistAndHasCorrectPermissions_AddsCaseManagerTbServices()
        {
            // Arrange
            var notification = new Notification
            {
                IsLegacy = true,
                LTBRID = "11111",
                HospitalDetails = new HospitalDetails {TBServiceCode = "TBS00TEST"}
            };
            _idToNotificationDict = new Dictionary<string, IEnumerable<MigrationDbNotification>>
            {
                {
                    "11111",
                    new List<MigrationDbNotification> {new MigrationDbNotification {CaseManager = "TestUser@nhs.net"}}
                }
            };
            _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>
            {
                {
                    "TestUser@nhs.net",
                    new MigrationLegacyUser {Username = "TestUser@nhs.net", GivenName = "John", FamilyName = "Johnston"}
                }
            };
            _usernameToLegacyUserHospitalDict = new Dictionary<string, IEnumerable<MigrationLegacyUserHospital>>
            {
                {
                    "TestUser@nhs.net",
                    new List<MigrationLegacyUserHospital>
                    {
                        new MigrationLegacyUserHospital {HospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A")}
                    }
                }
            };
            await _context.Hospital.AddAsync(new Hospital{HospitalId = new Guid("B8AA918D-233F-4C41-B9AE-BE8A8DC8BE7A"), TBService = new TBService{Code = "TBS00TEST"}});
            await _context.SaveChangesAsync();

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
        public async Task WhenCaseManagerForLegacyNotificationDoesExist_UpdatesName()
        {
            // Arrange
            var notification = new Notification
            {
                IsLegacy = true,
                LTBRID = "11111",
                HospitalDetails = new HospitalDetails {TBServiceCode = "TBS00TEST"}
            };
            _idToNotificationDict = new Dictionary<string, IEnumerable<MigrationDbNotification>>
            {
                {
                    "11111",
                    new List<MigrationDbNotification> {new MigrationDbNotification {CaseManager = "TestUser@nhs.net"}}
                }
            };
            _usernameToLegacyUserDict = new Dictionary<string, MigrationLegacyUser>
            {
                {
                    "TestUser@nhs.net",
                    new MigrationLegacyUser {Username = "TestUser@nhs.net", GivenName = "John", FamilyName = "Johnston"}
                }
            };
            _usernameToLegacyUserHospitalDict = new Dictionary<string, IEnumerable<MigrationLegacyUserHospital>> 
                {{ "TestUser@nhs.net", new List<MigrationLegacyUserHospital>() }};
            await _context.User.AddAsync(new User {GivenName = "Jon", FamilyName = "Jonston", Username = "TestUser@nhs.net"});
            await _context.SaveChangesAsync();

            // Act
            await _caseManagerImportService.ImportOrUpdateCaseManager(notification, null, "test-request-1");

            // Assert
            var updatedUser = _context.User.SingleOrDefault();
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

        public class DatabaseFixture : IDisposable
        {
            public DatabaseFixture()
            {
                Context = new NtbsContext(new DbContextOptionsBuilder<NtbsContext>()
                    .UseInMemoryDatabase(nameof(CaseManagerImportService))
                    .Options
                );
            }

            public void Dispose()
            {
                Context.Dispose();
            }

            public NtbsContext Context { get; private set; }
        }
    }
}
