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
    public class HospitalSearchServiceTest
    {
        private static readonly List<Hospital> DefaultHopsitals = new()
        {
            CreateHospital("Test HospitalOne"),
            CreateHospital("Test HospitalTwo"),
            CreateHospital("Test HospitalThree"),
        };

        private readonly IHospitalSearchService _service;
        private readonly Mock<IReferenceDataRepository> _mockReferenceDataRepository;

        public HospitalSearchServiceTest()
        {
            _mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            _service = new HospitalSearchService(_mockReferenceDataRepository.Object);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsEmptyList()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("");
            _mockReferenceDataRepository.Setup(r => r.GetAllActiveHospitals()).ReturnsAsync(new List<Hospital>());

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
        
            _mockReferenceDataRepository.Setup(r => r.GetAllActiveHospitals()).ReturnsAsync(DefaultHopsitals);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Empty(results);
        }
        
        [Fact]
        public async Task OrderQueryableAsync_ReturnsHospitalsWithNameMatchingSearch()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("HospitalT");
            var expectedResults = new List<Hospital> { DefaultHopsitals[1], DefaultHopsitals[2] };
        
            _mockReferenceDataRepository.Setup(r => r.GetAllActiveHospitals()).ReturnsAsync(DefaultHopsitals);
        
            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(2, results.Count);
            Assert.Equal(expectedResults, results);
        }
        
        [Fact]
        public async Task SearchWithTwoKeywords_ReturnsHospitalsWithBothInName()
        {
            // Arrange
            var searchStrings = SearchStringHelper.GetSearchKeywords("hospital Test");
            var expectedResults = new List<Hospital> { DefaultHopsitals[0], DefaultHopsitals[1] ,DefaultHopsitals[2] };
            
            _mockReferenceDataRepository.Setup(u => u.GetAllActiveHospitals()).ReturnsAsync(DefaultHopsitals);

            // Act
            var results = await _service.OrderQueryableAsync(searchStrings);
        
            // Assert
            Assert.Equal(3, results.Count);
            Assert.Equal(expectedResults, results);
        }
        
        private static Hospital CreateHospital(string name) => new() {Name = name};
    }
}
