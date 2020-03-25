using Microsoft.AspNetCore.Http;
using ntbs_service.Models;

namespace ntbs_service.Helpers
{
    public static class BreadcrumbsHelper
    {
        private const string LabelKey = "TopLevelLocationLabel";
        private const string UrlKey = "TopLevelLocationUrl";

        public static void SetTopLevelBreadcrumb(ISession session, string label, string url)
        {
            session.SetString(LabelKey, label);
            session.SetString(UrlKey, url);
        }
        
        public static Breadcrumb GetTopLevelBreadcrumb(ISession session)
        {
            return new Breadcrumb
            {
                Label = session.GetString(LabelKey),
                Url = session.GetString(UrlKey)
            };
        }
    }
}
