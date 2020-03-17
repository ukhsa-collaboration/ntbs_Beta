using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class NotificationSummaryTests : TestRunnerBase
    {
        public NotificationSummaryTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }
        
        private static string PageRoute(string notificationId) => $"/NotificationSummary/{notificationId}";

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.DENOTIFIED_ID)]
        [InlineData(Utilities.NEW_ID)]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsErrorIfInvalidId(int attemptedId)
        {
            // Act
            var response = await Client.GetAsync(PageRoute(attemptedId.ToString()));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("The NTBS ID does not match an existing ID in the system", result);
        }

        [Fact]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsErrorIfIdNotInteger()
        {
            // Arrange
            var formData = new Dictionary<string, string> {["value"] = "1e1"};

            // Act
            var response = await Client.GetAsync(PageRoute("1e1"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("The NTBS ID must be an integer", result);
        }

        [Fact]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsNotificationInfoIfValidId()
        {
            // Act
            var response = await Client.GetAsync(PageRoute(Utilities.NOTIFIED_ID.ToString()));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name", result);
            Assert.Contains("Dob", result);
        }
    }
}
