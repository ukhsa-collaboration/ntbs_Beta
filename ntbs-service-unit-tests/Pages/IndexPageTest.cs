using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Pages;
using ntbs_service.Services;
using ntbs_service_unit_tests.Helpers;
using Xunit;

namespace ntbs_service_unit_tests.Pages
{
    public class IndexPageTests
    {
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IAlertRepository> _mockAlertRepository;
        private readonly Mock<IHomepageKpiService> _mockHomepageKpiService;

        private readonly HomepageKpi mockHomepageKpiWithPhec = new HomepageKpi
        {
            Code = "PHEC001",
            PercentPositive = 17,
            PercentHivOffered = 25,
            PercentResistant = 11,
            PercentTreatmentDelay = 14
        };

        private readonly HomepageKpi mockHomepageKpiWithTbService = new HomepageKpi
        {
            Code = "TB001",
            PercentPositive = 15,
            PercentHivOffered = 27,
            PercentResistant = 12,
            PercentTreatmentDelay = 13
        };

        public IndexPageTests()
        {
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();
            _mockUserService = new Mock<IUserService>();
            _mockAlertRepository = new Mock<IAlertRepository>();
            _mockHomepageKpiService = new Mock<IHomepageKpiService>();
            SetUp();
        }

        private void SetUp()
        {
            var recent = GetRecentNotifications().AsQueryable();
            var drafts = GetDraftNotifications().AsQueryable();
            _mockNotificationRepository.Setup(s => s.GetRecentNotificationsIQueryable()).Returns(recent);
            _mockNotificationRepository.Setup(s => s.GetDraftNotificationsIQueryable()).Returns(drafts);
            _mockAuthorizationService.Setup(s => s.FilterNotificationsByUserAsync(It.IsAny<ClaimsPrincipal>(), recent)).Returns(Task.FromResult(recent));
            _mockAuthorizationService.Setup(s => s.FilterNotificationsByUserAsync(It.IsAny<ClaimsPrincipal>(), drafts)).Returns(Task.FromResult(drafts));

            _mockUserService.Setup(s => s.GetUserType(It.IsAny<ClaimsPrincipal>())).Returns(UserType.NationalTeam);
            _mockUserService.Setup(s => s.GetPhecCodesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new List<string> { mockHomepageKpiWithPhec.Code } as IEnumerable<string>));
            _mockHomepageKpiService.Setup(s => s.GetKpiForPhec(new List<string> { mockHomepageKpiWithPhec.Code }))
                .Returns(Task.FromResult(new List<HomepageKpi> { mockHomepageKpiWithPhec } as IEnumerable<HomepageKpi>));
            _mockHomepageKpiService.Setup(s => s.GetKpiForTbService(new List<string> { mockHomepageKpiWithTbService.Code }))
                .Returns(Task.FromResult(new List<HomepageKpi> { mockHomepageKpiWithTbService } as IEnumerable<HomepageKpi>));
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithRecentAndDraftNotifications()
        {
            // Arrange
            var pageModel = new IndexModel(_mockNotificationRepository.Object,
                _mockAlertRepository.Object,
                _mockAuthorizationService.Object,
                _mockUserService.Object,
                _mockHomepageKpiService.Object);
            pageModel.MockOutSession();

            // Act
            await pageModel.OnGetAsync();
            var resultRecent = Assert.IsAssignableFrom<List<Notification>>(pageModel.RecentNotifications);
            var resultDraft = Assert.IsAssignableFrom<List<Notification>>(pageModel.DraftNotifications);

            // Assert
            Assert.True(resultRecent.Count == 1);
            Assert.Equal("Bob", resultRecent[0].PatientDetails.GivenName);
            Assert.True(resultDraft.Count == 1);
            Assert.Equal("Ross", resultDraft[0].PatientDetails.GivenName);
        }

        [Fact]
        public async Task OnGetAsync_PopulatesHomepageKpisDetailsWithPhecCodes_WhenUserIsPheUser()
        {
            // Arrange
            _mockUserService.Setup(s => s.GetUserType(It.IsAny<ClaimsPrincipal>())).Returns(UserType.PheUser);
            _mockUserService.Setup(s => s.GetPhecCodesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new List<string> { mockHomepageKpiWithPhec.Code } as IEnumerable<string>));

            var pageModel = new IndexModel(_mockNotificationRepository.Object,
                _mockAlertRepository.Object,
                _mockAuthorizationService.Object,
                _mockUserService.Object,
                _mockHomepageKpiService.Object);
            pageModel.MockOutSession();

            // Act
            await pageModel.OnGetAsync();
            var phecCodes = Assert.IsAssignableFrom<SelectList>(pageModel.KpiFilter);
            var homepageKpiDetails = Assert.IsAssignableFrom<List<HomepageKpi>>(pageModel.HomepageKpiDetails);

            Assert.True(phecCodes.Count() == 1);
            Assert.True(homepageKpiDetails.Count == 1);
            Assert.True(homepageKpiDetails[0] == mockHomepageKpiWithPhec);
        }

        [Fact]
        public async Task OnGetAsync_PopulatesHomepageKpisDetailsWithTbServiceCodes_WhenUserIsNhsUser()
        {
            // Arrange
            _mockUserService.Setup(s => s.GetUserType(It.IsAny<ClaimsPrincipal>())).Returns(UserType.NhsUser);
            var mockTbService = new TBService() { Code = mockHomepageKpiWithTbService.Code };
            _mockUserService.Setup(s => s.GetTbServicesAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(Task.FromResult(new List<TBService> { mockTbService } as IEnumerable<TBService>));

            var pageModel = new IndexModel(_mockNotificationRepository.Object,
                _mockAlertRepository.Object,
                _mockAuthorizationService.Object,
                _mockUserService.Object,
                _mockHomepageKpiService.Object);
            pageModel.MockOutSession();

            // Act
            await pageModel.OnGetAsync();
            var phecCodes = Assert.IsAssignableFrom<SelectList>(pageModel.KpiFilter);
            var homepageKpiDetails = Assert.IsAssignableFrom<List<HomepageKpi>>(pageModel.HomepageKpiDetails);

            Assert.True(phecCodes.Count() == 1);
            Assert.True(homepageKpiDetails.Count == 1);
            Assert.True(homepageKpiDetails[0] == mockHomepageKpiWithTbService);
        }

        public IEnumerable<Notification> GetRecentNotifications()
        {
            var patient = new PatientDetails() { GivenName = "Bob" };

            return new List<Notification> { new Notification { PatientDetails = patient } };
        }

        public IEnumerable<Notification> GetDraftNotifications()
        {
            var patient = new PatientDetails() { GivenName = "Ross" };

            return new List<Notification> { new Notification { PatientDetails = patient } };
        }
    }
}
