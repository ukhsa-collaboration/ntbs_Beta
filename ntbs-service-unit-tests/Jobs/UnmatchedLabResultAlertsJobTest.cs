using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Jobs;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Jobs
{
    public class UnmatchedLabResultAlertsJobTest
    {
        private readonly Mock<ISpecimenService> _mockSpecimenService;
        private readonly Mock<IAlertRepository> _mockAlertRepository;
        private readonly Mock<IAlertService> _mockAlertService;

        private readonly UnmatchedLabResultAlertsJob _unmatchedLabResultAlertsJob;

        public UnmatchedLabResultAlertsJobTest()
        {
            _mockSpecimenService = new Mock<ISpecimenService>();
            _mockAlertRepository = new Mock<IAlertRepository>();
            _mockAlertService = new Mock<IAlertService>();
            
            _unmatchedLabResultAlertsJob = new UnmatchedLabResultAlertsJob(
                _mockSpecimenService.Object,
                _mockAlertRepository.Object,
                _mockAlertService.Object);
        }

        [Fact]
        public async Task OnRun_WithAlertsAndMatchesInParity_CallsExpectedMethodsWithEmptyParams()
        {
            // Arrange
            _mockAlertRepository.Setup(r => r.CloseAlertRangeAsync(It.IsAny<IEnumerable<Alert>>()));
            _mockAlertService.Setup(r =>
                r.CreateAlertsForUnmatchedLabResults(It.IsAny<IEnumerable<SpecimenMatchPairing>>()));

            var potentialMatches =
                Task.FromResult((IEnumerable<SpecimenMatchPairing>)new List<SpecimenMatchPairing>
                {
                    new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "1"},
                    new SpecimenMatchPairing {NotificationId = 2, ReferenceLaboratoryNumber = "1"},
                    new SpecimenMatchPairing {NotificationId = 3, ReferenceLaboratoryNumber = "1"},
                    new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "2"}
                });
            _mockSpecimenService.Setup(s => s.GetAllSpecimenPotentialMatchesAsync()).Returns(potentialMatches);

            var currentAlerts =
                Task.FromResult((IList<UnmatchedLabResultAlert>)new List<UnmatchedLabResultAlert>
                {
                    new UnmatchedLabResultAlert {AlertId = 1, NotificationId = 1, SpecimenId = "1"},
                    new UnmatchedLabResultAlert {AlertId = 2, NotificationId = 2, SpecimenId = "1"},
                    new UnmatchedLabResultAlert {AlertId = 3, NotificationId = 3, SpecimenId = "1"},
                    new UnmatchedLabResultAlert {AlertId = 4, NotificationId = 1, SpecimenId = "2"}
                });
            _mockAlertRepository.Setup(r => r.GetAllOpenUnmatchedLabResultAlertsAsync()).Returns(currentAlerts);

            // Act

            await _unmatchedLabResultAlertsJob.Run(JobCancellationToken.Null);

            // Assert
            _mockAlertRepository.Verify(s => s.CloseAlertRangeAsync(new List<Alert>()));
            _mockAlertService.Verify(s => s.CreateAlertsForUnmatchedLabResults(new List<SpecimenMatchPairing>()));
        }

        [Fact]
        public async Task OnRun_WithAlertsAndMatchesOutOfParity_CallsExpectedMethodsWithExpectedParams()
        {
            // Arrange
            _mockAlertRepository.Setup(r => r.CloseAlertRangeAsync(It.IsAny<IEnumerable<Alert>>()));
            _mockAlertService.Setup(r =>
                r.CreateAlertsForUnmatchedLabResults(It.IsAny<IEnumerable<SpecimenMatchPairing>>()));

            var potentialMatches =
                Task.FromResult((IEnumerable<SpecimenMatchPairing>)new List<SpecimenMatchPairing>
                {
                    new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "1"},
                    new SpecimenMatchPairing {NotificationId = 2, ReferenceLaboratoryNumber = "1"},
                    new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "2"},
                    new SpecimenMatchPairing {NotificationId = 4, ReferenceLaboratoryNumber = "3"}
                });
            _mockSpecimenService.Setup(s => s.GetAllSpecimenPotentialMatchesAsync()).Returns(potentialMatches);

            var currentAlerts =
                Task.FromResult((IList<UnmatchedLabResultAlert>)new List<UnmatchedLabResultAlert>
                {
                    new UnmatchedLabResultAlert {AlertId = 1, NotificationId = 1, SpecimenId = "1"},
                    new UnmatchedLabResultAlert {AlertId = 2, NotificationId = 2, SpecimenId = "1"},
                    new UnmatchedLabResultAlert {AlertId = 3, NotificationId = 3, SpecimenId = "1"},
                    new UnmatchedLabResultAlert {AlertId = 4, NotificationId = 4, SpecimenId = "1"},
                });
            _mockAlertRepository.Setup(r => r.GetAllOpenUnmatchedLabResultAlertsAsync()).Returns(currentAlerts);

            // Act
            await _unmatchedLabResultAlertsJob.Run(JobCancellationToken.Null);

            // Assert
            _mockAlertRepository.Verify(s =>
                s.CloseAlertRangeAsync(It.Is<IEnumerable<Alert>>(n => 
                    n.All(alert => 
                        alert.AlertId == 3 || alert.AlertId == 4))));
            
            _mockAlertService.Verify(s =>
                s.CreateAlertsForUnmatchedLabResults(new List<SpecimenMatchPairing>
                {
                    new SpecimenMatchPairing {NotificationId = 1, ReferenceLaboratoryNumber = "2"},
                    new SpecimenMatchPairing {NotificationId = 4, ReferenceLaboratoryNumber = "3"}
                }));
        }
    }
}
