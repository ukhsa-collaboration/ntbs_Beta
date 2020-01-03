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
        private readonly LdapConnectionSettings LdapConnectionSettings;
        private readonly AdfsOptions _adfsOptions;

        public AdDirectoryServiceServiceFactory(
            IOptions<LdapConnectionSettings> ldapConnectionSettings,
            IOptions<AdfsOptions> adfsOptions)
        {
            this.LdapConnectionSettings = ldapConnectionSettings.Value;
            this._adfsOptions = adfsOptions.Value;
        }

        public IAdDirectoryService Create()
        {
            return new AdDirectoryService(LdapConnectionSettings, _adfsOptions);
        }
    }
}
