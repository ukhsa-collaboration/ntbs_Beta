using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IAdDirectoryServiceFactory
    {
        IAdDirectoryService Create();
    }

    public class AdDirectoryServiceServiceFactory : IAdDirectoryServiceFactory
    {
        private readonly LdapSettings _ldapSettings;
        private readonly AdOptions _adOptions;

        public AdDirectoryServiceServiceFactory(
            IOptions<LdapSettings> LdapSettings,
            IOptions<AdOptions> adfsOptions)
        {
            _ldapSettings = LdapSettings.Value;
            _adOptions = adfsOptions.Value;
        }

        public IAdDirectoryService Create()
        {
            return new AdDirectoryService(_ldapSettings, _adOptions);
        }
    }
}
