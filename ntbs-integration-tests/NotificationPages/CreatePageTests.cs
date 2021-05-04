using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class CreatePageTests : TestRunnerNotificationBase
    {

        public CreatePageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetCreate_ReturnsRedirect()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.NhsUserForAbingdonAndPermitted)
                .CreateClientWithoutRedirects())
            {
                // Act
                var response = await client.GetAsync(RouteHelper.GetCreateNotificationPath());

                // Assert
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            }
        }

        [Fact]
        public async Task GetCreate_ReturnsNotification_WithDefaultDrugResistanceProfile()
        {
            // Arrange
            using (var client = Factory.WithUserAuth(TestUser.NhsUserForAbingdonAndPermitted)
                .CreateClientWithoutRedirects())
            {
                // Act
                var createResponse = await client.GetAsync(RouteHelper.GetCreateNotificationPath());

                // Assert
                var editPage = await Client.GetAsync(createResponse.Headers.Location);
                var editDocument = await GetDocumentAsync(editPage);

                // TODO NTBS-2246: use a better way of selecting the drug resistance profile value, such as
                // querying HTML id attributes.
                var drugResistanceValue = editDocument
                    .QuerySelectorAll("div.notification-banner-body div.bold-label")
                    .FirstOrDefault(e => e.InnerHtml == " Drug resistance profile ")
                    ?.NextElementSibling
                    .InnerHtml;

                Assert.Equal(" No result ", drugResistanceValue);
            }
        }
    }
}
