using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Enums;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IExternalLinksService
    {
        string GetReportingPageUrl();
        string GetSharePointHomePageUrl();
        string GetSharePointFaqPageUrl();
        string GetSharePointFaqPageWithAnchor(PermissionLevel permissionLevel);
        string GetClusterReport(string clusterId);
    }

    public class ExternalLinksService : IExternalLinksService
    {
        private readonly ExternalLinks _externalLinks;

        public ExternalLinksService(IConfiguration configuration)
        {
            _externalLinks = new ExternalLinks();
            configuration.GetSection(Constants.ExternalLinks).Bind(_externalLinks);
        }

        public string GetReportingPageUrl() => _externalLinks.ReportingOverview;

        public string GetSharePointHomePageUrl() => _externalLinks.SharePointHomePage;
        
        public string GetSharePointFaqPageUrl() => _externalLinks.SharePointFAQPage;

        public string GetSharePointFaqPageWithAnchor(PermissionLevel permissionLevel)
        {
            return permissionLevel switch
            {
                PermissionLevel.ReadOnly => $"{_externalLinks.SharePointFAQPage}#why-do-i-not-have-permission-to-edit-a-record",
                PermissionLevel.SharedWith => $"{_externalLinks.SharePointFAQPage}#why-can-i-only-edit-the-contact-tracing-details-of-a-record",
                PermissionLevel.None => $"{_externalLinks.SharePointFAQPage}#why-can’t-i-view-the-full-details-of-a-record",
                _ => null
            };
        }


        public string GetClusterReport(string clusterId)
        {
            const string clusterReportReplacementSymbol = "<CLUSTER_ID>";
            var clusterReportBase = _externalLinks.ClusterReport;

            return string.IsNullOrEmpty(clusterReportBase)
                ? null
                : clusterReportBase.Replace(clusterReportReplacementSymbol, clusterId);
        }
    }
}
