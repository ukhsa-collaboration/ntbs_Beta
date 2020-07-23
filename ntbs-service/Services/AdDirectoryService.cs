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
        private readonly LdapSettings _settings;
        private readonly AdfsOptions _adOptions;

        // ReSharper disable once UnusedMember.Global - constructor needed for mocking in tests
        public AdDirectoryService() { }

        private readonly LdapConnection _connection;

        public AdDirectoryService(LdapSettings settings, AdfsOptions adOptions)
        {
            _settings = settings;
            _adOptions = adOptions;
            _connection = new LdapConnection();
            Log.Information($"Connecting to AD: {settings.AdAddressName}:{settings.Port}");
            _connection.Connect(settings.AdAddressName, settings.Port);
            var withOrWithout = string.IsNullOrEmpty(settings.Password) ? "without" : "with";
            Log.Information($"Binding: {settings.UserIdentifier} {withOrWithout} a password");
            _connection.Bind(GetDistinguishedName(settings.UserIdentifier), settings.Password);
            if (_connection.Bound)
            {
                Log.Information("Bind completed");
            }
            else
            {
                var bindingException = new ApplicationException("Binding to LDAP failed");
                Log.Error(bindingException, "Aborting LDAP connection");
                throw bindingException;
            }
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
            var searchResults = Search(_settings.BaseDomain,
                LdapConnection.SCOPE_SUB,
                // The sequence of numbers sets the filter to search group hierarchy, not just direct membership:
                // https://stackoverflow.com/a/6202683/2363767
                $"(&(objectClass=user)(memberof:1.2.840.113556.1.4.1941:={distinguishedName}))",
                null,
                false,
                new LdapSearchConstraints {ReferralFollowing = true});
            
            // We don't want to use LdapSearchResults directly as Enumerable, since it can throw on `Next` 
            while (searchResults.HasMore())
            {
                var nextResult = searchResults.Next();
                if (nextResult != null)
                {
                    yield return nextResult;
                }
            }
        }

        private LdapSearchResults Search(string @base,
            int scope,
            string filter,
            string[] attrs,
            bool typesOnly,
            LdapSearchConstraints cons)
        {
            Log.Information("Getting all directory entries with the following config:\n" +
                            $"base: {@base}\n" +
                            $"scope: {scope}\n" +
                            $"filter: {filter}\n" +
                            $"attrs: {attrs}\n" +
                            $"typesOnly: {typesOnly}\n" +
                            $"cons: {cons}\n"
            );
            return _connection.Search(
                @base,
                scope,
                filter,
                attrs,
                typesOnly,
                cons);
        }

        private string GetDistinguishedName(string baseUserGroup)
        {
            return string.IsNullOrWhiteSpace(baseUserGroup)
                ? null
                : $"CN={baseUserGroup},CN=Users,{_settings.BaseDomain}";
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
