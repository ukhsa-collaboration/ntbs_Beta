using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AdImportServiceTest
    {
        private readonly IAdImportService _service;
        private readonly Mock<IAdDirectoryFactory> mockDirectoryFactory;
        private readonly Mock<AdDirectoryService> mockDirectoryService;
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly Mock<IReferenceDataRepository> mockReferenceDataRepository;

        private readonly Mock<DirectoryEntry> firstMockDirectoryEntry;
        private readonly Mock<DirectoryEntry> secondMockDirectoryEntry;

        public AdImportServiceTest()
        {
            mockDirectoryFactory = new Mock<IAdDirectoryFactory>();
            mockUserRepository = new Mock<IUserRepository>();
            mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            mockDirectoryService = new Mock<AdDirectoryService>(new Mock<AdConnectionSettings>().Object);
            mockDirectoryFactory.Setup(u => u.Create()).Returns(mockDirectoryService.Object);
            firstMockDirectoryEntry = new Mock<DirectoryEntry>();
            secondMockDirectoryEntry = new Mock<DirectoryEntry>();
            mockDirectoryService.Setup(s => s.GetAllDirectoryEntries())
                .Returns(new List<DirectoryEntry> { firstMockDirectoryEntry.Object, secondMockDirectoryEntry.Object});
            mockReferenceDataRepository.Setup(s => s.GetAllTbServicesAsync())
                .Returns(Task.FromResult((IList<TBService>)(new List<TBService> { new TBService { ServiceAdGroup = "Global.NIS.NTBS.Service_Nottingham" }})));

            _service = new AdImportService(mockDirectoryFactory.Object, mockReferenceDataRepository.Object, mockUserRepository.Object);
        }

        [Fact]
        public void RunCaseManagerImport_ReturnsExpectedUsers()
        {
            // Arrange
            var dummyContext = new PrincipalContext(ContextType.Domain);
            var firstUserPrincipal = new UserPrincipal(dummyContext)
            {
                SamAccountName = "First@gmail.com",
                GivenName = "First",
                Surname = "First surname",
                DisplayName = "First surname"
            };
            var secondUserPrincipal = new UserPrincipal(dummyContext)
            {
                SamAccountName = "Second@gmail.com",
                GivenName = "Second",
                Surname = "Second surname",
                DisplayName = "Second surname"
            };
            mockDirectoryService.Setup(s => s.GetUsername(firstMockDirectoryEntry.Object)).Returns("first.user");
            mockDirectoryService.Setup(s => s.GetUsername(secondMockDirectoryEntry.Object)).Returns("second.user");
            mockDirectoryService.Setup(s => s.GetUserPrincipal("first.user")).Returns(firstUserPrincipal);
            mockDirectoryService.Setup(s => s.GetUserPrincipal("second.user")).Returns(secondUserPrincipal);
            mockDirectoryService.Setup(s => s.IsUserEnabled(firstMockDirectoryEntry.Object)).Returns(true);
            mockDirectoryService.Setup(s => s.IsUserEnabled(secondMockDirectoryEntry.Object)).Returns(false);
            mockDirectoryService.Setup(s => s.GetDistinguisedGroupNames(firstMockDirectoryEntry.Object))
                .Returns(new List<string> { "CN=Global.NIS.NTBS.Service_Nottingham,CN=Users,DC=ntbs,DC=phe,DC=com", "CN=Global.NIS.NTBS.EMS,CN=Users,DC=ntbs,DC=phe,DC=com"});
            mockDirectoryService.Setup(s => s.GetDistinguisedGroupNames(secondMockDirectoryEntry.Object))
                .Returns(new List<string>
                {
                    "CN=Global.NIS.NTBS.NTA,CN=Users,DC=ntbs,DC=phe,DC=com",
                    "CN=RandomOtherGroup,CN=Users,DC=ntbs,DC=phe,DC=com"
                });
            List<User> savedUsers = new List<User>();
            mockUserRepository
                .Setup(r => r.AddOrUpdateUser(It.IsAny<User>(), It.IsAny<IEnumerable<TBService>>()))
                .Callback<User, IEnumerable<TBService>>((user, _) => savedUsers.Add(user));

            // Act
            _service.RunCaseManagerImport();

            // Assert
            Assert.Equal(2, savedUsers.Count);

            var savedUser = savedUsers[0];
            Assert.Equal(savedUser.Username, firstUserPrincipal.UserPrincipalName);
            Assert.Equal(savedUser.GivenName, firstUserPrincipal.GivenName);
            Assert.Equal(savedUser.FamilyName, firstUserPrincipal.Surname);
            Assert.Equal(savedUser.DisplayName, firstUserPrincipal.DisplayName);
            Assert.True(savedUser.IsActive);
            Assert.True(savedUser.IsCaseManager);
            Assert.Contains("Global.NIS.NTBS.Service_Nottingham", savedUser.AdGroup);
            Assert.Contains("Global.NIS.NTBS.EMS", savedUser.AdGroup);

            var savedInactiveUser = savedUsers[1];
            Assert.Equal(savedInactiveUser.Username, secondUserPrincipal.UserPrincipalName);
            Assert.Equal(savedInactiveUser.GivenName, secondUserPrincipal.GivenName);
            Assert.Equal(savedInactiveUser.FamilyName, secondUserPrincipal.Surname);
            Assert.Equal(savedInactiveUser.DisplayName, secondUserPrincipal.DisplayName);
            Assert.False(savedInactiveUser.IsActive);
            Assert.False(savedInactiveUser.IsCaseManager);
            Assert.Contains("Global.NIS.NTBS.NTA", savedInactiveUser.AdGroup);
        }
    }
}
