using AngleSharp.Html.Dom;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.Helpers
{
    public static class HtmlDocumentExtensions
    {
        public static string GetError(this IHtmlDocument document, string input) => 
            document?.QuerySelector($"span[id='{input}-error']")?.TextContent;
            
        public static void AssertErrorMessage(this IHtmlDocument resultDocument, string inputName, string expectedMessage)
        {
            var expected = HtmlDocumentHelpers.FullErrorMessage(expectedMessage);
            var actual = resultDocument.GetError(inputName);
            Assert.Equal(expected, actual);
        }
    }
}