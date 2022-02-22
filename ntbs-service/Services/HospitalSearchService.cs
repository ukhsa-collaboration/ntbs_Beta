using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface IHospitalSearchService
    {
        Task<IList<Hospital>> OrderQueryableAsync(List<string> searchKeywords);
    }

    class HospitalSearchService : IHospitalSearchService
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public HospitalSearchService(IReferenceDataRepository referenceDataRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IList<Hospital>> OrderQueryableAsync(List<string> searchKeywords)
        {
            var allHospitals = await _referenceDataRepository.GetAllHospitals();
            
            var filteredHospitals = allHospitals
                .Where(h => searchKeywords.All(s => h.Name.ToLower().Contains(s)))
                .ToList();
            
            return filteredHospitals;
        }
    }
}
