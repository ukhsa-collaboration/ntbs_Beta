using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IAdDirectoryService : IDisposable
    {
        IEnumerable<DirectoryEntry> GetAllDirectoryEntries();
        string GetUsername(DirectoryEntry de);
        bool IsUserEnabled(DirectoryEntry de);
        UserPrincipal GetUserPrincipal(string userName);
        IEnumerable<string> GetDistinguishedGroupNames(DirectoryEntry directoryEntry);
    }

    public class AdDirectoryService : IAdDirectoryService
    {
        private readonly PrincipalContext context;
        private readonly PrincipalSearcher searcher;

        public AdDirectoryService(AdConnectionSettings adConnectionSettings)
        {
            context = new PrincipalContext(
                ContextType.Domain,
                adConnectionSettings.DomainName,
                adConnectionSettings.UserName,
                adConnectionSettings.Password);
            searcher = new PrincipalSearcher(new UserPrincipal(context));
        }

        public void Dispose()
        {
            searcher.Dispose();
            context.Dispose();
        }

        public virtual IEnumerable<DirectoryEntry> GetAllDirectoryEntries()
        {
            return searcher.FindAll()
                .Select(result => result.GetUnderlyingObject() as DirectoryEntry);
        }

        public virtual string GetUsername(DirectoryEntry de)
        {
            return (string)de.Properties["userPrincipalName"].Value;
        }
        public virtual bool IsUserEnabled(DirectoryEntry de)
        {
            // https://support.microsoft.com/en-gb/help/305144/how-to-use-useraccountcontrol-to-manipulate-user-account-properties
            // ReSharper disable once StringLiteralTypo
            var uac = (int)de.Properties["useraccountcontrol"].Value;
            return !Convert.ToBoolean(uac & 2);
        }

        public virtual UserPrincipal GetUserPrincipal(string userName)
        {
            return UserPrincipal.FindByIdentity(context, userName);
        }

        public virtual IEnumerable<string> GetDistinguishedGroupNames(DirectoryEntry directoryEntry)
        {
            foreach (var group in directoryEntry.Properties["memberOf"])
            {
                yield return group.ToString();
            }
        }
    }
}
