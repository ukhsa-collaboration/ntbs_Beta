using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;
using Serilog;
using User = Sentry.User;

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
            using (var azureAdDirectoryService = _azureAdDirectoryServiceFactory.Create())
            {
                var usersInAd = (await azureAdDirectoryService.LookupUsers(tbServices)).ToList();
                await _adUserService.AddAndUpdateUsers(usersInAd);
            }
        }
    }
}
