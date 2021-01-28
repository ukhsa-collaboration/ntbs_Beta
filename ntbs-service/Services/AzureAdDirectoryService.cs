using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Graph;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;

namespace ntbs_service.Services
{
    public interface IAzureAdDirectoryService : IDisposable
    {
        Task<string> ResolveGroupNameFromId(string id);
        /// <summary>
        /// Returns NTBS users found in Azure Active Directory directory
        /// </summary>
        /// <param name="tbServices"> TB Services to match user memberships against </param>
        /// <returns> (user, tbServicesMatchingGroups) tuple, where
        ///     - user is an NTBS user object populated with data found in Azure Active Directory
        ///     - tbServicesMatchingGroups are TbService objects that correspond to user's memberships as case manager
        /// </returns>
        Task<IEnumerable<(Models.Entities.User user, List<TBService> tbServicesMatchingGroups)>> LookupUsers(IList<TBService> tbServices);

        bool IsGroupAnNtbsGroup(string groupName);

    }

    public class AzureAdDirectoryService : IAzureAdDirectoryService
    {
        private readonly IGraphServiceClient _graphServiceClient;
        private readonly AzureAdOptions _adOptions;

        public AzureAdDirectoryService() {}

        public AzureAdDirectoryService(IGraphServiceClient graphServiceClient, AzureAdOptions adOptions)
        {
            this._graphServiceClient = graphServiceClient;
            this._adOptions = adOptions;
        }

        public void Dispose()
        {
            
        }

        public async Task<string> ResolveGroupNameFromId(string id) {

            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var groupName = "";

            try {
                var result = await this._graphServiceClient.Groups[id]
                    .Request()
                    .GetAsync();

                if(result != null)
                {
                    groupName = result.DisplayName;
                }


            } catch (Exception) {
                // ignore exception
            }
            
            return groupName;
        }

        public async Task<IEnumerable<(Models.Entities.User user, List<TBService> tbServicesMatchingGroups)>> LookupUsers(IList<TBService> tbServices)
        {
            IList<(Models.Entities.User user, List<TBService> tbServicesMatchingGroups)> userWithTbServiceGroups = new List<(Models.Entities.User user, List<TBService> tbServicesMatchingGroups)>();
            IEnumerable<Microsoft.Graph.User> graphUsers = await GetAllDirectoryEntries();

            // ensure that users are unique
            graphUsers = graphUsers.GroupBy(u => u.UserPrincipalName).Select(u => u.FirstOrDefault());

            foreach (var user in graphUsers){
                var groupNames = await BuildGroups(user);
                var buildUserResult = BuildUser(user, tbServices, groupNames);
                userWithTbServiceGroups.Add(buildUserResult);
            }

            return userWithTbServiceGroups;
        }

        public async virtual Task<IEnumerable<Microsoft.Graph.User>> GetAllDirectoryEntries()
        {
            var listOfDirectoryEntries = new List<Microsoft.Graph.User>();
            
            var baseGroups = await this._graphServiceClient
            .Groups
            .Request()
            .Filter($"startswith(displayName, '{this._adOptions.BaseUserGroup}')")
            .Select("id, displayName, description")
            .Top(999)
            .GetAsync();

            var baseGroup = baseGroups.FirstOrDefault(g => g.DisplayName == this._adOptions.BaseUserGroup);
            
            if(baseGroup != null){

                var groupMembers = await this._graphServiceClient.Groups[baseGroup.Id]
                    .Members
                    .Request()
                    .GetAsync();

                if(groupMembers != null) {
                    // get users from root group
                    var usersInRootGroup = await GetUsersInGroup(baseGroup.Id);
                    listOfDirectoryEntries.AddRange(usersInRootGroup);

                    foreach(var groupMember in groupMembers) {
                        try {
                            switch(groupMember.ODataType){
                                case "#microsoft.graph.group":
                                    var usersInGroup = await GetUsersInGroup(groupMember.Id);
                                     listOfDirectoryEntries.AddRange(usersInGroup);
                                break;
                                default:
                                    break;
                            }
                        }
                        catch(Exception) {
                            // ignore
                        }
                    }
                }
            }

            return listOfDirectoryEntries.ToArray();
        }

