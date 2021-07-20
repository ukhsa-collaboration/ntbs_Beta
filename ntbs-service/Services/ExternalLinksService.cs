using Microsoft.Extensions.Configuration;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IExternalLinksService
    {
        string GetReportingPageUrl();
        string GetSharePointHomePageUrl();
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
