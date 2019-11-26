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
        public SearchPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public const string PageRoute = "/Search";

        [Fact]
        public async Task GetSearch_ReturnsPageWithModelErrors_IfSearchNotValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
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
            resultDocument.AssertErrorMessage("id", ValidationMessages.NumberFormat);
            resultDocument.AssertErrorMessage("family-name", ValidationMessages.StandardStringFormat);
            resultDocument.AssertErrorMessage("given-name", ValidationMessages.StandardStringFormat);
            resultDocument.AssertErrorMessage("postcode", ValidationMessages.StandardStringWithNumbersFormat);
            resultDocument.AssertErrorMessage("dob", ValidationMessages.InvalidDate("Date of birth"));
            resultDocument.AssertErrorMessage("notification-date", ValidationMessages.InvalidDate("Notification date"));
        }


        [Fact]
        public async Task GetSearch_ReturnsPageWithMatchingResult_IfSearchValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
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
