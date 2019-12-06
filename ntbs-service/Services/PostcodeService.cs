using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface IPostcodeService
    {
        Task<PostcodeLookup> FindPostcode(string postcode);
    }

    public class PostcodeService : IPostcodeService
    {
        private readonly NtbsContext context;

        public PostcodeService(NtbsContext context) {
            this.context = context;
        }

        public async Task<PostcodeLookup> FindPostcode(string postcode)
        {
            postcode = postcode?.Replace(" ", "").ToUpper();
            return await context.PostcodeLookup.FirstOrDefaultAsync(x => x.Postcode == postcode);

        }
    }
}
