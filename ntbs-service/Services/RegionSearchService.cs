using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface IRegionSearchService
    {
        Task<IList<PHEC>> OrderQueryableAsync(string searchKeyword);
    }

    public class RegionSearchService : IRegionSearchService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public RegionSearchService(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IList<PHEC>> OrderQueryableAsync(string searchKeyword)
        {
            var searchKeywords = searchKeyword.Split(" ")
                .Where(x => !x.IsNullOrEmpty())
                .Select(s => s.ToLower()).ToList();
            
            var allPhecs = await _referenceDataRepository.GetAllPhecs();

            var filteredPhecs = allPhecs
                .Where(phec => searchKeywords.All(s => phec.Name.ToLower().Contains(s)))
                .ToList();

            return filteredPhecs;
        }
    }
    
}
