using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class BasicEditPageTests : TestRunnerBase
    {
        public BasicEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsNotFound_ForNewId(string route)
        {
            // Act
            var response = await Client.GetAsync($"{route}?id={Utilities.NEW_ID}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory, MemberData(nameof(OkRouteToIdCombinations))]
        public async Task Get_ReturnsOk_ForExistingIds(string route, int id)
        {
            //Act
            var response = await Client.GetAsync($"{route}?id={id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsOk_ForNhsUserWithPermission(string route)
        {
            // Arrange
            using (var client = Factory.WithMockUserService<NhsUserService>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                 //Act
                var response = await client.GetAsync($"{route}?id={Utilities.DRAFT_ID}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsRedirect_ForNhsUserWithoutPermission(string route)
        {
            // Arrange
            using (var client = Factory.WithMockUserService<NhsUserService>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync($"{route}?id={Utilities.DRAFT_ID}");

                // Assert
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            }
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsOk_ForPheUserWithMatchingServicePermission(string route)
        {
            // Arrange
            using (var client = Factory.WithMockUserService<PheUserService>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync($"{route}?id={Utilities.DRAFT_ID}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsOk_ForPheUserWithMatchingPostcodePermission(string route)
        {
            // Arrange
            using (var client = Factory.WithMockUserService<PheUserService>()
                                        .WithNotificationAndPostcodeConnected(Utilities.DRAFT_ID, Utilities.PERMITTED_POSTCODE)
                                        .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync($"{route}?id={Utilities.DRAFT_ID}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsRedirect_ForPheUserWithoutMatchingServicePermission(string route)
        {
            // Arrange
            using (var client = Factory.WithMockUserService<PheUserService>()
                                        .WithNotificationAndTbServiceConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync($"{route}?id={Utilities.DRAFT_ID}");

                // Assert
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            }
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsRedirect_ForPheUserWithoutMatchingPostcodePermission(string route)
        {
            // Arrange
            using (var client = Factory.WithMockUserService<PheUserService>()
                                        .WithNotificationAndPostcodeConnected(Utilities.DRAFT_ID, Utilities.UNPERMITTED_POSTCODE)
                                        .CreateClientWithoutRedirects())
            {
                //Act
                var response = await client.GetAsync($"{route}?id={Utilities.DRAFT_ID}");

                // Assert
                Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            }
        }

        private static readonly List<string> Routes = new List<string>()
        {
            Helpers.Routes.Patient,
            Helpers.Routes.Episode,
            Helpers.Routes.ClinicalDetails,
            Helpers.Routes.ContactTracing,
            Helpers.Routes.SocialRiskFactors,
            Helpers.Routes.Travel,
            Helpers.Routes.Comorbidities,
            Helpers.Routes.Immunosuppression,
            Helpers.Routes.PreviousHistory
        };

        private static readonly List<int> OkNotificationIds = new List<int>()
        {
            Utilities.DRAFT_ID,
            Utilities.NOTIFIED_ID,
            Utilities.DENOTIFIED_ID
        };

        public static IEnumerable<object[]> EditPageRoutes()
        {
            return Routes.Select(route => new object[] { route });
        }

        public static IEnumerable<object[]> OkRouteToIdCombinations()
        {
            return Routes.SelectMany(route =>
                OkNotificationIds.Select(id => new object[] { route, id })
            );
        }
    }
}
