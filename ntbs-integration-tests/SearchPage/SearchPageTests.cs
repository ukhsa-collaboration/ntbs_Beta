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
                ["SearchParameters.PartialNotificationDate.Year"] = "1999",
                ["SearchParameters.Postcode"] = "$$$"
            };

            // Act
            var result = await SendGetFormWithData(pageContent, formData, PageRoute);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("id", "Id filter can only contain digits 0-9, the symbol - and spaces");
            resultDocument.AssertErrorMessage("family-name", "Family name can only contain letters and the symbols ' - . ,");
            resultDocument.AssertErrorMessage("given-name", "Given name can only contain letters and the symbols ' - . ,");
            resultDocument.AssertErrorMessage("postcode", "Postcode can only contain letters, numbers and the symbols ' - . ,");
            resultDocument.AssertErrorMessage("dob", "Date of birth does not have a valid date selection");
            resultDocument.AssertErrorMessage("notification-date", "Notification date does not have a valid date selection");
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

            Assert.Contains("#1", resultDocument.QuerySelector("a[id='notification-banner-id']").TextContent);
        }
    }
}
