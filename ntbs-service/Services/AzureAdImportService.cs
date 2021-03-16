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
                var users = (await azureAdDirectoryService.LookupUsers(tbServices)).ToList();
                var ntbsUsersNotInAd = this._userRepository.GetUserQueryable()
                    .Where(user => !users.Select(u => u.user.Username).Contains(user.Username)).ToList();
                var ntbsUsersNotInAdWithTbServices = ntbsUsersNotInAd.Select
                    (u => (u, u.CaseManagerTbServices.Select(cmtb => cmtb.TbService).ToList()));
                users.AddRange(ntbsUsersNotInAdWithTbServices);
                foreach (var (user, tbServicesMatchingGroups) in users)
                {
                    Log.Information($"Updating user {user.Username}");
                    if (ntbsUsersNotInAd.Select(u => u.Username).Contains(user.Username))
                    {
                        user.IsActive = false;
                        user.AdGroups = null;
                    }
                    await _userRepository.AddOrUpdateUser(user, tbServicesMatchingGroups);
                }
            }
        }
    }
}
