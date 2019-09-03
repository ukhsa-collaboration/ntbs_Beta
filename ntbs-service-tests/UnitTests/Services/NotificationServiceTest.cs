using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_tests.UnitTests.ntbs_service_tests
{
    public class NotificationServiceTest
    {
        private INotificationService service;
        private Mock<INotificationRepository> mockRepository;
        private Mock<NtbsContext> mockContext;

        private const int UkId = 1;
        public NotificationServiceTest()
        {
            mockRepository = new Mock<INotificationRepository>();
            mockContext = new Mock<NtbsContext>();
            service = new NotificationService(mockRepository.Object, mockContext.Object);
        }

        [Fact]
        public void TreatmentStartDate_IsSetToNullIfDidNotStartTreatmentTrue()
        {
            // Arrange
            var notification = new Notification();
            var timeline = new ClinicalTimeline() { TreatmentStartDate = DateTime.Now, DidNotStartTreatment = true };

            // Act
            service.UpdateTimelineAsync(notification, timeline);

            // Assert
            Assert.Null(timeline.TreatmentStartDate);
        }

         [Fact]
        public void DeathDate_IsSetToNullIfPostMortemFalse()
        {
            // Arrange
            var notification = new Notification();
            var timeline = new ClinicalTimeline() { DeathDate = DateTime.Now, IsPostMortem = false };

            // Act
            service.UpdateTimelineAsync(notification, timeline);

            // Assert
            Assert.Null(timeline.DeathDate);
        }
    }
}