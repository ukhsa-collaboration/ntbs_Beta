using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Identity.Client;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IAzureAdDirectoryServiceFactory
    {
        IAzureAdDirectoryService Create();
    }

    public class AzureAdDirectoryServiceFactory : IAzureAdDirectoryServiceFactory
    {
        private IGraphServiceClient _graphServiceClient;
        private readonly AdOptions _adSettings;
        private readonly AzureAdOptions _azureAdSettings;

        public AzureAdDirectoryServiceFactory(
            IOptions<AdOptions> AdSettings,
            IOptions<AzureAdOptions> AzureAdSettings)
        {
            _azureAdSettings = AzureAdSettings.Value;
            _adSettings = AdSettings.Value;
        }

        public IAzureAdDirectoryService Create()
        {
            IConfidentialClientApplication clientApplication = ConfidentialClientApplicationBuilder
            .Create(_azureAdSettings.ClientId)
            .WithAuthority(_azureAdSettings.Authority)
            .WithClientSecret(_azureAdSettings.ClientSecret)
            .Build();

            var scopes = "https://graph.microsoft.com/.default";
            var authProvider = new ClientCredentialProvider(clientApplication, scopes);


            _graphServiceClient = new GraphServiceClient(authProvider);
            return new AzureAdDirectoryService(_graphServiceClient, _adSettings);
        }
    }
}
