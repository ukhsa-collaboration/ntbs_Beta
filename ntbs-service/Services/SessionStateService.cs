using System;
using Microsoft.Extensions.Caching.Distributed;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface ISessionStateService
    {
        void UpdateSessionStateService(string cookie, string value = null);
        bool IsUpdatedRecently(string cookie);
    }
    
    public class SessionStateService : ISessionStateService
    {
        private IDistributedCache _cache;

        public SessionStateService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public void UpdateSessionStateService(string cookie, string value = null)
        {
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(13);
            
            _cache.SetString(cookie, value, options);
        }

        public bool IsUpdatedRecently(string cookie)
        {
            return _cache.GetString(cookie) != null;
        }
    }
}
