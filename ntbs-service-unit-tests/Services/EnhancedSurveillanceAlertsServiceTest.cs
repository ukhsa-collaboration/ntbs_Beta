using Moq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class EnhancedSurveillanceAlertsServiceTest
    {
        private readonly Mock<IAlertService> mockAlertService;
        private readonly EnhancedSurveillanceAlertsService EnhancedSurveillanceAlertsService;

        public EnhancedSurveillanceAlertsServiceTest()
        {
            mockAlertService = new Mock<IAlertService>();
            EnhancedSurveillanceAlertsService = new EnhancedSurveillanceAlertsService(mockAlertService.Object);
        }

        [Theory]
        [InlineData("RR/MDR/XDR", false, null, true, false)]
        [InlineData("RR/MDR/XDR", false, Status.Unknown, false, true)]
        [InlineData("RR/MDR/XDR", false, Status.No, false, true)]
        [InlineData("RR/MDR/XDR", false, Status.Yes, false, true)]
        [InlineData("RR/MDR/XDR", true, null, true, false)]
        [InlineData("RR/MDR/XDR", true, Status.Unknown, false, true)]
        [InlineData("RR/MDR/XDR", true, Status.No, false, true)]
        [InlineData("RR/MDR/XDR", true, Status.Yes, false, true)]
        [InlineData("NonMDR", false, null, false, true)]
        [InlineData("NonMDR", false, Status.Unknown, false, true)]
        [InlineData("NonMDR", false, Status.No, false, true)]
        [InlineData("NonMDR", false, Status.Yes, false, true)]
        [InlineData("NonMDR", true, null, true, false)]
        [InlineData("NonMDR", true, Status.Unknown, false, true)]
        [InlineData("NonMDR", true, Status.No, false, true)]
        [InlineData("NonMDR", true, Status.Yes, false, true)]
        public void CreateOrDismissMdrAlert(string drugResistance,
            bool isMdrPlanned,
            Status? mdrExposureStatus,
            bool shouldCreateAlert,
            bool shouldDismissAlert)
        {
            // Arrange
            var notification = new Notification
            {
                NotificationId = 1,
                DrugResistanceProfile = new DrugResistanceProfile
                {
                    Species = "Random Species",
                    DrugResistanceProfileString = drugResistance
                },
                ClinicalDetails = new ClinicalDetails
                {
                    TreatmentRegimen = isMdrPlanned ? TreatmentRegimen.MdrTreatment : TreatmentRegimen.StandardTherapy
                },
                MDRDetails = new MDRDetails
                {
                    ExposureToKnownCaseStatus = mdrExposureStatus
                }
            };

            // Act
            EnhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);

            // Assert
            var numberOfCallsToCreate = shouldCreateAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.AddUniqueAlertAsync(It.IsAny<MdrAlert>()), numberOfCallsToCreate);

            var numberOfCallsToDismiss = shouldDismissAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(
                x => x.DismissMatchingAlertAsync<MdrAlert>(notification.NotificationId, AuditService.AuditUserSystem),
                numberOfCallsToDismiss);
        }

        [Theory]
        [InlineData("M. bovis", true, false)]
        [InlineData("Non M. bovis", false, true)]
        public void CreateOrDismissMBovisAlert(string drugSpecies,
            bool shouldCreateAlert,
            bool shouldDismissAlert)
        {
            // Arrange
            var notification = new Notification
            {
                NotificationId = 1,
                DrugResistanceProfile = new DrugResistanceProfile
                {
                    Species = drugSpecies,
                    DrugResistanceProfileString = "Random string"
                }
            };

            // Act
            EnhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);

            // Assert
            var numberOfCallsToCreate = shouldCreateAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.AddUniqueAlertAsync(It.IsAny<MBovisAlert>()), numberOfCallsToCreate);

            var numberOfCallsToDismiss = shouldDismissAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(
                x => x.DismissMatchingAlertAsync<MBovisAlert>(notification.NotificationId,
                    AuditService.AuditUserSystem),
                numberOfCallsToDismiss);
        }
    }
}
