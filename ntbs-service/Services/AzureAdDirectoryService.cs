using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Graph;
using ntbs_service.Helpers;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using Serilog;

namespace ntbs_service.Services
{
    public interface IAzureAdDirectoryService
    {
        /// <summary>
        /// Returns NTBS users found in Azure Active Directory directory
        /// </summary>
        /// <param name="tbServices"> TB Services to match user memberships against </param>
        /// <returns> (user, tbServicesMatchingGroups) tuple, where
        ///     - user is an NTBS user object populated with data found in Azure Active Directory
        ///     - tbServicesMatchingGroups are TbService objects that correspond to user's memberships as case manager
        /// </returns>
        Task<IList<(Models.Entities.User user, IList<TBService> tbServicesMatchingGroups)>>
            LookupUsers(IList<TBService> tbServices);

        Task<IList<Claim>> BuildRoleClaimsForUser(string userPrincipalName);
    }

    public class AzureAdDirectoryService : IAzureAdDirectoryService
    {
        private readonly IGraphServiceClient _graphServiceClient;
        private readonly AdOptions _adOptions;

        public AzureAdDirectoryService() { }

        public AzureAdDirectoryService(IGraphServiceClient graphServiceClient, AdOptions adOptions)
        {
            _graphServiceClient = graphServiceClient;
            _adOptions = adOptions;
        }

        public async Task<IList<(Models.Entities.User user, IList<TBService> tbServicesMatchingGroups)>>
            LookupUsers(IList<TBService> tbServices)
        {
            var userWithTbServiceGroups =
                new List<(Models.Entities.User user, IList<TBService> tbServicesMatchingGroups)>();
            var graphUsers = await GetAllDirectoryEntries();

            // Ensure that users are unique
            var uniqueUsers = graphUsers
                .GroupBy(user => user.UserPrincipalName)
                .Select(grouping => grouping.First());

            var newUsers = await Task.WhenAll(uniqueUsers.Select(async user =>
            {
                var groupNames = await BuildGroups(user);
                return BuildUser(user, tbServices, groupNames);
            }));
            userWithTbServiceGroups.AddRange(newUsers);

            return userWithTbServiceGroups;
        }

        public async Task<IList<Claim>> BuildRoleClaimsForUser(string userPrincipalName)
        {
            var roleClaims = new List<Claim>();

            var foundUsers = await _graphServiceClient
                .Users
                .Request()
                .Filter($"UserPrincipalName eq '{userPrincipalName}' or Mail eq '{userPrincipalName}'")
                .GetAsync();
            var foundUser = foundUsers.FirstOrDefault();

            if (foundUser == null)
            {
                return roleClaims;
            }

            // Get groups for user
            var groupsAndDirectoryRolesRequest = _graphServiceClient
                .Users[foundUser.Id]
                .TransitiveMemberOf
                .Request();

            while (groupsAndDirectoryRolesRequest != null)
            {
                var groupsAndDirectoryRoles = await groupsAndDirectoryRolesRequest.GetAsync();

                var currentRoleClaimNames = roleClaims.Select(claim => claim.Value);
                var newRoleClaims = groupsAndDirectoryRoles
                    .OfType<Microsoft.Graph.Group>()
                    .Select(group => group.DisplayName)
                    .Where(groupName => IsGroupNameValidAndUnique(groupName, currentRoleClaimNames))
                    .Select(groupName => new Claim(ClaimTypes.Role, groupName));
                roleClaims.AddRange(newRoleClaims);

                groupsAndDirectoryRolesRequest = groupsAndDirectoryRoles.NextPageRequest;
            }

            return roleClaims;
        }

        private async Task<IList<Microsoft.Graph.User>> GetAllDirectoryEntries()
        {
            var directoryEntries = new List<Microsoft.Graph.User>();

            var baseGroups = await _graphServiceClient
                .Groups
                .Request()
                .Filter($"displayName eq '{_adOptions.BaseUserGroup}'")
                .Select("id, displayName, description")
                .Top(1)
                .GetAsync();

            var baseGroup = baseGroups.FirstOrDefault(g => g.DisplayName == _adOptions.BaseUserGroup);

            if (baseGroup == null)
            {
                Log.Error($"Could not find base group {_adOptions.BaseUserGroup} in Azure AD");
                return directoryEntries;
            }

            var usersInRootGroup = await GetUsersInGroup(baseGroup.Id);
            directoryEntries.AddRange(usersInRootGroup);

            var groupMembersRequest = _graphServiceClient
                .Groups[baseGroup.Id]
                .Members
                .Request();

            // The request is paginated, so iterate over each page
            while (groupMembersRequest != null)
            {
                var groupMembers = await groupMembersRequest.GetAsync();

                var newDirectoryEntries = (await Task.WhenAll(
                    groupMembers
                        .OfType<Microsoft.Graph.Group>()
                        .Select(group => GetUsersInGroup(group.Id)))
                    ).SelectMany(entry => entry);
                directoryEntries.AddRange(newDirectoryEntries);

                groupMembersRequest = groupMembers.NextPageRequest;
            }

            return directoryEntries;
        }

