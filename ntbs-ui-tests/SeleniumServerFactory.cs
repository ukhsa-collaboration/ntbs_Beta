using EFAuditer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using ntbs_service.DataAccess;
using ntbs_service.Services;
using ntbs_ui_tests.Helpers;
using Serilog;
using Serilog.Events;

namespace ntbs_ui_tests
{
    /*
        WebApplicationFactory is supposed to be used for in-memory http requests only, with a TestServer rather than a real server
        There is a workaround to make it work with a real Server for Selenium as described in
        https://github.com/aspnet/AspNetCore/issues/4892 and https://www.hanselman.com/blog/RealBrowserIntegrationTestingWithSeleniumStandaloneChromeAndASPNETCore21.aspx
        Disadvantage is that it makes it difficult to interact with the real host if required to make changes in a particular test, but for the simple test cases
        we are considering it works.
        There does not seem to be an agreed upon solution for Dotnet core MVC with Selenium to date (November 2019), so this workaround solution seems appropriate as it is 
        most similar in setup to the integration test project
     */

    public class SeleniumServerFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        IWebHost host;
        public string RootUri { get; set; }

        public SeleniumServerFactory()
        {
            ClientOptions.BaseAddress = new Uri("https://localhost");
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("CI");

            builder.ConfigureServices(services =>
            {
                var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.AddDbContext<NtbsContext>(options =>
                {
                    options.UseInMemoryDatabase("Ntbs_UI_Test_Db");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                services.AddDbContext<AuditDatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("Ntbs_UI_Audit_Test_Db");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                var sp = services.BuildServiceProvider();

                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<NtbsContext>();

                    db.Database.EnsureCreated();
                    Utilities.SeedDatabase(db);
                }
            });

            builder.ConfigureTestServices(services =>
            {
                services.AddScoped<ICultureAndResistanceService>(
                    sp => new MockCultureAndResistanceService(Utilities.NOTIFIED_ID));
                services.AddScoped<ISpecimenService>(
                    sp => new MockSpecimenService(
                        Utilities.NOTIFIED_ID,
                        Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                        Utilities.PHEC_CONTAINING_ABINGDON_CODE));
            });
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            //Real TCP port
            host = builder.Build();
            // When running features in parallel, this line gets called twice leading to a port in use error.
            // We disable parallelisation in xunit.runner.json, but if there was a way to only start up one host for the whole process
            // that would be preferable. Cannot call this method from a BeforeTestRun binding (which would only be called once)
            // as it only accepts static method and the ServerFactory cannot be DIed
            host.Start();

            RootUri = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); //port 5001

            // This is a 'fake' server, needed to be returned by the method but will not actually be used.
            return new TestServer(new WebHostBuilder().UseEnvironment("CI").UseStartup<TStartup>());
        }
        
        public void ConfigureLogger(string testName)
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Debug();
            // // Uncomment to have SUT app log into files
            // var logFilePath = $"../../../{DateTime.Now:yyyy-M-d}-{testName}-logs.txt";
            // logger = logger.WriteTo.File(logFilePath); 
            Log.Logger = logger.CreateLogger();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                host.Dispose();
            }
        }
    }
}
