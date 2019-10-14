using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Pages_Search;
using Xunit;
using ntbs_service.Models.Enums;
using System.Linq;

namespace ntbs_service_tests.UnitTests.Search
{
    public class SearchPageTest
    {
        private Mock<INotificationService> mockNotificationService;
        private Mock<ISearchService> mockSearchService;
        private Mock<NtbsContext> mockContext;
        public SearchPageTest() 
        {
            mockNotificationService = new Mock<INotificationService>();
            mockSearchService = new Mock<ISearchService>();
            mockContext = new Mock<NtbsContext>();
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithSearchResults()
        {
            // Arrange
            var draftStatusList = new List<NotificationStatus>() {NotificationStatus.Draft};
            var nonDraftStatusList = new List<NotificationStatus>() {NotificationStatus.Notified, NotificationStatus.Denotified};
            mockNotificationService.Setup(s => s.GetBaseQueryableNotificationByStatus(draftStatusList)).Returns(new List<Notification> { new Notification() {NotificationId = 1}}.AsQueryable());
            mockNotificationService.Setup(s => s.GetBaseQueryableNotificationByStatus(nonDraftStatusList)).Returns(new List<Notification> { new Notification() {NotificationId = 1}}.AsQueryable());
            
            mockSearchService.Setup(s => s.OrderQueryableByNotificationDate(It.IsAny<IQueryable<Notification>>())).Returns(new List<Notification> { new Notification() {NotificationId = 1}}.AsQueryable());
            var notificationIds = Task.FromResult(GetNotificationIds());
            mockSearchService.Setup(s => s.GetPaginatedItemsAsync(It.IsAny<IQueryable<int>>(), It.IsAny<PaginationParameters>())).Returns(notificationIds);

            var y = Task.FromResult(GetNotifications());
            mockNotificationService.Setup(s => s.GetNotificationsByIdAsync(new List<int>() {1})).Returns(y);

            IList<Sex> sexes = new List<Sex> {};
            var sexList = Task.FromResult(sexes);
            mockContext.Setup(s => s.GetAllSexesAsync()).Returns(sexList);

            var pageModel = new IndexModel(mockNotificationService.Object, mockSearchService.Object, mockContext.Object) {
                SearchParameters = new SearchParameters() {IdFilter = "1"}
            };

            // Act
            await pageModel.OnGetAsync(1);
            var results = Assert.IsAssignableFrom<PaginatedList<NotificationBannerModel>>(pageModel.SearchResults);

            // Assert
            Assert.True(results.Count == 1);
            Assert.Equal("Bob", results[0].Name);
        }

        public IList<int> GetNotificationIds() {
            return new List<int>() {1};
        }

        public IEnumerable<Notification> GetNotifications()
        {
            var patient1 = new PatientDetails() { GivenName = "Bob" };
            var patient2 = new PatientDetails() { GivenName = "Ross" };

            return new List<Notification> { new Notification{ PatientDetails = patient1, NotificationId = 1 }, new Notification{ PatientDetails = patient2, NotificationId = 2 }};
        }
    }
}
