using System.Threading.Tasks;
using EFAuditer;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface IPostcodeService
    {
        Task<PostcodeLookup> FindPostcode(string postcode);
    }

    public class PostcodeService : IPostcodeService
    {
        private readonly IPostcodeRepository repository;

        public PostcodeService(IPostcodeRepository repository) {
            this.repository = repository;
        }

        public async Task<PostcodeLookup> FindPostcode(string postcode)
        {
            return await repository.FindPostcode(postcode.Replace(" ", "").ToUpper());
        }
    }
}