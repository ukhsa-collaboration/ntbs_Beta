using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ntbs_service.Helpers;

namespace ntbs_service.Middleware
{
    public class ActivityDetectionMiddleware
    {
        private readonly RequestDelegate _next;

        public ActivityDetectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.Value.Contains("Heartbeat"))
            {
                SessionStateHelper.UpdateSessionActivity(context.Session);
            }
            
            await _next(context);
        }
    }
}
