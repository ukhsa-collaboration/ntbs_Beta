using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests
{
    // TODO: Complete tests for this page, c.f. ClinicalDetailsPageTests
    public class PatientPageTests : TestRunnerBase
    {
        protected override string PageRoute
        {
            get { return Routes.Patient; }
        }

        public PatientPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task Post_ReturnsPageWithModelErrors_IfModelNotValid()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var pageContent = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                // TODO: Add all the fields that can lead to model errors
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Patient.FamilyName"] = "111",
            };

            // Act
            var result = await SendFormWithData(pageContent, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.QuerySelector("span[id='family-name-error']").TextContent);
        }
    }
}