        public async virtual Task<IEnumerable<Microsoft.Graph.User>> GetUsersInGroup(string groupId)
        {
            var listOfUsers = new List<Microsoft.Graph.User>();

            try{
                var groupMembers = await this._graphServiceClient
                .Groups[groupId]
                .Members
                .Request()
                .Select("id, displayName")
                .Top(999)
                .GetAsync();
                
                if(groupMembers != null) 
                {
                    foreach(var groupMember in groupMembers) 
                    {
                        try {
                            switch(groupMember.ODataType){
                                case "#microsoft.graph.user":
                                    var graphUser = await this._graphServiceClient.Users[groupMember.Id]
                                    .Request()
                                    .Select("accountenabled, id, displayName, givenName, surname, userPrincipalName, mail")
                                    .GetAsync();
                                    listOfUsers.Add(graphUser);
                                break;
                                default:

                                break;
                            }
                        }
                        catch(Exception) {
                            // ignore
                        }
                    }
                }
            } 
            catch(Exception) {
                // ignore
            }

            return listOfUsers.ToArray();
        } 

        public bool IsUserExternal(Microsoft.Graph.User user){
            bool isExternal = user.UserPrincipalName.Contains("#EXT#");
            return isExternal;
        }

        public bool IsGroupAnNtbsGroup(string groupName){
            bool IsGroupANtbsGroup = groupName.StartsWith(this._adOptions.BaseUserGroup);
            return IsGroupANtbsGroup;
        }

        public async Task<IList<Claim>> BuildRoleClaimsForUser(string username) {
            List<Claim> groupClaims = new List<Claim>();

            try {
                var groupMembership = BuildGroups()
            } 
            catch (Exception ex) {

            }

            return groupClaims;
        }

        private async Task<IEnumerable<string>> BuildGroups(Microsoft.Graph.User graphUser){
            var groupNames = new List<string>();

            var groupIds = await this._graphServiceClient.Users[graphUser.Id]
                .MemberOf
                .Request()
                .Select("id, displayName")
                .Top(999)
                .GetAsync();

            foreach(var groupInfo in groupIds) {
                var groupName = await this.ResolveGroupNameFromId(groupInfo.Id);
                // ensure we do not add any duplicate groups or blank groups
                if(!String.IsNullOrEmpty(groupName) && IsGroupAnNtbsGroup(groupName) && !groupNames.Any(group => group.Equals(groupName, StringComparison.CurrentCultureIgnoreCase))) {
                    groupNames.Add(groupName);
                }
            }

            return groupNames;
        }

        private (Models.Entities.User user, List<TBService> tbServicesMatchingGroups) BuildUser(
            Microsoft.Graph.User graphUser, IList<TBService> tbServices, IEnumerable<string> groupNames)
        {

            var tbServicesMatchingGroups = tbServices
                .Where(tb => groupNames.Contains(tb.ServiceAdGroup))
                .ToList();

            var userName = graphUser.UserPrincipalName;
            if(IsUserExternal(graphUser)){
                userName = !String.IsNullOrEmpty(graphUser.Mail) ? graphUser.Mail : graphUser.UserPrincipalName;
            }
            
            var user = new Models.Entities.User
            {
                Username = userName,
                GivenName = graphUser.GivenName,
                FamilyName = graphUser.Surname,
                DisplayName = graphUser.DisplayName,
                IsActive = graphUser.AccountEnabled.HasValue && graphUser.AccountEnabled.Value,
                AdGroups = string.Join(",", groupNames),
                IsCaseManager = tbServicesMatchingGroups.Any()
            };

            return (user, tbServicesMatchingGroups);
        }
    }
}
