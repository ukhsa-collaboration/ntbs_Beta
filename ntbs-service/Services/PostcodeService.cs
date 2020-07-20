using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface IPostcodeService
    {
        Task<PostcodeLookup> FindPostcodeAsync(string postcode);
    }

    public class PostcodeService : IPostcodeService
    {
        private readonly NtbsContext _context;

        public PostcodeService(NtbsContext context) {
            this._context = context;
        }

        public async Task<PostcodeLookup> FindPostcodeAsync(string postcode)
        {
            postcode = ToUpperAndRemoveSpaces(postcode);
            return await _context.PostcodeLookup.FirstOrDefaultAsync(x => x.Postcode == postcode);

        }

        private string ToUpperAndRemoveSpaces(string text)
        {
            return text?.Replace(" ", "").ToUpper();
        }
    }
}
