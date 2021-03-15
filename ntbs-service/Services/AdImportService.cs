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
                var users = adDirectoryService.LookupUsers(tbServices).ToList();
                foreach (var (user, tbServicesMatchingGroups) in users)
                {
                    Log.Information($"Updating user {user.Username}");
                    await _userRepository.AddOrUpdateUser(user, tbServicesMatchingGroups);
                }
                
                var ntbsUsersNotInAd = this._userRepository.GetUserIQueryable()
                    .Where(user => !users.Select(u => u.user.Username)
                        .Contains(user.Username)).ToList();
                foreach (var user in ntbsUsersNotInAd)
                {
                    Log.Information($"Updating user {user.Username}");
                    user.IsActive = false;
                    user.AdGroups = null;
                    await _userRepository.AddOrUpdateUser(user, user.CaseManagerTbServices.Select(cmtb => cmtb.TbService));
                }
            }
        }
    }
}
