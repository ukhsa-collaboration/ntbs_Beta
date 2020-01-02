using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IAdDirectoryFactory
    {
        IAdDirectoryService Create();
    }
    
    public class AdDirectoryServiceFactory : IAdDirectoryFactory
    {
        private readonly AdConnectionSettings adConnectionSettings;

        public AdDirectoryServiceFactory(IOptions<AdConnectionSettings> options)
        {
            adConnectionSettings = options.Value;
        }

        public IAdDirectoryService Create()
        {
            return new AdDirectoryService(adConnectionSettings);
        }
    }
}
