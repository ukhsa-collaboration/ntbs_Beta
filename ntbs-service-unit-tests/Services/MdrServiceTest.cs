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
        private readonly MdrService mockMdrService;

        public MdrServiceTest()
        {
            mockAlertService = new Mock<IAlertService>();
            mockMdrService = new MdrService(mockAlertService.Object);
        }

        [Theory]
        [InlineData("RR/MDR/XDR", false)]
        [InlineData("RR/MDR/XDR", true)]
        [InlineData("NonMDR", false)]
        [InlineData("NonMDR", true)]
        public void CreateOrDismissMdrAlert_CreatOrDismissAlert(string drugResistance, bool questionnaireFilled)
        {
            // Arrange
            var notification = new Notification
            {
                NotificationId = 1,
                DrugResistanceProfile = new DrugResistanceProfile
                {
                    Species = "Random Species", DrugResistanceProfileString = drugResistance
                },
                MDRDetails = new MDRDetails
                {
                    ExposureToKnownCaseStatus = questionnaireFilled ? Status.Unknown : (Status?) null
                }
            };
            
            // Act
            mockMdrService.CreateOrDismissMdrAlert(notification);
            
            // Assert
            var numberOfCalls = (drugResistance == "RR/MDR/XDR" && !questionnaireFilled) ? Times.Once() : Times.Never();
            mockAlertService.Verify(x =>x.AddUniqueAlertAsync(It.IsAny<MdrAlert>()), numberOfCalls);
            
            numberOfCalls = (drugResistance != "RR/MDR/XDR" && !questionnaireFilled) ? Times.Once() : Times.Never();
            mockAlertService.Verify(x => x.DismissMatchingAlertAsync(notification.NotificationId, AlertType.EnhancedSurveillanceMDR), numberOfCalls);
        }
    }
}
