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
            }
        }
    }
}
