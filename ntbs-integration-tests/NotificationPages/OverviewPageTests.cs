using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    // TODO: Complete tests for this page
    public class OverviewPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Overview;

        public OverviewPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        public static IEnumerable<object[]> OverviewRoutes()
        {
            yield return new object[] { Utilities.DRAFT_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.DENOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.NEW_ID, HttpStatusCode.NotFound };
        }

        [Theory, MemberData(nameof(OverviewRoutes))]
        public async Task GetOverviewPage_ReturnsCorrectStatusCode_DependentOnId(int id, HttpStatusCode code)
        {
            // Act
            var response = await client.GetAsync(GetPageRouteForId(id));

            // Assert
            Assert.Equal(code, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Equal(BuildRoute(Routes.Patient, id), GetRedirectLocation(response));
            }
        }

        [Fact]
        public async Task Get_ReturnsOverviewPage_ForUserWithPermission()
        {
            // Arrange
            var idToServiceCodeMap = new Dictionary<int, string>
            {
                { Utilities.NOTIFIED_ID, Utilities.PERMITTED_SERVICE_CODE }
            };
            var client = factory.WithNhsUserBuilder(idToServiceCodeMap).WithoutRedirects();

            //Act
            var response = await client.GetAsync($"{GetPageRouteForId(Utilities.NOTIFIED_ID)}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var document = await GetDocumentAsync(response);
            // There are 8 sections on the overview page
            Assert.Equal(8, document.GetElementsByClassName("notification-overview-type-and-edit-container").Length);
        }

        [Fact]
        public async Task Get_ShowsWarning_ForUserWithoutPermission()
        {
            // Arrange
            var idToServiceCodeMap = new Dictionary<int, string>
            {
                { Utilities.NOTIFIED_ID, Utilities.UNPERMITTED_SERVICE_CODE }
            };
            var client = factory.WithNhsUserBuilder(idToServiceCodeMap).WithoutRedirects();

            //Act
            var response = await client.GetAsync($"{GetPageRouteForId(Utilities.NOTIFIED_ID)}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var document = await GetDocumentAsync(response);
            Assert.Equal(ValidationMessages.UnauthorizedWarning, document.GetElementById("unauthorized-warning").FirstElementChild.TextContent);
        }
    }
}
