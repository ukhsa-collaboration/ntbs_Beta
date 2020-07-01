using System;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using Serilog;
using Serilog.Events;

namespace ntbs_service
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            SetUpLogger();
            try
            {
                Log.Information("Building web host");
                var host = CreateWebHostBuilder(args).Build();
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    MigrateAppDb(services);
                    MigrateAuditDb(services);
                    MigrateKeysDb(services);
                    await CreateMigrationDbTablesAsync(services);
                }

                Log.Information("Starting web host");
                host.Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void SetUpLogger()
        {
            Log.Logger = new LoggerConfiguration()
                // Swap these to increase logging. In particular to see EF queries
                // .MinimumLevel.Debug()
                // .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Sentry(s =>
                {
                    s.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                    s.MinimumEventLevel = LogEventLevel.Warning;
                })
                .CreateLogger();
        }

        private static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSentry(o => o.Release = Environment.GetEnvironmentVariable(Constants.Release))
                .UseSerilog();

        private static void MigrateAppDb(IServiceProvider services)
        {
            try
            {
                Log.Information("Starting app db migration");
                // We're using DesignTimeFactory rather than NtbsContext directly in order to use a different login
                var factory = services.GetRequiredService<NtbsContextDesignTimeFactory>();
                var context = factory.CreateDbContext(new string[]{});
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the db.");
                throw;
            }
        }

        private static void MigrateAuditDb(IServiceProvider services)
        {
            try
            {
                Log.Information("Starting audit db migration");
                var context = services.GetRequiredService<AuditDatabaseContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the Audit db.");
                throw;
            }
        }

        private static void MigrateKeysDb(IServiceProvider services)
        {
            try
            {
                Log.Information("Starting keys db migration");
                var context = services.GetRequiredService<KeysContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the keys db.");
                throw;
            }
        }

        private static async Task CreateMigrationDbTablesAsync(IServiceProvider services)
        {
            var notificationImportHelper = services.GetRequiredService<INotificationImportHelper>();
            await notificationImportHelper.CreateTableIfNotExists();
        }
    }
}
