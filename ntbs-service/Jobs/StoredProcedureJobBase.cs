using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;
using Serilog;

namespace ntbs_service.Jobs
{
    public class StoredProcedureJobBase
    {
        protected static void CheckExecutedSuccessfully(PerformContext context, IEnumerable<dynamic> resultToTest)
        {
            var serialisedResult = JsonConvert.SerializeObject(resultToTest);
            LogInfo(context, $"Result: {serialisedResult}");

            if (resultToTest.Any())
            {
                throw new ApplicationException(
                    "Stored procedure did not execute successfully as result has messages, check the logs");
            }
        }

        protected static void LogInfo(PerformContext context, string message)
        {
            Log.Information(message);
            context.WriteLine(message);
        }
    }
}
