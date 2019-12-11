using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ntbs_service.Models.Enums;
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

            if (response.StatusCode == StatusCodes.Status200OK && request.Method == "GET" && request.Path.Value.Contains("Notifications"))
            {
                // We only want to audit reads of pages, i.e. where paths are of form Notifications/{id} or /Notifications/{id}/Edit/{modelName}
                // We also make get requests for validation etc (which have longer paths), so ensure we ignore these here
                var pathArray = request.Path.Value.Split('/');
                var maxIndex = pathArray.Length - 1;
                var notificationIndex = Array.IndexOf(pathArray, "Notifications");
                var shouldAudit = (maxIndex > notificationIndex && maxIndex <= notificationIndex + 3);

                if (shouldAudit && int.TryParse(pathArray[notificationIndex + 1], out var id))
                {
                    var userName = context.User.FindFirstValue(ClaimTypes.Email);
                    // Fallbacks if user doesn't have an email associated with them - as is the case with our test users
                    if (string.IsNullOrEmpty(userName)) userName = context.User.Identity.Name;
                    // TODO: Differentiate between Cluster and Full view.
                    await auditService.OnGetAuditAsync(id, model: "Notification", auditDetails: AuditType.Full, userName: userName);
                };
            }
        }
    }
}