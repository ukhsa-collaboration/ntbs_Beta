using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_integration_tests.TestServices
{
    public class MockHomepageKpiService : IHomepageKpiService
    {
        public Task<IEnumerable<HomepageKpi>> GetKpiForPhec(IEnumerable<string> phecCodes)
        {
            var homepageKpiDetails = phecCodes.Select(x => new HomepageKpi {Code = x});
            return Task.FromResult(homepageKpiDetails);
        }

        public Task<IEnumerable<HomepageKpi>> GetKpiForTbService(IEnumerable<string> tbServiceCodes)
        {
            var homepageKpiDetails = tbServiceCodes.Select(x => new HomepageKpi {Code = x});
            return Task.FromResult(homepageKpiDetails);
        }
    }
}
