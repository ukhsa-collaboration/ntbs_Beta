using Microsoft.ApplicationInsights.AspNetCore;

namespace ntbs_service.Services
{
    // This is based on https://stackoverflow.com/a/49458076
    // They allow us to avoid the dependency injection issues that happen if Application Insights is conditionally
    // injected

    public class BlankJavaScriptSnippet : IJavaScriptSnippet
    {
        public string FullScript => string.Empty;
    }
}
