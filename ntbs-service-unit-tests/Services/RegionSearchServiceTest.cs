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
    public class RegionSearchServiceTest
    {
        private static readonly List<PHEC> DefaultRegions = new()
        {
            CreateRegion("Test RegionOne"), 
            CreateRegion("Test RegionTwo"), 
            CreateRegion("Test RegionThree")
        };

        private readonly IRegionSearchService _service;
        private readonly Mock<IReferenceDataRepository> _mockReferenceDataRepository;

        public RegionSearchServiceTest()
        {
            _mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            _service = new RegionSearchService(_mockReferenceDataRepository.Object);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyList()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("");
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(new List<PHEC>());

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
        
            _mockReferenceDataRepository.Setup(r => r.GetAllPhecs()).ReturnsAsync(DefaultRegions);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsTBServicesWithNameMatchingSearch()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("RegionT");
            var expectedResults = new List<PHEC> { DefaultRegions[1], DefaultRegions[2] };
        
            _mockReferenceDataRepository.Setup(u => u.GetAllPhecs()).ReturnsAsync(DefaultRegions);
        
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
            var searchStrings = SearchStringHelper.GetSearchKeywords("region Test");
            var expectedResults = new List<PHEC> { DefaultRegions[0], DefaultRegions[1] ,DefaultRegions[2] };
            
            _mockReferenceDataRepository.Setup(u => u.GetAllPhecs()).ReturnsAsync(DefaultRegions);

            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(expectedResults, results);
        }
        
        private static PHEC CreateRegion(string name) => new() {Name = name};
    }

}
