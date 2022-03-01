using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface IUserSearchService
    {
        Task<IList<User>> OrderQueryableAsync(List<string> searchKeywords);
    }

    public class UserSearchService : IUserSearchService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserRepository _userRepository;

        public UserSearchService(IReferenceDataRepository referenceDataRepository, IUserRepository userRepository)
        {
            _referenceDataRepository = referenceDataRepository;
            _userRepository = userRepository;
        }

        public async Task<IList<User>> OrderQueryableAsync(
            List<string> searchKeywords)
        {
            var allPhecs = await _referenceDataRepository.GetAllPhecs();

            var caseManagersAndRegionalUsers = (await _userRepository.GetOrderedUsers())
                .Where(u => u.IsActive && (u.CaseManagerTbServices.Any()
                            || (u.AdGroups != null && allPhecs.Any(phec => u.AdGroups.Split(",").Contains(phec.AdGroup)))));

            // This query is too complex to translate to sql, so we explicitly work on an in-memory list.
            // The size of the directory should make this ok.
            var filteredCaseManagersAndRegionalUsers = caseManagersAndRegionalUsers.Where(c =>
                    searchKeywords.All(s =>
                        c.FamilyName != null && c.FamilyName.ToLower().Contains(s)
                        || c.GivenName != null && c.GivenName.ToLower().Contains(s))
                    || searchKeywords.All(s => c.DisplayName != null && c.DisplayName.ToLower().Contains(s)))
                .ToList();

            return filteredCaseManagersAndRegionalUsers;
        }
    }
}
