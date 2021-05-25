using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using Serilog;

namespace ntbs_service.Services
{
    public interface IAdImportService
    {
        Task RunCaseManagerImportAsync();
    }

    public class AdImportService : IAdImportService
    {
        private readonly IAdDirectoryServiceFactory _adDirectoryServiceFactory;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IAdUserService _adUserService;

        public AdImportService(IAdDirectoryServiceFactory adDirectoryServiceFactory,
            IReferenceDataRepository referenceDataRepository,
            IAdUserService adUserService)
        {
            _adDirectoryServiceFactory = adDirectoryServiceFactory;
            _referenceDataRepository = referenceDataRepository;
            _adUserService = adUserService;
        }

        public async Task RunCaseManagerImportAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            using (var adDirectoryService = _adDirectoryServiceFactory.Create())
            {
                var usersInAd = adDirectoryService.LookupUsers(tbServices);
                await _adUserService.AddAndUpdateUsers(usersInAd);
            }
        }
    }
}
