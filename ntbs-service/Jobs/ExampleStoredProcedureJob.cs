using System.Threading.Tasks;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using Serilog;

namespace ntbs_service.Jobs
{
    public class ExampleStoredProcedureJob : StoredProcedureJobBase
    {
        public ExampleStoredProcedureJob(IConfiguration configuration)
        : base(configuration)
        {
            this._sqlString = "[dbo].[uspCallDivZero]";
            this._parameters = null;
        }

        public override async Task Run(IJobCancellationToken token)
        {
            Log.Information($"Starting example stored procedure job");
            
            await base.Run(token);

            Log.Information($"Finishing example stored procedure job.");
        }
    }
}
