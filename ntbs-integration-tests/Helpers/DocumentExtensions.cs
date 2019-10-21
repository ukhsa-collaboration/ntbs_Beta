using AngleSharp.Html.Dom;
using ntbs_service.Helpers;

namespace ntbs_integration_tests.Helpers
{
    public static class DocumentExtensions
    {
        public static string GetError(IHtmlDocument document, string input) => 
            document.QuerySelector($"span[id='{input}-error']").TextContent;
    }
}