        private async Task<IList<Microsoft.Graph.User>> GetUsersInGroup(string groupId)
        {
            var listOfUsers = new List<Microsoft.Graph.User>();

            var groupMembersRequest = _graphServiceClient
                .Groups[groupId]
                .Members
                .Request()
                .Select("id, displayName");

            while (groupMembersRequest != null)
            {
                var groupMembers = await groupMembersRequest.GetAsync();

                var graphUsers = groupMembers
                    .OfType<Microsoft.Graph.User>()
                    .Select(user => _graphServiceClient.Users[user.Id]
                    .Request()
                    .Select("accountenabled, id, displayName, givenName, surname, userPrincipalName, mail")
                    .GetAsync());
                listOfUsers.AddRange(await Task.WhenAll(graphUsers));

                groupMembersRequest = groupMembers.NextPageRequest;
            }

            return listOfUsers;
        }

        private async Task<IList<string>> BuildGroups(Microsoft.Graph.User graphUser)
        {
            var groupNames = new List<string>();

            var groupsAndDirectoryRolesRequest = _graphServiceClient
                .Users[graphUser.Id]
                .MemberOf
                .Request()
                .Select("id, displayName");

            while (groupsAndDirectoryRolesRequest != null)
            {
                var groupsAndDirectoryRoles = await groupsAndDirectoryRolesRequest.GetAsync();

                var newGroupNames = groupsAndDirectoryRoles
                    .OfType<Microsoft.Graph.Group>()
                    .Select(group => group.DisplayName)
                    .Where(groupName => IsGroupNameValidAndUnique(groupName, groupNames));
                groupNames.AddRange(newGroupNames);

                groupsAndDirectoryRolesRequest = groupsAndDirectoryRoles.NextPageRequest;
            }

            return groupNames;
        }

        private bool IsGroupNameValidAndUnique(string groupName, IEnumerable<string> groupNames)
        {
            return !string.IsNullOrEmpty(groupName)
                   && groupName.StartsWith(_adOptions.BaseUserGroup)
                   && !groupNames.Any(group => group.Equals(groupName, StringComparison.CurrentCultureIgnoreCase));
        }

        private (Models.Entities.User user, IList<TBService> tbServicesMatchingGroups) BuildUser(
            Microsoft.Graph.User graphUser,
            IEnumerable<TBService> tbServices,
            IEnumerable<string> groupNames)
        {

            var tbServicesMatchingGroups = tbServices
                .Where(tb => groupNames.Contains(tb.ServiceAdGroup))
                .ToList();

            var userName = graphUser.UserPrincipalName;
            if (IsUserExternal(graphUser))
            {
                userName = !string.IsNullOrEmpty(graphUser.Mail) ? graphUser.Mail : graphUser.UserPrincipalName;
            }

            var displayName = !string.IsNullOrEmpty(graphUser.DisplayName)
                ? NameFormattingHelper.FormatDisplayName(graphUser.DisplayName)
                : $"{graphUser.GivenName} {graphUser.Surname}";

            var user = new Models.Entities.User
            {
                Username = userName,
                GivenName = graphUser.GivenName,
                FamilyName = graphUser.Surname,
                DisplayName = displayName,
                IsActive = graphUser.AccountEnabled.HasValue && graphUser.AccountEnabled.Value,
                AdGroups = string.Join(",", groupNames),
                IsCaseManager = tbServicesMatchingGroups.Any()
            };

            return (user, tbServicesMatchingGroups);
        }

        private static bool IsUserExternal(Microsoft.Graph.User user)
        {
            return user.UserPrincipalName.Contains("#EXT#");
        }
    }
}
