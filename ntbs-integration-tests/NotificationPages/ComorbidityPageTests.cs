using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class ComorbidityPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Comorbidities;

        public ComorbidityPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["ComorbidityDetails.DiabetesStatus"] = "Yes",
                ["ComorbidityDetails.HepatitisBStatus"] = "No",
                ["ComorbidityDetails.HepatitisCStatus"] = "Unknown",
                ["ComorbidityDetails.LiverDiseaseStatus"] = "No"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.Immunosuppression, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("diabetes-radio-button-Yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("hepatitis-b-radio-button-No")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("hepatitis-c-radio-button-Unknown")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("liver-radio-button-No")).IsChecked);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("renal-radio-button-Yes")).IsChecked);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("renal-radio-button-No")).IsChecked);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("renal-radio-button-Unknown")).IsChecked);
        }
    }
}
