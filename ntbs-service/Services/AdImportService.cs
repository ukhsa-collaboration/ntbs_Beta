using System.Threading.Tasks;
using ntbs_service.DataAccess;

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
        private readonly IUserRepository _userRepository;

        public AdImportService(IAdDirectoryServiceFactory adDirectoryServiceFactory,
            IReferenceDataRepository referenceDataRepository,
            IUserRepository userRepository)
        {
            _adDirectoryServiceFactory = adDirectoryServiceFactory;
            _referenceDataRepository = referenceDataRepository;
            _userRepository = userRepository;
        }

        public async Task RunCaseManagerImportAsync()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            using (var adDirectoryService = _adDirectoryServiceFactory.Create())
            {
                foreach (var (user, tbServicesMatchingGroups) in adDirectoryService.LookupUsers(tbServices))
                {
                    await _userRepository.AddOrUpdateUser(user, tbServicesMatchingGroups);
                }
            }
        }
    }
}
