using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Pages.Search;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Pages
{
    public class SearchPageTest
    {
        private readonly Mock<INotificationRepository> _mockNotificationRepository;
        private readonly Mock<IReferenceDataRepository> _mockReferenceDataRepository;
        private readonly Mock<ISearchService> _mockSearchService;
        private readonly Mock<IAuthorizationService> _mockAuthorizationService;
        public SearchPageTest()
        {
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockSearchService = new Mock<ISearchService>();
            _mockAuthorizationService = new Mock<IAuthorizationService>();
            _mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithSearchResults()
        {
            // Arrange
            IList<Sex> sexes = new List<Sex>();
            var sexList = Task.FromResult(sexes);
            _mockReferenceDataRepository.Setup(s => s.GetAllSexesAsync()).Returns(sexList);

            IList<Country> countries = new List<Country>();
            var countrySelectList = Task.FromResult(countries);
            _mockReferenceDataRepository.Setup(s => s.GetAllCountriesAsync()).Returns(countrySelectList);

            IList<TBService> tbServices = new List<TBService>();
            var tbServiceList = Task.FromResult(tbServices);
            _mockReferenceDataRepository.Setup(s => s.GetAllTbServicesAsync()).Returns(tbServiceList);

            _mockNotificationRepository.Setup(s => s.GetQueryableNotificationByStatus(It.IsAny<List<NotificationStatus>>())).Returns(new List<Notification> { new Notification() { NotificationId = 1 } }.AsQueryable());

            var unionAndPaginateResult = Task.FromResult(GetNotificationIdsAndCount());
            _mockSearchService.Setup(s => s.OrderAndPaginateQueryables(It.IsAny<IQueryable<Notification>>(), It.IsAny<IQueryable<Notification>>(),
                It.IsAny<PaginationParameters>())).Returns(unionAndPaginateResult);

            var notifications = Task.FromResult(GetNotifications());
            _mockNotificationRepository.Setup(s => s.GetNotificationsByIdsAsync(It.IsAny<List<int>>())).Returns(notifications);

            var httpContext = new DefaultHttpContext();
            var modelState = new ModelStateDictionary();
            var actionContext = new ActionContext(httpContext, new Microsoft.AspNetCore.Routing.RouteData(), new PageActionDescriptor(), modelState);
            var pageContext = new PageContext(actionContext);

            var pageModel = new IndexModel(_mockNotificationRepository.Object, _mockSearchService.Object, _mockAuthorizationService.Object, _mockReferenceDataRepository.Object)
            {
                SearchParameters = new SearchParameters(),
                PageContext = pageContext
            };

            // Act
            await pageModel.OnGetAsync(1);

            // Assert
            var results = Assert.IsAssignableFrom<PaginatedList<NotificationBannerModel>>(pageModel.SearchResults);
            Assert.True(results.Count == 2);
            Assert.Equal("Bob", results[0].Name);
            Assert.Equal("Ross", results[1].Name);
        }

        public (IList<int> notificationIds, int count) GetNotificationIdsAndCount()
        {
            return (notificationIds: new List<int> { 1, 2 }, count: 1);
        }

        public IList<Notification> GetNotifications()
        {
            var patient1 = new PatientDetails { GivenName = "Bob" };
            var patient2 = new PatientDetails { GivenName = "Ross" };

            return new List<Notification> { new Notification { PatientDetails = patient1, NotificationId = 1 }, new Notification { PatientDetails = patient2, NotificationId = 2 } };
        }
    }
}
