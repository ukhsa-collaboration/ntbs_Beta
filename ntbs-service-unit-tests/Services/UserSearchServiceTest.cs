using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class UserSearchServiceTest
    {
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
        public async Task OrderQueryableAsync_ReturnsEmptyList()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("");
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(new List<User>());

            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyListWhenThereAreNoDisplayNameMatches()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("abcdefg");
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultDisplayNameCaseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyListWhenThereAreNoGivenNameMatches()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("abcdefg");
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultGivenNameCaseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyListWhenThereAreNoFamilyNameMatches()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("abcdefg");
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultFamilyNameCaseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsDisplayNameMatchingUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("UserT");
            var expectedResults = new List<User> { DefaultDisplayNameCaseManagers[1], DefaultDisplayNameCaseManagers[2] };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultDisplayNameCaseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(expectedResults, results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsGivenNameMatchingUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("UserT");
            var expectedResults = new List<User> { DefaultGivenNameCaseManagers[1], DefaultGivenNameCaseManagers[2] };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultGivenNameCaseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(expectedResults, results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsFamilyNameMatchingUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("UserT");
            var expectedResults = new List<User> { DefaultFamilyNameCaseManagers[1], DefaultFamilyNameCaseManagers[2] };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(DefaultFamilyNameCaseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(expectedResults, results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_DoesNotReturnInactiveFamilyNameMatchingUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("Glasper");
            var activeCaseManager = CreateDefaultCaseManager(givenName: "Robert", familyName: "Glasper");
            var inactiveCaseManager = CreateDefaultCaseManager(givenName: "Robin", familyName: "Glasper");
            inactiveCaseManager.IsActive = false;
            var usersToReturn = new List<User> {activeCaseManager, inactiveCaseManager};
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(usersToReturn);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(1, results.Count);
            Assert.Equal(activeCaseManager, results.Single());
        }

        [Fact]
        public async Task OrderQueryableAsync_ReturnsDisplayNameMatchingRegionalUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("Leslie");
            var expectedResult = CreateRegionalUser("RegionalAdGroup", "Leslie Knope");
            var regionalUsers = new List<User>
            {
                CreateRegionalUser("RegionalAdGroup", "Ben Wyatt"), expectedResult
            };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs())
                .ReturnsAsync(new List<PHEC>{new PHEC{AdGroup = "RegionalAdGroup", Name = "Pawnee"}});
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(regionalUsers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(1, results.Count);
            Assert.Equal(expectedResult, results[0]);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_DoesNotReturnInactiveDisplayNameMatchingRegionalUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("Leslie");
            var regionalUser = CreateRegionalUser("RegionalAdGroup", "Leslie Knope");
            regionalUser.IsActive = false;
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs())
                .ReturnsAsync(new List<PHEC>{new PHEC{AdGroup = "RegionalAdGroup", Name = "Pawnee"}});
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(new List<User>{regionalUser});
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task OrderQueryableAsync_DoesNotFindNonCaseManagerOrRegionalUsers()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("Frank");
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(new List<User>
            {
                new User {DisplayName = "Frank Ma"},
                CreateDefaultCaseManager("Rufus Hound", "Rufus", "Hound", new List<CaseManagerTbService>())
            });
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public async Task SearchWithTwoKeywords_ReturnsUsersContainingBothInDisplayName()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("Test User");
            var caseManagers = new List<User>
            {
                CreateDefaultCaseManager(displayName: "Test User"),
                CreateDefaultCaseManager(displayName: "Tester User"),
                CreateDefaultCaseManager(displayName: "Test Userer"),
                CreateDefaultCaseManager(displayName: "TestUser"),
                CreateDefaultCaseManager(displayName: "Test Bob"),
                CreateDefaultCaseManager(displayName: "Bob User"),
                CreateDefaultCaseManager(displayName: "Foo Bar")
            };
            var expectedResults = new List<User> { caseManagers[0], caseManagers[1], caseManagers[2], caseManagers[3] };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(caseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(expectedResults, results);
        }
        
        [Fact]
        public async Task SearchWithTwoKeywords_ReturnsUsersContainingBothInEitherName()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("Test User");
            var caseManagers = new List<User>
            {
                CreateDefaultCaseManager(givenName: "Test", familyName: "User"),
                CreateDefaultCaseManager(givenName: "Tester", familyName: "User"),
                CreateDefaultCaseManager(givenName: "Test", familyName: "Userer"),
                CreateDefaultCaseManager(givenName: "Test", familyName: "Bob"),
                CreateDefaultCaseManager(givenName: "Bob", familyName: "User"),
                CreateDefaultCaseManager(givenName: "Foo", familyName: "Bar")
            };
            var expectedResults = new List<User> { caseManagers[0], caseManagers[1], caseManagers[2] };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());
            _mockUserRepository.Setup(u => u.GetOrderedUsers()).ReturnsAsync(caseManagers);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(expectedResults, results);
        }
        
        private static User CreateDefaultCaseManager(string displayName = null, string givenName = null,
            string familyName = null,
            List<CaseManagerTbService> caseManagerTbServices = null)
        {
            var defaultTbService = new TBService { Name = "Test service", Code = "TTT001" };
            return new User
            {
                DisplayName = displayName,
                GivenName = givenName,
                FamilyName = familyName,
                CaseManagerTbServices = caseManagerTbServices ?? new List<CaseManagerTbService>
                {
                    new CaseManagerTbService
                    {
                        TbService = defaultTbService,
                        TbServiceCode = defaultTbService.Code,
                        CaseManagerId = 1
                    }
                },
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
