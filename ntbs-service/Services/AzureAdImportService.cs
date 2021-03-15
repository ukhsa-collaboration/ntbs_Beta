using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using Serilog;

namespace ntbs_service.Services
{
    public class AzureAdImportService : IAdImportService
    {
        private readonly IAzureAdDirectoryServiceFactory _azureAdDirectoryServiceFactory;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserRepository _userRepository;

        public AzureAdImportService(IAzureAdDirectoryServiceFactory azureAdDirectoryServiceFactory,
            IReferenceDataRepository referenceDataRepository,
            IUserRepository userRepository)
        {
            _azureAdDirectoryServiceFactory = azureAdDirectoryServiceFactory;
            _referenceDataRepository = referenceDataRepository;
            _userRepository = userRepository;
        }

        public async Task RunCaseManagerImportAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            using (var azureAdDirectoryService = _azureAdDirectoryServiceFactory.Create())
            {
                var users = await azureAdDirectoryService.LookupUsers(tbServices);
                foreach (var (user, tbServicesMatchingGroups) in users)
                {
                    Log.Information($"Updating user {user.Username}");
                    await _userRepository.AddOrUpdateUser(user, tbServicesMatchingGroups);
                }

                var ntbsUsersNotInAd = (await this._userRepository.GetUsernameDictionary()).Keys
                    .Where(username => !users.Select(u => u.user.Username).Contains(username));
                foreach (var username in ntbsUsersNotInAd)
                {
                    Log.Information($"Removing AD groups from user {username}");
                    var user = await _userRepository.GetUserByUsername(username);
                    user.IsActive = false;
                    user.AdGroups = null;
                    await _userRepository.AddOrUpdateUser(user, user.CaseManagerTbServices.Select(cmtb => cmtb.TbService));
                }
            }
        }
    }
}
