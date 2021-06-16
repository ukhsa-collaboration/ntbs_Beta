using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class UserSearchServiceTest
    {
        private static readonly PaginationParameters DefaultPaginationParameters =
            new PaginationParameters { Offset = 0, PageSize = 20 };

        private static readonly List<User> DefaultDisplayNameCaseManagers = new List<User>
        {
            CreateDefaultCaseManager(displayName: "Test UserOne"),
            CreateDefaultCaseManager(displayName: "Test UserTwo"),
            CreateDefaultCaseManager(displayName: "Test UserThree"),
        };

        private static readonly List<User> DefaultGivenNameCaseManagers = new List<User>
        {
            CreateDefaultCaseManager(givenName: "Test UserOne"),
            CreateDefaultCaseManager(givenName: "Test UserTwo"),
            CreateDefaultCaseManager(givenName: "Test UserThree"),
        };

        private static readonly List<User> DefaultFamilyNameCaseManagers = new List<User>
        {
            CreateDefaultCaseManager(familyName: "Test UserOne"),
            CreateDefaultCaseManager(familyName: "Test UserTwo"),
            CreateDefaultCaseManager(familyName: "Test UserThree"),
        };

        private static readonly List<User> DefaultRegionalUsers = new List<User>
        {
            CreateRegionalUser("Region1AdGroup"),
            CreateRegionalUser("Region2AdGroup"),
            CreateRegionalUser("Region3AdGroup")
        };

        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IReferenceDataRepository> _mockReferenceDataRepository;
        private readonly IUserSearchService _service;

        public UserSearchServiceTest()
        {
            _mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            _service = new UserSearchService(_mockReferenceDataRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsEmptyList()
        {
            // Arrange
            const string searchString = "";
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(new List<User>());

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(0, results.count);
            Assert.Empty(results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsEmptyListWhenThereAreNoDisplayNameMatches()
        {
            // Arrange
            const string searchString = "abcdefg";

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultDisplayNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(0, results.count);
            Assert.Empty(results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsEmptyListWhenThereAreNoGivenNameMatches()
        {
            // Arrange
            const string searchString = "abcdefg";

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultGivenNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(0, results.count);
            Assert.Empty(results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsEmptyListWhenThereAreNoFamilyNameMatches()
        {
            // Arrange
            const string searchString = "abcdefg";

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultFamilyNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(0, results.count);
            Assert.Empty(results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsDisplayNameMatchingUsers()
        {
            // Arrange
            const string searchString = "UserT";
            var expectedResults = new List<User> { DefaultDisplayNameCaseManagers[1], DefaultDisplayNameCaseManagers[2] };

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultDisplayNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(2, results.count);
            Assert.Equal(expectedResults, results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsGivenNameMatchingUsers()
        {
            // Arrange
            const string searchString = "UserT";
            var expectedResults = new List<User> { DefaultGivenNameCaseManagers[1], DefaultGivenNameCaseManagers[2] };

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultGivenNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(2, results.count);
            Assert.Equal(expectedResults, results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsFamilyNameMatchingUsers()
        {
            // Arrange
            const string searchString = "UserT";
            var expectedResults = new List<User> { DefaultFamilyNameCaseManagers[1], DefaultFamilyNameCaseManagers[2] };

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultFamilyNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(2, results.count);
            Assert.Equal(expectedResults, results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_DoesNotReturnInactiveFamilyNameMatchingUsers()
        {
            // Arrange
            const string searchString = "UserT";
            var expectedResults = new List<User> { DefaultFamilyNameCaseManagers[2] };
            var usersToReturn = DefaultFamilyNameCaseManagers;
            usersToReturn[1].IsActive = false;

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultFamilyNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(1, results.count);
            Assert.Equal(expectedResults, results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsRegionMatchingCaseManagersAndUsers()
        {
            // Arrange
            const string searchString = "Region1";
            string regionAdGroup = DefaultRegionalUsers[0].AdGroups;
            var caseManagerWithRegion = CreateDefaultCaseManager("Thomas Haverford");
            caseManagerWithRegion.AdGroups = regionAdGroup;
            var expectedResult = new List<User> {DefaultRegionalUsers[0], caseManagerWithRegion};
            var usersToReturn = DefaultRegionalUsers;
            usersToReturn.Add(caseManagerWithRegion);

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs())
                .ReturnsAsync(new List<PHEC>{new PHEC{AdGroup = regionAdGroup, Name = searchString}});
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(usersToReturn);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(2, results.count);
            Assert.Equal(expectedResult, results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_ReturnsDisplayNameMatchingRegionalUsers()
        {
            // Arrange
            const string searchString = "Leslie";
            var expectedResult = CreateRegionalUser("RegionalAdGroup", "Leslie Knope");
            var regionalUsers = new List<User>
            {
                CreateRegionalUser("RegionalAdGroup", "Ben Wyatt"), expectedResult
            };

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs())
                .ReturnsAsync(new List<PHEC>{new PHEC{AdGroup = "RegionalAdGroup", Name = "Pawnee"}});
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(regionalUsers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(1, results.count);
            Assert.Equal(expectedResult, results.users[0]);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_DoesNotReturnInactiveDisplayNameMatchingRegionalUsers()
        {
            // Arrange
            const string searchString = "Leslie";
            var regionalUser = CreateRegionalUser("RegionalAdGroup", "Leslie Knope");
            regionalUser.IsActive = false;

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs())
                .ReturnsAsync(new List<PHEC>{new PHEC{AdGroup = "RegionalAdGroup", Name = "Pawnee"}});
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(new List<User>{regionalUser});

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Empty(results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_IsCaseInsensitive()
        {
            // Arrange
            const string searchString = "oNe";
            var expectedResults = new List<User> { DefaultDisplayNameCaseManagers[0] };

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultDisplayNameCaseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Equal(1, results.count);
            Assert.Equal(expectedResults, results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_DoesNotFindNonCaseManagerOrRegionalUsers()
        {
            // Arrange
            const string searchString = "Frank";

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(new List<User> {new User {DisplayName = "Frank Ma"}});

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, DefaultPaginationParameters);

            // Assert
            Assert.Empty(results.users);
        }

        [Fact]
        public async Task OrderAndPaginateQueryableAsync_PaginatesCorrectly()
        {
            // Arrange
            const string searchString = "Test";
            var caseManagers = Enumerable.Range(1, 9).Select(i => CreateDefaultCaseManager($"Test {i}")).ToList();
            var paginationParameters = new PaginationParameters { Offset = 2, PageSize = 3 };

            var expectedResults = new List<User> { caseManagers[2], caseManagers[3], caseManagers[4] };

            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(caseManagers);

            // Act
            var results = await _service.OrderAndPaginateQueryableAsync(searchString, paginationParameters);

            // Assert
            Assert.Equal(9, results.count);
            Assert.Equal(expectedResults, results.users);
        }

        private static User CreateDefaultCaseManager(string displayName = null, string givenName = null,
            string familyName = null,
            List<CaseManagerTbService> caseManagerTbServices = null)
        {
            return new User
            {
                DisplayName = displayName,
                GivenName = givenName,
                FamilyName = familyName,
                CaseManagerTbServices = caseManagerTbServices ?? new List<CaseManagerTbService>(),
                IsCaseManager = true,
                IsActive = true
            };
        }

        private static User CreateRegionalUser(string regionAdGroup, string displayName = null)
        {
            return new User
            {
                DisplayName = displayName,
                GivenName = "",
                FamilyName = "",
                CaseManagerTbServices = new List<CaseManagerTbService>(),
                AdGroups = regionAdGroup,
                IsActive = true
            };
        }
    }
}
