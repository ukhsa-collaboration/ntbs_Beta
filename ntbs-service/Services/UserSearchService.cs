﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface IUserSearchService
    {
        Task<IList<User>> OrderQueryableAsync(string searchKeyword);
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
            string searchKeyword)
        {
            var searchKeywords = searchKeyword.Split(" ")
                .Where(x => !x.IsNullOrEmpty())
                .Select(s => s.ToLower()).ToList();

            var allPhecs = await _referenceDataRepository.GetAllPhecs();
            var filteredPhecs = allPhecs
                .Where(phec => searchKeywords.All(s => phec.Name.ToLower().Contains(s)));

            var caseManagersAndRegionalUsers = (await _userRepository.GetOrderedUsers())
                .Where(u => u.IsActive && (u.CaseManagerTbServices.Any()
                            || (u.AdGroups != null && allPhecs.Any(phec => u.AdGroups.Split(",").Contains(phec.AdGroup)))));

            // This query is too complex to translate to sql, so we explicitly work on an in-memory list.
            // The size of the directory should make this ok.
            var filteredCaseManagersAndRegionalUsers = caseManagersAndRegionalUsers.Where(c =>
                    searchKeywords.All(s =>
                        c.FamilyName != null && c.FamilyName.ToLower().Contains(s)
                            || c.GivenName != null && c.GivenName.ToLower().Contains(s))
                    || searchKeywords.All(s => c.DisplayName != null && c.DisplayName.ToLower().Contains(s))
                    || c.CaseManagerTbServices.Any(x =>
                        searchKeywords.All(s => x.TbService.Name.ToLower().Contains(s)))
                    || filteredPhecs.Any(phec => c.AdGroups != null && c.AdGroups.Split(",").Contains(phec.AdGroup)))
                .ToList();

            return filteredCaseManagersAndRegionalUsers;
        }

        // private IList<T> GetPaginatedItems<T>(IEnumerable<T> items,
        //     PaginationParametersBase paginationParameters)
        // {
        //     return items
        //         .Skip(paginationParameters.Offset ?? 0)
        //         .Take(paginationParameters.PageSize)
        //         .ToList();
        // }
    }
}
