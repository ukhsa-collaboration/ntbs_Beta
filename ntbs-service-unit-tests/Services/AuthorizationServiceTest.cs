using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class AuthorizationServiceTest
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IUserHelper> _mockUserHelper;
        private readonly Mock<INotificationRepository> _mockNotificationRepository;

        public AuthorizationServiceTest()
        {
            _mockUserService = new Mock<IUserService>();
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockUserHelper = new Mock<IUserHelper>();

            _authorizationService = new AuthorizationService(
                _mockUserService.Object,
                _mockNotificationRepository.Object,
                _mockUserHelper.Object);
        }

        [Fact]
        public async Task IsUserAuthorizedToManageAlert_AllowsUserWithCorrectTbServiceToManageTransferAlert()
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var tbService = new TBService() {Code = "TBS0008"};
            var testAlert = new TransferAlert()
            {
                NotificationId = 2, AlertType = AlertType.TransferRequest, TbServiceCode = tbService.Code
            };
            _mockUserHelper.Setup(uh => uh.UserIsReadOnly(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _mockUserService.Setup(us => us.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult((new List<TBService> { tbService }).AsEnumerable()));

            // Act
            var result = await _authorizationService.IsUserAuthorizedToManageAlert(testUser, testAlert);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsUserAuthorizedToManageAlert_DoesNotAllowUserWithoutTbServiceToManageTransferAlert()
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var tbService = new TBService() {Code = "TBS0008"};
            var testAlert = new TransferAlert()
            {
                NotificationId = 2, AlertType = AlertType.TransferRequest, TbServiceCode = tbService.Code
            };
            _mockUserHelper.Setup(uh => uh.UserIsReadOnly(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _mockUserService.Setup(us => us.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult((new List<TBService>()).AsEnumerable()));

            // Act
            var result = await _authorizationService.IsUserAuthorizedToManageAlert(testUser, testAlert);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsUserAuthorizedToManageAlert_DoesNotAllowReadOnlyUserToManageAlert()
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var testAlert = new TestAlert()
            {
                NotificationId = 2, AlertType = AlertType.Test
            };
            _mockUserHelper.Setup(uh => uh.UserIsReadOnly(It.IsAny<ClaimsPrincipal>())).Returns(true);

            // Act
            var result = await _authorizationService.IsUserAuthorizedToManageAlert(testUser, testAlert);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task IsUserAuthorizedToManageAlert_AllowsUserWithCorrectTbServiceToManageNonTransferAlert()
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var tbService = new TBService() {Code = "TBS0008"};
            var testAlert = new TestAlert()
            {
                NotificationId = 2, AlertType = AlertType.Test
            };
            var testNotification = new Notification{HospitalDetails = new HospitalDetails{TBServiceCode = tbService.Code}};
            _mockUserHelper.Setup(uh => uh.UserIsReadOnly(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _mockUserService.Setup(us => us.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult((new List<TBService>{tbService}).AsEnumerable()));
            _mockNotificationRepository.Setup(nr => nr.GetNotificationAsync((int)testAlert.NotificationId))
                .Returns(Task.FromResult(testNotification));

            // Act
            var result = await _authorizationService.IsUserAuthorizedToManageAlert(testUser, testAlert);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsUserAuthorizedToManageAlert_DoesNotAllowUserWithoutTbServiceToManageNonTransferAlert()
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var tbService = new TBService() {Code = "TBS0008"};
            var testAlert = new TestAlert()
            {
                NotificationId = 2, AlertType = AlertType.Test
            };
            var testNotification = new Notification{HospitalDetails = new HospitalDetails{TBServiceCode = "TBS1111"}};
            _mockUserHelper.Setup(uh => uh.UserIsReadOnly(It.IsAny<ClaimsPrincipal>())).Returns(false);
            _mockUserService.Setup(us => us.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult((new List<TBService> {tbService}).AsEnumerable()));
            _mockNotificationRepository.Setup(nr => nr.GetNotificationAsync((int)testAlert.NotificationId))
                .Returns(Task.FromResult(testNotification));

            // Act
            var result = await _authorizationService.IsUserAuthorizedToManageAlert(testUser, testAlert);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(UserType.NhsUser)]
        [InlineData(UserType.PheUser)]
        [InlineData(UserType.NationalTeam)]
        public async Task FilterAlertsForUser_FiltersByTheUsersTbServicesForAllUserTypes(UserType userType)
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var tbService = new TBService() {Code = "TBS0008"};
            var alertToExpect = new AlertWithTbServiceForDisplay()
            {
                AlertId = 1, NotificationId = 2, AlertType = AlertType.Test, TbServiceCode = tbService.Code
            };
            var testAlerts = new List<AlertWithTbServiceForDisplay>
            {
                alertToExpect,
                new AlertWithTbServiceForDisplay()
                {
                    AlertId = 2, NotificationId = 3, AlertType = AlertType.Test, TbServiceCode = "TBS0DOG"
                }
            };
            _mockUserService.Setup(us => us.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult((new List<TBService> {tbService}).AsEnumerable()));
            _mockUserService.Setup(us => us.GetUserType(It.IsAny<ClaimsPrincipal>()))
                .Returns(userType);

            // Act
            var result = await _authorizationService.FilterAlertsForUserAsync(testUser, testAlerts);

            // Assert
            Assert.Single(result);
            Assert.Contains(alertToExpect, result);
        }

        [Fact]
        public async Task FilterAlertsForUser_DoesNotFilterAnyAlertsForPheUser()
        {
            // Arrange
            var testUser = new ClaimsPrincipal(new ClaimsIdentity("TestDev"));
            var tbService = new TBService() {Code = "TBS0008"};
            var alertsToExpect = new List<AlertWithTbServiceForDisplay>
            {
                new AlertWithTbServiceForDisplay()
                {
                    AlertId = 1,
                    NotificationId = 2,
                    AlertType = AlertType.TransferRequest,
                    TbServiceCode = tbService.Code
                },
                new AlertWithTbServiceForDisplay()
                {
                    AlertId = 1,
                    NotificationId = 2,
                    AlertType = AlertType.Test,
                    TbServiceCode = tbService.Code
                },
                new AlertWithTbServiceForDisplay()
                {
                    AlertId = 2,
                    NotificationId = 3,
                    AlertType = AlertType.TransferRejected,
                    TbServiceCode = tbService.Code
                }
            };
            _mockUserService.Setup(us => us.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult((new List<TBService> {tbService}).AsEnumerable()));
            _mockUserService.Setup(us => us.GetUserType(It.IsAny<ClaimsPrincipal>()))
                .Returns(UserType.PheUser);

            // Act
            var result = await _authorizationService.FilterAlertsForUserAsync(testUser, alertsToExpect);

            // Assert
            Assert.Equal(alertsToExpect.Count, result.Count);
            Assert.Contains(alertsToExpect[0], result);
            Assert.Contains(alertsToExpect[1], result);
            Assert.Contains(alertsToExpect[2], result);
        }
    }
}
