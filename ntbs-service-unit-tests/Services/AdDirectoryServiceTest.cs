using System.Collections.Generic;
using System.Linq;
using Moq;
using Novell.Directory.Ldap;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AdDirectoryServiceTest
    {
        private readonly Mock<AdDirectoryService> adDirectoryServiceMock = new Mock<AdDirectoryService>() { CallBase = true };
        private readonly AdDirectoryService adDirectoryService;

        public AdDirectoryServiceTest()
        {
            var firstUserPrincipal = new LdapEntry("CN=First,CN=Users,DC=ntbs,DC=phe,DC=com", new LdapAttributeSet
            {
                new LdapAttribute("userPrincipalName", "First@phe.ntbs.com"),
                new LdapAttribute("givenName", "First"),
                new LdapAttribute("sn", "First surname"),
                new LdapAttribute("displayName", "First surname"),
                new LdapAttribute("userAccountControl", "512"),
                new LdapAttribute("memberof",new[] {
                    "CN=Global.NIS.NTBS.Service_Nottingham,CN=Users,DC=ntbs,DC=phe,DC=com",
                    "CN=Global.NIS.NTBS.EMS,CN=Users,DC=ntbs,DC=phe,DC=com"
                })
            });
            var secondUserPrincipal = new LdapEntry("CN=Second,CN=Users,DC=ntbs,DC=phe,DC=com", new LdapAttributeSet
            {
                new LdapAttribute("userPrincipalName", "Second@phe.ntbs.com"),
                new LdapAttribute("givenName", "Second"),
                new LdapAttribute("sn", "Second surname"),
                new LdapAttribute("displayName", "Second surname"),
                new LdapAttribute("userAccountControl", "510"),
                new LdapAttribute("memberof",new[] {
                    "CN=Global.NIS.NTBS.NTS,CN=Users,DC=ntbs,DC=phe,DC=com",
                    "CN=RandomOtherGroup,CN=Users,DC=ntbs,DC=phe,DC=com"
                })
            });

            adDirectoryServiceMock.Setup(s => s.GetAllDirectoryEntries()).Returns(new List<LdapEntry>
            {
                firstUserPrincipal,
                secondUserPrincipal
            });

            adDirectoryService = adDirectoryServiceMock.Object;
        }

        [Fact]
        public void RunCaseManagerImport_ReturnsExpectedUsers()
        {
            // Arrange
            // Act
            var results = adDirectoryService.LookupUsers(new List<TBService>
            {
                new TBService {ServiceAdGroup = "Global.NIS.NTBS.Service_Nottingham"}
            });
            var savedUsers = results.Select(result => result.user).ToList();

            // Assert
            Assert.Equal(2, savedUsers.Count);

            var savedUser = savedUsers[0];
            Assert.Equal("First@phe.ntbs.com", savedUser.Username);
            Assert.Equal("First", savedUser.GivenName);
            Assert.Equal("First surname", savedUser.FamilyName);
            Assert.Equal("First surname", savedUser.DisplayName);
            Assert.True(savedUser.IsActive);
            Assert.True(savedUser.IsCaseManager);
            Assert.Contains("Global.NIS.NTBS.Service_Nottingham", savedUser.AdGroups);
            Assert.Contains("Global.NIS.NTBS.EMS", savedUser.AdGroups);

            var savedInactiveUser = savedUsers[1];
            Assert.Equal("Second@phe.ntbs.com", savedInactiveUser.Username);
            Assert.Equal("Second", savedInactiveUser.GivenName);
            Assert.Equal("Second surname", savedInactiveUser.FamilyName);
            Assert.Equal("Second surname", savedInactiveUser.DisplayName);
            Assert.False(savedInactiveUser.IsActive);
            Assert.False(savedInactiveUser.IsCaseManager);
            Assert.Contains("Global.NIS.NTBS.NTS", savedInactiveUser.AdGroups);
        }
    }
}
