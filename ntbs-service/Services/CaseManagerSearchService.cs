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
            var searchKeywords = searchKeyword.Split(" ")
                .Where(x => !x.IsNullOrEmpty())
                .Select(s => s.ToLower()).ToList();
            var caseManagers = await _referenceDataRepository.GetAllCaseManagersQueryable()
                .ToListAsync();

            // This query is too complex to translate to sql, so we explicitly work on an in-memory list.
            // The size of the directory should make this ok.
            var filtered = caseManagers.Where(c =>
                    searchKeywords.Any(s => c.FamilyName.ToLower().Contains(s))
                    || searchKeywords.Any(s => c.GivenName.ToLower().Contains(s))
                    || c.CaseManagerTbServices.Any(x =>
                        searchKeywords.Any(s => x.TbService.Name.ToLower().Contains(s))))
                .ToList();

            return (GetPaginatedItems(filtered, paginationParameters), filtered.Count);
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
