using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface ITBServiceSearchService
    {
        Task<IList<TBService>> OrderQueryableAsync(List<string> searchKeywords);
    }

    class TbServiceSearchService : ITBServiceSearchService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public TbServiceSearchService(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IList<TBService>> OrderQueryableAsync(List<string> searchKeywords)
        {
            var allTBServices = await _referenceDataRepository.GetAllTbServicesAsync();
            
            var filteredTBServices = allTBServices
                .Where(tbs => searchKeywords.All(s => tbs.Name.ToLower().Contains(s)))
                .OrderBy(tbs => tbs.Name)
                .ToList();

            return filteredTBServices;
        }
    }
}
