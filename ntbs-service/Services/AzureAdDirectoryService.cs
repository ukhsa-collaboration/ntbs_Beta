using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace ntbs_service.Services
{
    public interface IAzureAdDirectoryService 
    {
        Task<string> ResolveUserMemberGroupNameFromId(string id);
    }

    public class AzureAdDirectoryService : IAzureAdDirectoryService
    {
        private readonly IGraphServiceClient _graphServiceClient;

        public AzureAdDirectoryService() {}

        public AzureAdDirectoryService(IGraphServiceClient graphServiceClient)
        {
            this._graphServiceClient = graphServiceClient;
        }

        public async Task<string> ResolveUserMemberGroupNameFromId(string id) {

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
    }
}
