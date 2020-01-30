using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class DrugResistanceProfileServiceTest
    {
        private DrugResistanceProfileService mockDrpService;
        private readonly Mock<INotificationRepository> mockNotificationRepository;
        private readonly Mock<INotificationService> mockNotificationService;
        private readonly Mock<IMdrService> mockMdrService;
        private readonly Mock<IReportingService> mockReportingService;


        private static readonly DrugResistanceProfile drpWithMdr = new DrugResistanceProfile
        {
            Species = "TestSpecies", DrugResistanceProfileString = "RR/MDR/XDR"
        };        
        
        private static readonly DrugResistanceProfile drpWithoutMdr = new DrugResistanceProfile
        {
            Species = "TestSpecies", DrugResistanceProfileString = "No Mdr"
        };
        
        private readonly Notification mockNotificationWithMdr =
            new Notification {NotificationId = 1, DrugResistanceProfile = drpWithMdr };
        
        private readonly Notification mockNotificationWithoutMdr =
            new Notification {NotificationId = 2, DrugResistanceProfile = drpWithoutMdr};

        public DrugResistanceProfileServiceTest()
        {
            mockNotificationRepository = new Mock<INotificationRepository>();
            mockNotificationService = new Mock<INotificationService>();
            mockMdrService = new Mock<IMdrService>();
            mockReportingService = new Mock<IReportingService>();

            mockDrpService = new DrugResistanceProfileService(
                mockNotificationService.Object, 
                mockNotificationRepository.Object, 
                mockReportingService.Object, 
                mockMdrService.Object);
        }

        [Fact]
        public void UpdateDrugResistanceProfile_DoesNotUpdateRecordIfNotMatching()
        {
            // Arrange
            mockReportingService.Setup(x => x.GetDrugResistanceProfiles())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {3, drpWithMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(3))
                .Returns(Task.FromResult<Notification>(null));
            
            // Act
            mockDrpService.UpdateDrugResistanceProfiles();
            
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>(), false), Times.Never);
        }
        
        [Fact]
        public void UpdateDrugResistanceProfile_DoesNotUpdateRecordIfMatching()
        {
            // Arrange
            mockReportingService.Setup(x => x.GetDrugResistanceProfiles())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));

            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(It.IsAny<int>()))
                .Returns<int>((x) => Task.FromResult(new Notification { NotificationId = x, DrugResistanceProfile = new DrugResistanceProfile()}));    
            
            // Act
            mockDrpService.UpdateDrugResistanceProfiles();
            
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Exactly(2));
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>(), null), Times.Exactly(2));
        }       
        
        [Fact]
        public void UpdateDrugResistanceProfile_WhenNoUpdatedDrp_NoAlertCreated()
        {
            // Arrange
            mockReportingService.Setup(x => x.GetDrugResistanceProfiles())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithoutMdr}
                }));
            
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMdr));           
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithoutMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMdr));
            
            // Act
            mockDrpService.UpdateDrugResistanceProfiles();
            
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Never);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>(), null), Times.Never);
        }       
        
        [Fact]
        public void UpdateDrugResistanceProfile_UpdatedOnlyChangedDrp()
        {
            // Arrange
            mockReportingService.Setup(x => x.GetDrugResistanceProfiles())
                .Returns(Task.FromResult(new Dictionary<int, DrugResistanceProfile>
                {
                    {1, drpWithMdr},
                    {2, drpWithMdr}
                }));
            
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithMdr));           
            mockNotificationRepository
                .Setup(x => x.GetNotificationAsync(mockNotificationWithoutMdr.NotificationId))
                .Returns(Task.FromResult(mockNotificationWithoutMdr));
            
            // Act
            mockDrpService.UpdateDrugResistanceProfiles();
            
            mockNotificationService.Verify(x => x.UpdateDrugResistanceProfile(It.IsAny<Notification>(), It.IsAny<DrugResistanceProfile>()), Times.Once);
            mockMdrService.Verify(x => x.CreateOrDismissMdrAlert(It.IsAny<Notification>(), null), Times.Once);
        }
    }
}
