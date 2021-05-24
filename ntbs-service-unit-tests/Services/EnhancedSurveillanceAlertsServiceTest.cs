using System.Threading.Tasks;
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
        public async Task CreateOrDismissMdrAlert(string drugResistance,
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
            await EnhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);

            // Assert
            var numberOfCallsToCreate = shouldCreateAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.AddUniqueAlertAsync(It.IsAny<MdrAlert>()), numberOfCallsToCreate);

            var numberOfCallsToDismiss = shouldDismissAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(
                x => x.DismissMatchingAlertAsync<MdrAlert>(notification.NotificationId, AuditService.AuditUserSystem),
                numberOfCallsToDismiss);
        }

        [Theory]
        [InlineData("M. bovis", false, null, null, null, null, true, false)]
        [InlineData("M. bovis", true, null, null, null, null, false, true)]
        [InlineData("M. bovis", false, null, null, null, Status.Yes, true, false)]
        [InlineData("M. bovis", false, null, null, Status.Yes, null, true, false)]
        [InlineData("M. bovis", false, null, Status.Yes, null, null, true, false)]
        [InlineData("M. bovis", false, Status.Yes, null, null, null, true, false)]
        [InlineData("M. bovis", false, Status.No, Status.No, Status.No, null, true, false)]
        [InlineData("M. bovis", false, Status.No, Status.No, null, Status.No, true, false)]
        [InlineData("M. bovis", false, Status.No, null, Status.No, Status.No, true, false)]
        [InlineData("M. bovis", false, null, Status.No, Status.No, Status.No, true, false)]
        [InlineData("M. bovis", true, null, Status.No, Status.No, Status.No, false, true)]
        [InlineData("M. bovis", false, Status.No, Status.No, Status.No, Status.No, false, true)]
        [InlineData("M. bovis", false, Status.Yes, Status.No, Status.Unknown, Status.No, false, true)]
        [InlineData("M. bovis", false, Status.Yes, Status.Yes, Status.Yes, Status.Yes, false, true)]
        [InlineData("M. bovis", false, Status.Unknown, Status.Unknown, Status.Unknown, Status.Unknown, false, true)]
        [InlineData("Non M. bovis", false, null, null, null, null, false, true)]
        [InlineData("Non M. bovis", true, null, null, null, null, false, true)]
        [InlineData("Non M. bovis", false, null, Status.No, Status.Unknown, Status.Yes, true, false)]
        [InlineData("Non M. bovis", true, null, Status.No, Status.Unknown, Status.Yes, false, true)]
        [InlineData("Non M. bovis", false, Status.Yes, Status.Yes, Status.Yes, Status.Yes, false, true)]
        [InlineData("Non M. bovis", false, Status.No, Status.No, Status.No, Status.No, false, true)]
        [InlineData("Non M. bovis", false, Status.Unknown, Status.Unknown, Status.Unknown, Status.Unknown, false, true)]
        public async Task CreateOrDismissMBovisAlert(string drugSpecies,
            bool notificationClosed,
            Status? hasExposureToKnownCases,
            Status? hasUnpasteurisedMilkConsumption,
            Status? hasOccupationExposure,
            Status? hasAnimalExposure,
            bool shouldCreateAlert,
            bool shouldDismissAlert)
        {
            // Arrange
            var notification = new Notification
            {
                NotificationId = 1,
                NotificationStatus = notificationClosed ? NotificationStatus.Closed : NotificationStatus.Notified,
                DrugResistanceProfile = new DrugResistanceProfile
                {
                    Species = drugSpecies,
                    DrugResistanceProfileString = "Random string",
                },
                MBovisDetails = new MBovisDetails
                {
                    ExposureToKnownCasesStatus = hasExposureToKnownCases,
                    UnpasteurisedMilkConsumptionStatus = hasUnpasteurisedMilkConsumption,
                    OccupationExposureStatus = hasOccupationExposure,
                    AnimalExposureStatus = hasAnimalExposure,
                }
            };

            // Act
            await EnhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);

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
