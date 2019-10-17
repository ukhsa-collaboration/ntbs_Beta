using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.DataAccess
{
    public interface IPostcodeRepository
    {
        Task<PostcodeLookup> FindPostcode(string postcode);
    }

    public class PostcodeRepository : IPostcodeRepository
    {
        private readonly NtbsContext context;
        public PostcodeRepository(NtbsContext context) 
        {
            this.context = context;
        }

        public async Task<PostcodeLookup> FindPostcode(string postcode)
        {
            return await context.PostcodeLookup.FirstOrDefaultAsync(x => x.Postcode == postcode);
        }
    }
}