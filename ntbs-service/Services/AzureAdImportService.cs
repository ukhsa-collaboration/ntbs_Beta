using System.Threading.Tasks;
using ntbs_service.DataAccess;

namespace ntbs_service.Services
{
    public class AzureAdImportService : IAdImportService
    {
        private readonly IAzureAdDirectoryServiceFactory _azureAdDirectoryServiceFactory;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IAdUserService _adUserService;

        public AzureAdImportService(IAzureAdDirectoryServiceFactory azureAdDirectoryServiceFactory,
            IReferenceDataRepository referenceDataRepository,
            IAdUserService adUserService)
        {
            _azureAdDirectoryServiceFactory = azureAdDirectoryServiceFactory;
            _referenceDataRepository = referenceDataRepository;
            _adUserService = adUserService;
        }

        public async Task RunCaseManagerImportAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            var azureAdDirectoryService = _azureAdDirectoryServiceFactory.Create();

            var usersInAd = (await azureAdDirectoryService.LookupUsers(tbServices));
            await _adUserService.AddAndUpdateUsers(usersInAd);
        }
    }
}
