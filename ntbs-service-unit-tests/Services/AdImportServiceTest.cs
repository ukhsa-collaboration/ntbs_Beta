using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Novell.Directory.Ldap;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AdImportServiceTest : IDisposable
    {
        private readonly AdImportService _adImportService;
        private readonly Mock<IAdDirectoryServiceFactory> _adDirectoryServiceFactoryMock = new Mock<IAdDirectoryServiceFactory>();
        private readonly Mock<IOptionsMonitor<AdOptions>> _adOptionMock = new Mock<IOptionsMonitor<AdOptions>>();
        private readonly Mock<AdDirectoryService> _adDirectoryServiceMock = new Mock<AdDirectoryService> { CallBase = true };
        private List<LdapEntry> _adUsers;
        private NtbsContext _context;

        public AdImportServiceTest()
        {
            _context = SetupTestContext();
            
            _adUsers = new List<LdapEntry>();
            var adDirectoryService = SetupMockAdDirectoryService();
            _adDirectoryServiceFactoryMock.Setup(s => s.Create()).Returns(adDirectoryService);
            _adOptionMock.Setup(s => s.CurrentValue).Returns(new AdOptions{ReadOnlyUserGroup = "TestReadOnly"});

            var referenceRepo = new ReferenceDataRepository(_context);
            var userRepo = new UserRepository(_context, _adOptionMock.Object);
            var adUserService = new AdUserService(userRepo);
            
            _adImportService = new AdImportService(
                _adDirectoryServiceFactoryMock.Object,
                referenceRepo,
                adUserService);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        [Fact]
        public async void UserSyncJobImportsUserFromAd()
        {
            // Arrange
            GivenUserInAdWithUsernameAndTbService("testerson@phe.ntbs.com", "Global.NIS.NTBS.Service_Nottingham");
            GivenTbServicesExist(new List<TBService>{new TBService{ServiceAdGroup = "Global.NIS.NTBS.Service_Nottingham"}});
            
            // Act
            await _adImportService.RunCaseManagerImportAsync();

            // Assert
            var userImported = _context.User.Single();
            Assert.Equal("testerson@phe.ntbs.com", userImported.Username);
            Assert.True(userImported.IsActive);
            Assert.Contains("Global.NIS.NTBS.Service_Nottingham", userImported.AdGroups);
        }
        
        [Fact]
        public async void UserSyncJobSetsNtbsUserAsLegacyIfNotPresentInAd()
        {
            // Arrange
            GivenUserExistsInNtbs(new User{Username = "ghost@phe.nhs.uk", IsActive = true, AdGroups = "Global.NIS.NTBS.Service_Leeds"});
            
            // Act
            await _adImportService.RunCaseManagerImportAsync();

            // Assert
            var legacyUser = _context.User.Single(u => u.Username == "ghost@phe.nhs.uk");
            Assert.False(legacyUser.IsActive);
            Assert.Null(legacyUser.AdGroups);
        }

        private void GivenUserExistsInNtbs(User user)
        {
            _context.User.Add(user);
            _context.SaveChanges();
        }

        private void GivenTbServicesExist(List<TBService> tbServices)
        {
            _context.TbService.AddRange(tbServices);
            _context.SaveChanges();
        }

        private void GivenUserInAdWithUsernameAndTbService(string username, string tbService)
        {
            var userPrincipal = new LdapEntry("CN=First,CN=Users,DC=ntbs,DC=phe,DC=com", new LdapAttributeSet
            {
                new LdapAttribute("userPrincipalName", username),
                new LdapAttribute("givenName", "Test"),
                new LdapAttribute("sn", "Test Testerson"),
                new LdapAttribute("displayName", "Test Testerson"),
                new LdapAttribute("userAccountControl", "512"),
                new LdapAttribute("memberof",new[] {
                    $"CN={tbService},CN=Users,DC=ntbs,DC=phe,DC=com"
                })
            });

            _adUsers.Add(userPrincipal);
        }

        private AdDirectoryService SetupMockAdDirectoryService()
        {
            _adDirectoryServiceMock.Setup(s => s.GetAllDirectoryEntries()).Returns
            (
                _adUsers
            );
            return _adDirectoryServiceMock.Object;
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
