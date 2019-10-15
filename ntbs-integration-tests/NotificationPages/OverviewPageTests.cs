using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    // TODO: Complete tests for this page
    public class OverviewPageTests : TestRunnerBase
    {
        protected override string PageRoute
        {
            get { return Routes.Overview; }
        }

        public OverviewPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        public static IEnumerable<object[]> OverviewRoutes()
        {
            yield return new object[] { Utilities.DRAFT_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NOTIFIED_ID, HttpStatusCode.OK };
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
                Assert.Equal(BuildRoute(Routes.Patient, Utilities.DRAFT_ID), GetRedirectLocation(response));
            }
        }
    }
}
