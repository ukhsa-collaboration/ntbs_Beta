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
                .Returns(new List<string> { "CN=Global.NIS.NTBS.NTA,CN=Users,DC=ntbs,DC=phe,DC=com", "CN=RandomOtherGroup,CN=Users,DC=ntbs,DC=phe,DC=com"});

            // Act
            _service.RunCaseManagerImport();

            // Assert
            mockUserRepository.Verify(r => r.AddOrUpdateUser(It.Is<User>
                (u => u.Username == firstUserPrincipal.UserPrincipalName &&
                      u.GivenName == firstUserPrincipal.GivenName &&
                      u.FamilyName == firstUserPrincipal.Surname &&
                      u.DisplayName == firstUserPrincipal.DisplayName &&
                      u.IsActive &&
                      u.IsCaseManager &&
                      u.AdGroup.Contains("Global.NIS.NTBS.Service_Nottingham") &&
                      u.AdGroup.Contains("Global.NIS.NTBS.EMS")
                ), It.IsAny<IEnumerable<TBService>>()), Times.Once()
            );

            mockUserRepository.Verify(r => r.AddOrUpdateUser(It.Is<User>
                (u => u.Username == secondUserPrincipal.UserPrincipalName &&
                      u.GivenName == secondUserPrincipal.GivenName &&
                      u.FamilyName == secondUserPrincipal.Surname &&
                      u.DisplayName == secondUserPrincipal.DisplayName &&
                      !u.IsActive &&
                      !u.IsCaseManager &&
                      u.AdGroup.Contains("Global.NIS.NTBS.NTA")
                ), It.IsAny<IEnumerable<TBService>>()), Times.Once()
            );
        }
    }
}