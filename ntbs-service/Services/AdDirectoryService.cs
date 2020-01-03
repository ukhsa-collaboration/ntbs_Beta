using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Novell.Directory.Ldap;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using Serilog;

namespace ntbs_service.Services
{
    public interface IAdDirectoryService : IDisposable
    {
        /// <summary>
        /// Returns NTBS users found in the external directory
        /// </summary>
        /// <param name="tbServices"> TB Services to match user memberships against </param>
        /// <returns> (user, tbServicesMatchingGroups) tuple, where
        ///     - user is an NTBS user object populated with data found in the external directory
        ///     - tbServicesMatchingGroups are TbService objects that correspond to user's memberships as case manager
        /// </returns>
        IEnumerable<(User user, List<TBService> tbServicesMatchingGroups)> LookupUsers(IList<TBService> tbServices);
    }

    public class AdDirectoryService : IAdDirectoryService
    {
        private readonly AdfsOptions _adOptions;

        // ReSharper disable once UnusedMember.Global - constructor needed for mocking in tests
        public AdDirectoryService() { }

        private readonly LdapConnection _connection;

        public AdDirectoryService(LdapConnectionSettings settings, AdfsOptions adOptions)
        {
            _adOptions = adOptions;
            _connection = new LdapConnection();
            _connection.Connect(settings.AdAddressName, settings.Port);
            _connection.Bind(GetDistinguishedName(settings.UserIdentifier), settings.Password);
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        public IEnumerable<(User user, List<TBService> tbServicesMatchingGroups)> LookupUsers(
            IList<TBService> tbServices)
        {
            return GetAllDirectoryEntries()
                .Select(result => BuildUser(result, tbServices))
                .ToList();
        }

        public virtual IEnumerable<LdapEntry> GetAllDirectoryEntries()
        {
            var distinguishedName = GetDistinguishedName(_adOptions.BaseUserGroup);
            // The sequence of numbers sets the filter to search group hierarchy, not just direct membership:
            // https://stackoverflow.com/a/6202683/2363767
            var filter = $"(&(objectClass=user)(memberof:1.2.840.113556.1.4.1941:={distinguishedName}))";
            var searchResults = _connection.Search(
                _adOptions.BaseDomain,
                LdapConnection.SCOPE_SUB,
                filter,
                null,
                false);
            
            // We don't want to use LdapSearchResults directly as Enumerable, since it can throw on `Next` 
            while (searchResults.HasMore())
            {
                LdapEntry nextResult = null;
                try
                {
                     nextResult = searchResults.Next();
                }
                catch (LdapReferralException e)
                {
                    Log.Warning("Ldap result skipped", e);
                }

                if (nextResult != null)
                {
                    yield return nextResult;
                }
            }
        }

        private string GetDistinguishedName(string baseUserGroup)
        {
            return $"CN={baseUserGroup},CN=Users,{_adOptions.BaseDomain}";
        }

        private static (User user, List<TBService> tbServicesMatchingGroups) BuildUser(
            LdapEntry searchResult, IList<TBService> tbServices)
        {
            var attributes = searchResult.getAttributeSet();
            var userAccountControl = Convert.ToInt32(
                attributes.getAttribute("userAccountControl").StringValue);
            var groups = GetAdGroups(attributes.getAttribute("memberOf").StringValues);
            var tbServicesMatchingGroups = tbServices
                .Where(tb => groups.Contains(tb.ServiceAdGroup))
                .ToList();
            var user = new User
            {
                Username = attributes.getAttribute("userPrincipalName").StringValue,
                GivenName = attributes.getAttribute("givenName").StringValue,
                FamilyName = attributes.getAttribute("sn").StringValue,
                DisplayName = attributes.getAttribute("displayName").StringValue,
                IsActive = IsUserEnabled(userAccountControl),
                AdGroups = string.Join(",", groups),
                IsCaseManager = tbServicesMatchingGroups.Any()
            };
            return (user, tbServicesMatchingGroups);
        }

        // Example of the distinguished name format:
        // "CN=Global.NIS.NTBS.Service_Nottingham,CN=Users,DC=ntbs,DC=phe,DC=com"
        // We are interested in the group names, e.g. Global.NIS.NTBS.Service_Nottingham 
        private static readonly Regex DistinguishedNameRegex = new Regex("(CN=)(Global.NIS.NTBS[^,]+)(,.*)");

        private static List<string> GetAdGroups(IEnumerator distinguishedNames)
        {
            return distinguishedNames.ToIEnumerable()
                .Cast<object>()
                .Select(distinguishedName => DistinguishedNameRegex.Match((string)distinguishedName))
                .Where(match => match.Success)
                .Select(match => match.Groups[2].Captures[0].Value)
                .ToList();
        }

        private static bool IsUserEnabled(int userAccountControl)
        {
            // See the ACCOUNTDISABLE property on
            // https://support.microsoft.com/en-gb/help/305144/how-to-use-useraccountcontrol-to-manipulate-user-account-properties
            return !Convert.ToBoolean(userAccountControl & 2);
        }
    }
}
