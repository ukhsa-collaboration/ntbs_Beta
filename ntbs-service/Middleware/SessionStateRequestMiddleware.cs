using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ntbs_service.Helpers;
using ntbs_service.Services;

namespace ntbs_service.Middleware
{
    public class SessionStateRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionStateRequestMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Path.Value.Contains("Heartbeat"))
            {
                SessionStateHelper.UpdateSessionActivity(context.Session);
            }
            
            // Call the MVC middleware so we know HTTP status code
            await _next(context);
        }
    }
}
