using System.Threading.Tasks;
using Moq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class MdrServiceTest
    {
        private readonly Mock<IAlertService> mockAlertService;
        private readonly MdrService mdrService;

        public MdrServiceTest()
        {
            mockAlertService = new Mock<IAlertService>();
            mdrService = new MdrService(mockAlertService.Object);
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
            mdrService.CreateOrDismissMdrAlert(notification);
            
            // Assert
            var numberOfCalls = shouldCreateAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x =>x.AddUniqueAlertAsync(It.IsAny<MdrAlert>()), numberOfCalls);
            
            numberOfCalls = shouldDismissAlert ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR), numberOfCalls);
        }
    }
}
