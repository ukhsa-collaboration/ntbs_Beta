using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
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

        public CaseManagerSearchService(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }
        
        public async Task<(IList<User> caseManagers, int count)> OrderAndPaginateQueryableAsync(
            string searchKeyword,
            PaginationParametersBase paginationParameters)
        {
            var searchKeywords = searchKeyword.Split(" ").Where(x => !x.IsNullOrEmpty()).ToList();
            var caseManagersQueryable = _referenceDataRepository
                .GetAllCaseManagersQueryable()
                .Where(c => c.CaseManagerTbServices.Any(x => 
                                searchKeywords.Any(s => x.TbService.Name.ToLower().Contains(s.ToLower()))) 
                            || searchKeywords.Any(s => c.FamilyName.ToLower().Contains(s.ToLower())) 
                            || searchKeywords.Any(s => c.GivenName.ToLower().Contains(s.ToLower())));

            var caseManagers = await GetPaginatedItemsAsync(caseManagersQueryable, paginationParameters);
            var count = await caseManagersQueryable.CountAsync();
            return (caseManagers, count);
        }
        
        private async Task<IList<T>> GetPaginatedItemsAsync<T>(IQueryable<T> items, 
            PaginationParametersBase paginationParameters)
        {
            return await items
                .Skip(paginationParameters.Offset ?? 0)
                .Take(paginationParameters.PageSize)
                .ToListAsync();
        }
    }
}
