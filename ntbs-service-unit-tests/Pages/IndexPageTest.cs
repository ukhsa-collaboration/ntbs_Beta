using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Moq;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Pages;
using Xunit;
using ntbs_service.DataAccess;
using System.Security.Claims;

namespace ntbs_service_unit_tests.Pages
{
    public class IndexPageTests
    {
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        private readonly Mock<IAuthorizationService> mockAuthorizationService;
        private readonly Mock<IUserService> mockUserService;
        private readonly Mock<IAlertRepository> mockAlertRepository;
        public IndexPageTests() 
        {
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockAuthorizationService = new Mock<IAuthorizationService>();
            mockUserService = new Mock<IUserService>();
            mockAlertRepository = new Mock<IAlertRepository>();
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithRecentAndDraftNotifications()
        {
            // Arrange
            var recent = GetRecentNotifications().AsQueryable();
            var drafts = GetDraftNotifications().AsQueryable();
            mockNotificationRepository.Setup(s => s.GetRecentNotificationsIQueryable()).Returns(recent);
            mockNotificationRepository.Setup(s => s.GetDraftNotificationsIQueryable()).Returns(drafts);
            mockAuthorizationService.Setup(s => s.FilterNotificationsByUserAsync(It.IsAny<ClaimsPrincipal>(), recent)).Returns(Task.FromResult(recent));
            mockAuthorizationService.Setup(s => s.FilterNotificationsByUserAsync(It.IsAny<ClaimsPrincipal>(), drafts)).Returns(Task.FromResult(drafts));

            var pageModel = new IndexModel(mockNotificationRepository.Object, mockAlertRepository.Object, mockAuthorizationService.Object, mockUserService.Object);

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

        public IEnumerable<Notification> GetRecentNotifications()
        {
            var patient = new PatientDetails() { GivenName = "Bob" };

            return new List<Notification> { new Notification{ PatientDetails = patient } };
        }

        public IEnumerable<Notification> GetDraftNotifications()
        {
            var patient = new PatientDetails() { GivenName = "Ross" };

            return new List<Notification> { new Notification{ PatientDetails = patient } };
        }
    }
}
