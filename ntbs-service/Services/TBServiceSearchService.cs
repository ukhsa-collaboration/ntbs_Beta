using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface ITBServiceSearchService
    {
        Task<IList<TBService>> OrderQueryableAsync(string searchKeyword);
    }

    class TbServiceSearchService : ITBServiceSearchService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public TbServiceSearchService(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IList<TBService>> OrderQueryableAsync(string searchKeyword)
        {
            var searchKeywords = searchKeyword.Split(" ")
                .Where(x => !x.IsNullOrEmpty())
                .Select(s => s.ToLower()).ToList();

            var allTBServices = await _referenceDataRepository.GetAllTbServicesAsync();
            
            var filteredTBServices = allTBServices
                .Where(tbs => searchKeywords.All(s => tbs.Name.ToLower().Contains(s)))
                .ToList();

            return filteredTBServices;
        }
    }
}
