using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class PreviousHistoryPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditPreviousHistory;

        public PreviousHistoryPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment_ForNotified()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");
        }
        
        [Theory]
        [InlineData(1899, "Please enter a valid year")]
        [InlineData(2040, "Previous year of diagnosis must be the current year or earlier")]
        [InlineData(1950, "Previous year of diagnosis must be later than date of birth year")]
        public async Task Post_ReturnsPageWithModelErrors_IfYearOfDiagnosisInvalid(int tbDiagnosisYear, string errorMessage)
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["PreviousTbHistory.PreviouslyHadTb"] = "true",
                ["PreviousTbHistory.PreviousTbDiagnosisYear"] = tbDiagnosisYear.ToString()
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            resultDocument.AssertErrorSummaryMessage("PreviousTbHistory-PreviousTbDiagnosisYear",
                "previous-tb-diagnosis-year", errorMessage);
        }
        
        [Fact]
        public async Task NotifiedPageHasReturnLinkToOverview()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }
    }
}
