using System.Threading.Tasks;
using Moq;
using ntbs_service.Models.Entities;
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
        [InlineData("RR/MDR/XDR", false, true, false)]
        [InlineData("RR/MDR/XDR", true, true, false)]
        [InlineData("NonMDR", false, false, true)]
        [InlineData("NonMDR", true, true, false)]
        public void CreateOrDismissMdrAlert(string drugResistance, 
            bool isMdrPlanned,
            bool shouldCreateAlert, 
            bool shouldDismissAlert)
        {
            // Arrange
            var notification = new Notification
            {
                NotificationId = 1,
                DrugResistanceProfile = new DrugResistanceProfile
                {
                    Species = "Random Species", DrugResistanceProfileString = drugResistance
                },
                ClinicalDetails = new ClinicalDetails
                {
                    IsMDRTreatment = isMdrPlanned
                }
            };
            
            // Act
            EnhancedSurveillanceAlertsService.CreateOrDismissMdrAlert(notification);
            
            // Assert
            var numberOfCallsToCreate = shouldCreateAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x =>x.AddUniqueAlertAsync(It.IsAny<MdrAlert>()), numberOfCallsToCreate);
            
            var numberOfCallsToDismiss = shouldDismissAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR), numberOfCallsToDismiss);
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
                    Species = drugSpecies, DrugResistanceProfileString = "Random string"
                }
            };
            
            // Act
            EnhancedSurveillanceAlertsService.CreateOrDismissMBovisAlert(notification);
            
            // Assert
            var numberOfCallsToCreate = shouldCreateAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x =>x.AddUniqueAlertAsync(It.IsAny<MBovisAlert>()), numberOfCallsToCreate);
            
            var numberOfCallsToDismiss = shouldDismissAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMBovis), numberOfCallsToDismiss);
        }
    }
}
