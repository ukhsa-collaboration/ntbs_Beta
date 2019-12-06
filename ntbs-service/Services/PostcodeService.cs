using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using System.Linq;

namespace ntbs_service.Services
{
    public interface IPostcodeService
    {
        Task<PostcodeLookup> FindPostcode(string postcode);
        IEnumerable<PostcodeLookup> FindPostcodes(List<string> postcodes);
    }

    public class PostcodeService : IPostcodeService
    {
        private readonly NtbsContext context;

        public PostcodeService(NtbsContext context) {
            this.context = context;
        }

        public async Task<PostcodeLookup> FindPostcode(string postcode)
        {
            postcode = ToUpperAndRemoveSpaces(postcode);
            return await context.PostcodeLookup.FirstOrDefaultAsync(x => x.Postcode == postcode);

        }

        public IEnumerable<PostcodeLookup> FindPostcodes(List<string> postcodes)
        {
            var normalisedPostcodes = postcodes.Select(x => ToUpperAndRemoveSpaces(x));
            return context.PostcodeLookup.Where(x => normalisedPostcodes.Contains(x.Postcode));
        }

        private string ToUpperAndRemoveSpaces(string text)
        {
            return text?.Replace(" ", "").ToUpper();
        }
    }
}