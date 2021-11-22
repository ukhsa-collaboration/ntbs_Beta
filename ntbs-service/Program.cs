using System;
using System.Linq;
using EFAuditer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using Sentry;
using Serilog;
using Serilog.Events;

namespace ntbs_service
{
    public class Program
    {
        public static int Main(string[] args)
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
                .ConfigureKestrel(options =>
                {
                    options.AddServerHeader = false;
                })
                .UseStartup<Startup>()
                .UseSentry(options =>
                {
                    options.Release = Environment.GetEnvironmentVariable(Constants.Release);
                    // This is a workaround for a known issue in Sentry.
                    // See https://github.com/getsentry/sentry-dotnet/issues/1210
                    options.DisableDiagnosticSourceIntegration();
                })
                .UseSerilog();

        private static void MigrateAppDb(IServiceProvider services)
        {
            try
            {
                Log.Information("Starting app db migration");
                // We're using DesignTimeFactory rather than NtbsContext directly in order to use a different login
                var factory = services.GetRequiredService<NtbsContextDesignTimeFactory>();
                var context = factory.CreateDbContext(new string[] { });
                context.Database.Migrate();
                SetCurrentVersionData(context);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the db.");
                throw;
            }
        }

        private static void SetCurrentVersionData(NtbsContext context)
        {
            var currentVersion = context.ReleaseVersion.SingleOrDefault();
            var hasCurrentVersion = currentVersion != null;

            if (!hasCurrentVersion || currentVersion.Version != VersionInfo.CurrentVersion)
            {
                if (hasCurrentVersion)
                {
                    context.Remove(currentVersion);
                }

                var newVersion = new ReleaseVersion
                {
                    Version = VersionInfo.CurrentVersion,
                    Date = VersionInfo.CurrentVersionDate
                };
                context.Add(newVersion);
                context.SaveChanges();
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
    }
}
