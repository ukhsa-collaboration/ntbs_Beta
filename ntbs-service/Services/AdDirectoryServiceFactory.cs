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
        private readonly AdfsOptions _adfsOptions;

        public AdDirectoryServiceServiceFactory(
            IOptions<LdapSettings> LdapSettings,
            IOptions<AdfsOptions> adfsOptions)
        {
            this._ldapSettings = LdapSettings.Value;
            this._adfsOptions = adfsOptions.Value;
        }

        public IAdDirectoryService Create()
        {
            return new AdDirectoryService(_ldapSettings, _adfsOptions);
        }
    }
}
