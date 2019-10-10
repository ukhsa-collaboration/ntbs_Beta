using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ntbs_service.Services;

namespace ntbs_service.Middleware
{
/*
    This class is used to audit all page reads of the system.
    It hooks into the middleware pipeline, and audits all successful (status 200) GET requests of particular records (no search pages/lists of records).
 */
    public class AuditGetRequestMiddleWare
    {
        private readonly RequestDelegate _next;

        public AuditGetRequestMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuditService auditService)
        {
            // Call the MVC middleware so we know HTTP status code
            await _next(context);

            var request = context.Request;
            var response = context.Response;

            if (response.StatusCode == StatusCodes.Status200OK && request.Method == "GET" && request.Query.ContainsKey("id"))
            {
                var id = int.Parse(request.Query["id"].ToString());
                var userName = context.User.FindFirstValue(ClaimTypes.Email);
                // Fallbacks if user doesn't have an email associated with them - as is the case with our test users
                if (string.IsNullOrEmpty(userName)) userName = context.User.Identity.Name;
                // TODO: Differentiate between Cluster and Full view.
                await auditService.OnGetAuditAsync(id, model: "Notification", viewType: "Full", userName: userName);
            }
        }
    }
}