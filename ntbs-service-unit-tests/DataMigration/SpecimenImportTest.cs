using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Server;
using Moq;
using ntbs_service.DataMigration;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.DataMigration
{
    public class SpecimenImportTest
    {
        private readonly ISpecimenImportService _specimenImportService;

        private readonly Mock<IImportLogger> _logger = new Mock<IImportLogger>();
        private readonly Mock<ISpecimenService> _specimenService = new Mock<ISpecimenService>();
        private readonly ImportResult _importResult = new ImportResult("John Doe");

        private readonly PerformContext _performContext = null;
        private readonly string _requestId = "request1";

        public SpecimenImportTest()
        {
            _specimenImportService = new SpecimenImportService(_logger.Object, _specimenService.Object);
        }

        [Fact]
        public async Task ImportReferenceLabResultsAsync_ImportsResults()
        {
            // Arrange
            var notifications = new[] {
                new Notification { NotificationId = 101, ETSID = "1" },
                new Notification { NotificationId = 102, ETSID = "2" }
            };
            var specimenMatches = new[] { ("1", "Reference1"), ("2", "Reference2"), ("2", "Reference3") };

            _specimenService.Setup(s =>
                    s.GetLegacyReferenceLaboratoryMatches(
                        It.Is<IEnumerable<string>>(l => l.SequenceEqual(new[] { "1", "2" }))))
                .Returns(Task.FromResult(specimenMatches.AsEnumerable()));

            _specimenService.Setup(s =>
                    s.MatchSpecimenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(true));

            // Act
            await _specimenImportService.ImportReferenceLabResultsAsync(_performContext, _requestId, notifications,
                _importResult);

            // Assert
            Assert.Empty(_importResult.ValidationErrors);
            _specimenService.Verify(
                s => s.MatchSpecimenAsync(101, "Reference1", AuditService.AuditUserSystem, true), Times.Once);
            _specimenService.Verify(
                s => s.MatchSpecimenAsync(102, "Reference2", AuditService.AuditUserSystem, true), Times.Once);
            _specimenService.Verify(
                s => s.MatchSpecimenAsync(102, "Reference3", AuditService.AuditUserSystem, true), Times.Once);
        }

        [Fact]
        public async Task ImportReferenceLabResultsAsync_RecordsErrors()
        {
            // Arrange
            var notifications = new[] {
                new Notification { NotificationId = 101, ETSID = "1" },
                new Notification { NotificationId = 102, ETSID = "2" }
            };
            var specimenMatches = new[] { ("1", "Reference1"), ("2", "Reference2") };

            _specimenService.Setup(s =>
                    s.GetLegacyReferenceLaboratoryMatches(
                        It.Is<IEnumerable<string>>(l => l.SequenceEqual(new[] { "1", "2" }))))
                .Returns(Task.FromResult(specimenMatches.AsEnumerable()));

            _specimenService.Setup(s =>
                    s.MatchSpecimenAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(true));

            _specimenService.Setup(s =>
                    s.MatchSpecimenAsync(102, "Reference2", It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(false));

            // Act
            await _specimenImportService.ImportReferenceLabResultsAsync(_performContext, _requestId, notifications,
                _importResult);

            // Assert
            Assert.Collection(_importResult.ValidationErrors, errorPair => Assert.Equal("2", errorPair.Key));
        }
    }
}
