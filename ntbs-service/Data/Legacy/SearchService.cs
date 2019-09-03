using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ntbs_service.Data.Legacy
{

    public interface ISearchService
    {
        IEnumerable<SearchResult> Search(SearchRequest request);
    }


    public class SearchService : ISearchService
    {
        private readonly IETSSearchService etsSearcher;
        private readonly ILTBRSearchService ltbrSearcher;
        private readonly IAnnualReportSearchService annualReportSearchService;

        public SearchService(
            IETSSearchService etsSearcher,
            ILTBRSearchService ltbrSearcher,
            IAnnualReportSearchService annualReportSearchService)
        {
            this.etsSearcher = etsSearcher;
            this.ltbrSearcher = ltbrSearcher;
            this.annualReportSearchService = annualReportSearchService;
        }

        public IEnumerable<SearchResult> Search(SearchRequest request)
        {
            var etsTask = etsSearcher.Search(request);
            var ltbrTask = ltbrSearcher.Search(request);
            var annualreportTask = annualReportSearchService.Search(request);
            Task.WaitAll(etsTask, ltbrTask, annualreportTask);

            return etsTask.Result.Union(ltbrTask.Result).Union(annualreportTask.Result);
        }
    }
}
