using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.SearchPage
{
    public class SearchPageTests : TestRunnerBase
    {
        public SearchPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        public const string PageRoute = "/Search";

        [Fact]
        public async Task GetSearch_ReturnsPageWithModelErrors_IfSearchNotValid()
        {
            // Arrange
            var initialPage = await client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["SearchParameters.IdFilter"] = "ABC",
                ["SearchParameters.FamilyName"] = "111",
                ["SearchParameters.GivenName"] = "111",
                ["SearchParameters.PartialDob.Day"] = "31",
                ["SearchParameters.PartialDob.Month"] = "13",
                ["SearchParameters.PartialDob.Year"] = "1899",
                ["SearchParameters.PartialNotificationDate.Day"] = "31",
                ["SearchParameters.PartialNotificationDate.Month"] = "13",
                ["SearchParameters.PartialNotificationDate.Year"] = "1899",
                ["SearchParameters.Postcode"] = "$$$",
            };

            // Act
            var result = await SendGetFormWithData(pageContent, formData, PageRoute);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            Assert.Equal(FullErrorMessage(ValidationMessages.NumberFormat), resultDocument.GetError("id"));
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.GetError("family-name"));
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.GetError("given-name"));
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringWithNumbersFormat), resultDocument.GetError("postcode"));
            Assert.Equal(FullErrorMessage(ValidationMessages.InvalidDate), resultDocument.GetError("dob"));
            Assert.Equal(FullErrorMessage(ValidationMessages.InvalidDate), resultDocument.GetError("notification-date"));
        }


        [Fact]
        public async Task GetSearch_ReturnsPageWithMatchingResult_IfSearchValid()
        {
            // Arrange
            var initialPage = await client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);
            var formData = new Dictionary<string, string>
            {
                ["SearchParameters.IdFilter"] = Utilities.DRAFT_ID.ToString()
            };

            // Act
            var result = await SendGetFormWithData(pageContent, formData, PageRoute);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            
            Assert.Equal(" #1 ", resultDocument.QuerySelector("a[id='notification-banner-id']").TextContent);
        }
    }
}
