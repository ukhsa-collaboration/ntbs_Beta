using Microsoft.Extensions.Configuration;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IReportingLinksService
    {
        string GetReportingPageUrl();
    }

    public class ReportingLinksService : IReportingLinksService
    {
        private readonly ExternalLinks _externalLinks;

        public ReportingLinksService(IConfiguration configuration)
        {
            _externalLinks = new ExternalLinks();
            configuration.GetSection(Constants.ExternalLinks).Bind(_externalLinks);
        }

        public string GetReportingPageUrl() => _externalLinks.ReportingOverview;
    }
}
