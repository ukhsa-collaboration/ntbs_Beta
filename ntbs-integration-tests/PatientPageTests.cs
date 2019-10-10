using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests
{
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
            var response = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var pageContent = await GetDocumentAsync(response);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Patient.FamilyName"] = "111",
                ["Patient.SexId"] = "1",
            };

            // Act
            var result = await SendFormWithData(pageContent, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            Assert.True(resultDocument.QuerySelector("span[id='family-name-error']").TextContent.Contains(ValidationMessages.StandardStringFormat));
        }

        [Fact]
        public async Task Post_RedirectsToNextPage_IfModelValid()
        {
            
        }
    }
}
