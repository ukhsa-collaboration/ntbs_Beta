using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IAdDirectoryFactory
    {
        AdDirectoryService Create();
    }

    public class AdDirectoryService : IDisposable
    {
        private readonly PrincipalContext context;
        private readonly PrincipalSearcher searcher;

        public AdDirectoryService(AdConnectionSettings adConnectionSettings)
        {
            context = new PrincipalContext(ContextType.Domain, adConnectionSettings.DomainName, adConnectionSettings.UserName, adConnectionSettings.Password);
            searcher = new PrincipalSearcher(new UserPrincipal(context));
        }

        public void Dispose()
        {
            searcher.Dispose();
            context.Dispose();
        }

        public virtual IEnumerable<DirectoryEntry> GetAllDirectoryEntries()
        {
            foreach (var result in searcher.FindAll())
            {
                yield return result.GetUnderlyingObject() as DirectoryEntry;
            }       
        }

        public virtual string GetUsername(DirectoryEntry de)
        {
            return (string)de.Properties["userPrincipalName"].Value;
        }
        public virtual bool IsUserEnabled(DirectoryEntry de)
        {
            // https://support.microsoft.com/en-gb/help/305144/how-to-use-useraccountcontrol-to-manipulate-user-account-properties
            var uac = (int)de.Properties["useraccountcontrol"].Value;
            return !Convert.ToBoolean(uac & 2);
        }

        public virtual UserPrincipal GetUserPrincipal(string userName)
        {
            return UserPrincipal.FindByIdentity(context, userName);
        }

        public virtual IEnumerable<string> GetDistinguisedGroupNames(DirectoryEntry directoryEntry)
        {
            foreach (var group in directoryEntry.Properties["memberOf"])
            {
                yield return group.ToString();
            }
        }
    }
    public class AdDirectoryFactory : IAdDirectoryFactory
    {
        private readonly AdConnectionSettings adConnectionSettings;

        public AdDirectoryFactory(IOptions<AdConnectionSettings> options)
        {
            adConnectionSettings = options.Value;
        }

        public AdDirectoryService Create()
        {
            return new AdDirectoryService(adConnectionSettings);
        }
    }
}