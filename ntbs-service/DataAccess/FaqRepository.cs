using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataAccess
{
    public interface IFaqRepository
    {
        IEnumerable<FrequentlyAskedQuestion> GetAll();
    }
    
    public class FaqRepository : IFaqRepository
    {
        private readonly NtbsContext _context;

        public FaqRepository(NtbsContext context)
        {
            _context = context;
        }
        
        public IEnumerable<FrequentlyAskedQuestion> GetAll()
        {
            return _context.FrequentlyAnsweredQuestions
                .OrderBy(x => x.OrderIndex);
        }
    }
}
