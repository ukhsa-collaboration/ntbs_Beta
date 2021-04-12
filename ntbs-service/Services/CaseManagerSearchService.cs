using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ICaseManagerSearchService
    {
        Task<(IList<User> caseManagers, int count)> OrderAndPaginateQueryableAsync(
            string searchKeyword,
            PaginationParametersBase paginationParameters);
    }

    public class CaseManagerSearchService : ICaseManagerSearchService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserRepository _userRepository;

        public CaseManagerSearchService(IReferenceDataRepository referenceDataRepository, IUserRepository userRepository)
        {
            _referenceDataRepository = referenceDataRepository;
            _userRepository = userRepository;
        }

        public async Task<(IList<User> caseManagers, int count)> OrderAndPaginateQueryableAsync(
            string searchKeyword,
            PaginationParametersBase paginationParameters)
        {
            var searchKeywords = searchKeyword.Split(" ")
                .Where(x => !x.IsNullOrEmpty())
                .Select(s => s.ToLower()).ToList();
            
            var allPhecs = await _referenceDataRepository.GetAllPhecs();
            var filteredPhecs = allPhecs.Where(phec => searchKeywords.Any(s => phec.Name.ToLower().Contains(s)));

            var caseManagersAndRegionalUsers = (await _referenceDataRepository.GetAllCaseManagersOrdered())
                .Concat(_userRepository.GetUserQueryable().ToList()
                    .Where(user => user.AdGroups != null && allPhecs.Any(phec => user.AdGroups.Contains(phec.AdGroup))))
                .Distinct()
                .OrderBy(u => u.DisplayName);

            // This query is too complex to translate to sql, so we explicitly work on an in-memory list.
            // The size of the directory should make this ok.
            var filteredCaseManagersAndRegionalUsers = caseManagersAndRegionalUsers.Where(c =>
                    searchKeywords.Any(s => c.FamilyName != null && c.FamilyName.ToLower().Contains(s))
                    || searchKeywords.Any(s => c.GivenName != null && c.GivenName.ToLower().Contains(s))
                    || searchKeywords.Any(s => c.DisplayName != null && c.DisplayName.ToLower().Contains(s))
                    || c.CaseManagerTbServices.Any(x =>
                        searchKeywords.Any(s => x.TbService.Name.ToLower().Contains(s)))
                    || (c.AdGroups != null && filteredPhecs.Any(phec => c.AdGroups.Split(",").Contains(phec.AdGroup))))
                .ToList();

            return (GetPaginatedItems(filteredCaseManagersAndRegionalUsers, paginationParameters), filteredCaseManagersAndRegionalUsers.Count);
        }

        private IList<T> GetPaginatedItems<T>(IEnumerable<T> items,
            PaginationParametersBase paginationParameters)
        {
            return items
                .Skip(paginationParameters.Offset ?? 0)
                .Take(paginationParameters.PageSize)
                .ToList();
        }
    }
}
