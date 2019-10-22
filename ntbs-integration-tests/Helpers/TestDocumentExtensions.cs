using AngleSharp.Html.Dom;
using ntbs_service.Helpers;

namespace ntbs_integration_tests.Helpers
{
    public static class HtmlDocumentExtensions
    {
        public static string GetError(this IHtmlDocument document, string input) => 
            document?.QuerySelector($"span[id='{input}-error']")?.TextContent;
    }
}