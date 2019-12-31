using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{

    public interface IAdImportService
    {
        Task RunCaseManagerImport();
    }
    public class AdImportService : IAdImportService
    {
        private readonly IAdDirectoryFactory _adDirectoryFactory;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserRepository _userRepository;

        private static readonly Regex DistinguishedNameRegex = new Regex("(CN=)(Global.NIS.NTBS[^,]+)(,.*)");

        public AdImportService(IAdDirectoryFactory adDirectoryFactory, IReferenceDataRepository referenceDataRepository, IUserRepository userRepository)
        {
            _adDirectoryFactory = adDirectoryFactory;
            _referenceDataRepository = referenceDataRepository;
            _userRepository = userRepository;
        }

        // get those values from settings
        public async Task RunCaseManagerImport()
        {
            var tbServices = await _referenceDataRepository.GetAllTbServicesAsync();
            using (var adDirectoryService = _adDirectoryFactory.Create())
            {
                foreach (var directoryEntry in adDirectoryService.GetAllDirectoryEntries())
                {
                    var userName = adDirectoryService.GetUsername(directoryEntry);
                    if (userName == null)
                    {
                        continue;
                    }

                    var userPrincipal = adDirectoryService.GetUserPrincipal(userName);
                    var user = new User
                    {
                        Username = userPrincipal.UserPrincipalName,
                        GivenName = userPrincipal.GivenName,
                        FamilyName = userPrincipal.Surname,
                        DisplayName = userPrincipal.DisplayName,
                        IsActive = adDirectoryService.IsUserEnabled(directoryEntry)
                    };

                    user.SetAdGroups(GetAdGroups(adDirectoryService, directoryEntry));
                    var filteredTbServices = tbServices.Where(tb => user.AdGroupNames.Contains(tb.ServiceAdGroup));
                    user.IsCaseManager = filteredTbServices.Any();

                    await _userRepository.AddOrUpdateUser(user, filteredTbServices);
                }  
            }
        }

        private static List<string> GetAdGroups(IAdDirectoryService adDirectoryService, DirectoryEntry directoryEntry)
        {
            var adGroups = new List<string>();
            // The more natural way to obtain membership groups would have been to use user.GetAuthorizationGroups()
            // However, all operations trying to fetch AD groups failed with an error "PrincipalOperationException: Information about the domain could not be retrieved"
            // This is caused by not being in the same network as the AD server. None of the suggested workarounds worked, so fetching this via this property instead
            foreach (var distinguishedName in adDirectoryService.GetDistinguishedGroupNames(directoryEntry))
            {
                var match = DistinguishedNameRegex.Match(distinguishedName);
                if (!match.Success)
                {
                    continue;
                }
                adGroups.Add(match.Groups[2].Captures[0].Value);
            }
            return adGroups;
        }
    }
}
