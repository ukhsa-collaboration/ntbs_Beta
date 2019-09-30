using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ntbs_service.Services;

namespace ntbs_service.Middleware
{
    public class AuditGetRequestMiddleWare
    {
        private readonly RequestDelegate _next;

        public AuditGetRequestMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IAuditService auditService)
        {
            var request = context.Request;

            if (request.Method == "GET" && request.Query.ContainsKey("id"))
            {
                var id = int.Parse(request.Query["id"].ToString());
                var modelName = request.Path.Value.Split("/").Last();
                await auditService.OnGetAuditAsync(id, modelName, viewType: "Full");
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}