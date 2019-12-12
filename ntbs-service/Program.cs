using System;
using EFAuditer;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service.DataAccess;
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
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseSerilog();

        private static void MigrateAppDb(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<NtbsContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the DB.");
                throw ex;
            }
        }

        private static void MigrateAuditDb(IServiceProvider services)
        {
            try
            {
                var context = services.GetRequiredService<AuditDatabaseContext>();
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred migrating the Audit DB.");
                throw ex;
            }
        }
    }
}
