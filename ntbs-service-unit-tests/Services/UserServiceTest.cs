using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class UserServiceTest
    {
        private const string NationalTeam = "NTA";
        private const string ServicePrefix = "Service";
        private readonly IUserService service;
        private readonly AdfsOptions options = new AdfsOptions
        { 
            NationalTeamAdGroup = NationalTeam, 
            ServiceGroupAdPrefix = ServicePrefix,
            AdGroupsPrefix = ""
        };
    
        private readonly Mock<IOptionsMonitor<AdfsOptions>> mockOptionsMonitor;
        private readonly Mock<NtbsContext> mockContext;
        private readonly Mock<ClaimsPrincipal> mockUser;

        public UserServiceTest()
        {
            mockContext = new Mock<NtbsContext>();
            mockOptionsMonitor = new Mock<IOptionsMonitor<AdfsOptions>>();
            mockOptionsMonitor.Setup(o => o.CurrentValue).Returns(options);
            service = new UserService(mockContext.Object, mockOptionsMonitor.Object);

            mockUser = new Mock<ClaimsPrincipal>();
        }

        [Fact]
        public async Task GetUserPermissionsFilter_ReturnsExpectedFilter_ForNationalUser()
        {
            // Arrange
            var claim = new Claim("User", NationalTeam);
            SetupClaimMocking(claim);
            mockUser.Setup(u => u.IsInRole(NationalTeam)).Returns(true);

            // Act
            var result = await service.GetUserPermissionsFilterAsync(mockUser.Object);

            // Assert
            Assert.Empty(result.IncludedTBServiceCodes);
            Assert.Empty(result.IncludedPHECCodes);
            Assert.Equal(UserType.NationalTeam, result.Type);
        }

        [Fact]
        public async Task GetUserPermissionsFilter_ReturnsExpectedFilter_ForNhsUser()
        {
            // Arrange
            var serviceAdGroup = ServicePrefix + "Ashford";
            var code = "TBS0008";
            var tbService = new TBService() { ServiceAdGroup = serviceAdGroup, Code = code };
    
            var claim = new Claim("User", serviceAdGroup);
            SetupClaimMocking(claim);
            var serviceCodeList = new List<string> { code };
            mockContext.Setup(c => c.GetTbServiceCodesMatchingRolesAsync(It.Is<IEnumerable<string>>(l => l.Contains(serviceAdGroup))))
                .Returns(Task.FromResult(serviceCodeList));

            // Act
            var result = await service.GetUserPermissionsFilterAsync(mockUser.Object);

            // Assert
            Assert.Empty(result.IncludedPHECCodes);
            Assert.Single(result.IncludedTBServiceCodes);
            Assert.Equal(code, result.IncludedTBServiceCodes.First());
            Assert.Equal(UserType.NhsUser, result.Type);
        }

        [Fact]
        public async Task GetUserPermissionsFilter_ReturnsExpectedFilter_ForPheUser()
        {
            // Arrange
            var adGroup = "SoE";
            var code = "E45000019";
            var phec = new PHEC() { AdGroup = adGroup, Code = code };
    
            var claim = new Claim("User", adGroup);
            SetupClaimMocking(claim);
            var phecCodeList = new List<string> { code };
            mockContext.Setup(c => c.GetPhecCodesMatchingRolesAsync(It.Is<IEnumerable<string>>(l => l.Contains(adGroup))))
                .Returns(Task.FromResult(phecCodeList));

            // Act
            var result = await service.GetUserPermissionsFilterAsync(mockUser.Object);

            // Assert
            Assert.Empty(result.IncludedTBServiceCodes);
            Assert.Single(result.IncludedPHECCodes);
            Assert.Equal(code, result.IncludedPHECCodes.First());
            Assert.Equal(UserType.PheUser, result.Type);
        }

        [Fact]
        public async Task GetDefaultTbService_ReturnsNull_ForNationalUser()
        {
            // Arrange
            var claim = new Claim("User", NationalTeam);
            SetupClaimMocking(claim);
            mockUser.Setup(u => u.IsInRole(NationalTeam)).Returns(true);

            // Act
            var result = await service.GetDefaultTBService(mockUser.Object);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetDefaultTbService_ReturnsMatchingService_ForNhsUser()
        {
            // Arrange
            var serviceAdGroup = ServicePrefix + "Ashford";
            var tbService = new TBService() { ServiceAdGroup = serviceAdGroup, Code = "TBS0008" };
    
            var claim = new Claim("User", serviceAdGroup);
            SetupClaimMocking(claim);
            mockContext.Setup(c => c.GetDefaultTbServiceForNhsUserAsync(It.Is<IEnumerable<string>>(l => l.Contains(serviceAdGroup))))
                .Returns(Task.FromResult(tbService));

            // Act
            var result = await service.GetDefaultTBService(mockUser.Object);

            // Assert
            Assert.Equal(tbService, result);
        }

        [Fact]
        public async Task GetDefaultTbService_ReturnsMatchingService_ForPheUser()
        {
            // Arrange
            var adGroup = "SoE";
            var serviceAdGroup = ServicePrefix + "Ashford";
            var tbService = new TBService() { ServiceAdGroup = serviceAdGroup, Code = "TBS0008" };
    
            var claim = new Claim("User", adGroup);
            SetupClaimMocking(claim);
            mockContext.Setup(c => c.GetDefaultTbServiceForPheUserAsync(It.Is<IEnumerable<string>>(l => l.Contains(adGroup))))
                .Returns(Task.FromResult(tbService));

            // Act
            var result = await service.GetDefaultTBService(mockUser.Object);

            // Assert
            Assert.Equal(tbService, result);
        }


        private void SetupClaimMocking(Claim claim)
        {
            mockUser.Setup(u => u.FindAll(It.IsAny<Predicate<Claim>>())).Returns(new List<Claim> { claim });
        }
    }
}