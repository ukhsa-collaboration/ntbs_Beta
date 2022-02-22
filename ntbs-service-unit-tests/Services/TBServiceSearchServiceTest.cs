using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class TBServiceSearchServiceTest
    {
        private static readonly List<TBService> DefaultTBServices = new()
        {
            CreateTbService( "Test TBServiceOne"),
            CreateTbService( "Test TBServiceTwo"),
            CreateTbService("Test TBServiceThree")
        };

        private readonly ITBServiceSearchService _service;
        private readonly Mock<IReferenceDataRepository> _mockReferenceDataRepository;

        public TBServiceSearchServiceTest()
        {
            _mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            _service = new TBServiceSearchService(_mockReferenceDataRepository.Object);
        }

        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyList()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("");
            _mockReferenceDataRepository.Setup(r => r.GetAllActiveTbServicesAsync()).ReturnsAsync(new List<TBService>());

            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);

            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyListWhenThereAreNoMatches()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("abcdefg");
        
            _mockReferenceDataRepository.Setup(r => r.GetAllActiveTbServicesAsync()).ReturnsAsync(DefaultTBServices);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsTBServicesWithNameMatchingSearch()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("ServiceT");
            var expectedResults = new List<TBService> { DefaultTBServices[1], DefaultTBServices[2] };
        
            _mockReferenceDataRepository.Setup(u => u.GetAllActiveTbServicesAsync()).ReturnsAsync(DefaultTBServices);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(expectedResults, results);
        }

        [Fact]
        public async Task SearchWithTwoKeywords_ReturnsServicesWithBothInName()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("service Test");
            var expectedResults = new List<TBService> { DefaultTBServices[0], DefaultTBServices[1] ,DefaultTBServices[2] };
            
            _mockReferenceDataRepository.Setup(u => u.GetAllActiveTbServicesAsync()).ReturnsAsync(DefaultTBServices);

            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(expectedResults, results);
        }

        private static TBService CreateTbService(string name) => new() {Name = name};
    }
}
