using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.AdminPages
{
    public class CloneNotificationPageTests : TestRunnerBase
    {
        
        private const string cloneNotificationUri = "/Admin/CloneNotification";

        public CloneNotificationPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task WhenValid_ValidateGivenNameProperty_ReturnsExpectedResponse()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await Client.GetAsync(cloneNotificationUri);
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel
            {
                Key = "GivenName",
                Value = "Brett"
            };
            const string handlerPath = "ValidateProperty";
            var endpointPath = $"{cloneNotificationUri}/{handlerPath}";

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                endpointPath,
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task WhenInvalid_ValidateGivenNameProperty_ReturnsExpectedResponse()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var initialPage = await Client.GetAsync(cloneNotificationUri);
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel
            {
                Key = "GivenName",
                Value = "<><"
            };
            const string handlerPath = "ValidateProperty";
            var endpointPath = $"{cloneNotificationUri}/{handlerPath}";

            // Act
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                endpointPath,
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains(string.Format(ValidationMessages.StandardStringFormat, "Given name"), result);
        }
    }
}
