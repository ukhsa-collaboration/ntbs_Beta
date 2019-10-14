using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests
{
    public class BasicEditPageTests : TestRunnerBase
    {
        public BasicEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        public static IEnumerable<object[]> EditPageRoutes()
        {
            yield return new object[] { Routes.Patient, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.Patient, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.Episode, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.Episode, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.ClinicalDetails, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.ClinicalDetails, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.ContactTracing, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.ContactTracing, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.SocialRiskFactors, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.SocialRiskFactors, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.Travel, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.Travel, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.Comorbidities, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.Comorbidities, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.Immunosuppression, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.Immunosuppression, Utilities.NEW_ID, HttpStatusCode.NotFound };
            yield return new object[] { Routes.PreviousHistory, Utilities.DRAFT_ID, HttpStatusCode.OK };
            yield return new object[] { Routes.PreviousHistory, Utilities.NEW_ID, HttpStatusCode.NotFound };
        }

        [Theory, MemberData(nameof(EditPageRoutes))]
        public async Task Get_ReturnsOkOrNotFound_DependentOnId(string route, int id, HttpStatusCode code)
        {
            // Act
            var response = await client.GetAsync($"{route}?id={id}");

            // Assert
            Assert.Equal(code, response.StatusCode);
        }
    }
}
