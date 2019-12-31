using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AdImportServiceTest
    {
        private readonly IAdImportService _service;
        private readonly Mock<IUserRepository> mockUserRepository = new Mock<IUserRepository>();

        private readonly IAdDirectoryService mockDirectoryService = new MockAdDirectoryService();
        private static readonly PrincipalContext DummyContext = new PrincipalContext(ContextType.Domain);

        private static readonly UserPrincipal FirstUserPrincipal = new UserPrincipal(DummyContext)
        {
            SamAccountName = "First@gmail.com",
            GivenName = "First",
            Surname = "First surname",
            DisplayName = "First surname"
        };

        private static readonly UserPrincipal SecondUserPrincipal = new UserPrincipal(DummyContext)
        {
            SamAccountName = "Second@gmail.com",
            GivenName = "Second",
            Surname = "Second surname",
            DisplayName = "Second surname"
        };

        public AdImportServiceTest()
        {
            var mockDirectoryFactory = new Mock<IAdDirectoryFactory>();
            mockDirectoryFactory.Setup(u => u.Create()).Returns(mockDirectoryService);
            
            var mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            mockReferenceDataRepository.Setup(s => s.GetAllTbServicesAsync())
                .Returns(Task.FromResult((IList<TBService>)(new List<TBService>
                {
                    new TBService {ServiceAdGroup = "Global.NIS.NTBS.Service_Nottingham"}
                })));

            _service = new AdImportService(
                mockDirectoryFactory.Object,
                mockReferenceDataRepository.Object,
                mockUserRepository.Object);
        }

        [Fact]
        public void RunCaseManagerImport_ReturnsExpectedUsers()
        {
            // Arrange
            List<User> savedUsers = new List<User>();
            mockUserRepository
                .Setup(r => r.AddOrUpdateUser(It.IsAny<User>(), It.IsAny<IEnumerable<TBService>>()))
                .Callback<User, IEnumerable<TBService>>((user, _) => savedUsers.Add(user));

            // Act
            _service.RunCaseManagerImport();

            // Assert
            Assert.Equal(2, savedUsers.Count);

            var savedUser = savedUsers[0];
            Assert.Equal(savedUser.Username, FirstUserPrincipal.UserPrincipalName);
            Assert.Equal(savedUser.GivenName, FirstUserPrincipal.GivenName);
            Assert.Equal(savedUser.FamilyName, FirstUserPrincipal.Surname);
            Assert.Equal(savedUser.DisplayName, FirstUserPrincipal.DisplayName);
            Assert.True(savedUser.IsActive);
            Assert.True(savedUser.IsCaseManager);
            Assert.Contains("Global.NIS.NTBS.Service_Nottingham", savedUser.AdGroup);
            Assert.Contains("Global.NIS.NTBS.EMS", savedUser.AdGroup);

            var savedInactiveUser = savedUsers[1];
            Assert.Equal(savedInactiveUser.Username, SecondUserPrincipal.UserPrincipalName);
            Assert.Equal(savedInactiveUser.GivenName, SecondUserPrincipal.GivenName);
            Assert.Equal(savedInactiveUser.FamilyName, SecondUserPrincipal.Surname);
            Assert.Equal(savedInactiveUser.DisplayName, SecondUserPrincipal.DisplayName);
            Assert.False(savedInactiveUser.IsActive);
            Assert.False(savedInactiveUser.IsCaseManager);
            Assert.Contains("Global.NIS.NTBS.NTA", savedInactiveUser.AdGroup);
        }

        private class MockAdDirectoryService : IAdDirectoryService
        {
            private readonly Dictionary<string, (DirectoryEntry de, bool isEnabled, List<string> groups)> users
                = new Dictionary<string, (DirectoryEntry, bool, List<string>)>()
                {
                    {
                        FirstUserUsername, (new Mock<DirectoryEntry>().Object, true,
                            new List<string>
                            {
                                "CN=Global.NIS.NTBS.Service_Nottingham,CN=Users,DC=ntbs,DC=phe,DC=com",
                                "CN=Global.NIS.NTBS.EMS,CN=Users,DC=ntbs,DC=phe,DC=com"
                            })
                    },
                    {
                        SecondUserUsername, (new Mock<DirectoryEntry>().Object, false,
                            new List<string>
                            {
                                "CN=Global.NIS.NTBS.NTA,CN=Users,DC=ntbs,DC=phe,DC=com",
                                "CN=RandomOtherGroup,CN=Users,DC=ntbs,DC=phe,DC=com"
                            })
                    },
                };

            private const string FirstUserUsername = "first.user";
            private const string SecondUserUsername = "second.user";

            public IEnumerable<DirectoryEntry> GetAllDirectoryEntries()
            {
                return users.Values.Select(user => user.de);
            }

            public string GetUsername(DirectoryEntry de)
            {
                return users.Where((pair => pair.Value.de == de)).First().Key;
            }

            public bool IsUserEnabled(DirectoryEntry de)
            {
                return users[GetUsername(de)].isEnabled;
            }

            public IEnumerable<string> GetDistinguishedGroupNames(DirectoryEntry de)
            {
                return users[GetUsername(de)].groups;
            }

            public UserPrincipal GetUserPrincipal(string userName)
            {
                switch (userName)
                {
                    case FirstUserUsername: return FirstUserPrincipal;
                    case SecondUserUsername: return SecondUserPrincipal;
                    default: throw new ArgumentException();
                }
            }

            public void Dispose()
            {
                // no-op
            }
        }
    }
}
