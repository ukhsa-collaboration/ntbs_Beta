using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Models.QueryEntities;
using Serilog;

namespace ntbs_service.Jobs
{
    public class UpdateTableCountsJob
    {
        private readonly ITableCountsRepository _tableCountsRepository;
        private PerformContext _context;

        private const double DecreaseMarginOfError = 0.995; // Allow decreases of less than 0.5%

        public UpdateTableCountsJob(ITableCountsRepository tableCountsRepository)
        {
            _tableCountsRepository = tableCountsRepository;
        }

        /// PerformContext context is passed in via Hangfire Server
        public async Task Run(PerformContext context)
        {
            _context = context;
            Log.Information("Starting table counting job.");

            var results = await _tableCountsRepository.ExecuteUpdateTableCountsStoredProcedure();
            CheckExecutedSuccessfully(results);

            Log.Information("Starting table count comparison.");

            await CompareRecentTableCounts();

            Log.Information("Finishing table counting job.");
        }

        private async Task CompareRecentTableCounts()
        {
            var counts = (await _tableCountsRepository.GetRecentTableCounts()).ToList();

            if (counts.Count < 2)
            {
                const string notEnoughCountsMessage =
                    "Not enough table counts available. Try re-running the update-table-counts job.";

                Log.Information(notEnoughCountsMessage);
                _context.WriteLine(notEnoughCountsMessage);
                return;
            }

            var currentCount = counts[0];
            var previousCount = counts[1];

            var validationErrors = typeof(TableCounts)
                .GetProperties()
                .SelectMany(property => ValidateCountHasNotDecreased(property, currentCount, previousCount))
                .ToList();

            if (validationErrors.Any())
            {
                throw new ApplicationException(
                    $"Some table counts have decreased:\n{string.Join("\n", validationErrors)}");
            }

            const string normalMessage = "Table counts look normal";

            Log.Information(normalMessage);
            _context.WriteLine(normalMessage);
        }

        private static IEnumerable<string> ValidateCountHasNotDecreased(PropertyInfo property, TableCounts currentCount,
            TableCounts previousCount)
        {
            var errors = new List<string>();

            if (property.PropertyType != typeof(int))
            {
                return errors;
            }

            var currentValue = (int)property.GetValue(currentCount);
            var previousValue = (int)property.GetValue(previousCount);
            if (currentValue < previousValue * DecreaseMarginOfError && currentValue < previousValue - 1)
            {
                errors.Add($"Property: {property.Name} has decreased from {previousValue} to {currentValue}");
            }

            return errors;
        }

        private void CheckExecutedSuccessfully(IEnumerable<dynamic> errorMessages)
        {
            var errorMessagesJson = JsonConvert.SerializeObject(errorMessages);
            Log.Information(errorMessagesJson);
            _context.WriteLine($"Stored procedure result: {errorMessagesJson}");

            if (errorMessages.Any())
            {
                throw new ApplicationException(
                    "Stored procedure did not execute successfully as result has messages, check the logs.");
            }
        }
    }
}
