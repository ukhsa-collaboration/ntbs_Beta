using EFAuditer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service.Models;
using System;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using ntbs_service.DataAccess;

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
            builder.UseEnvironment("Test");

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
                }
            });
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            //Real TCP port
            host = builder.Build();
            host.Start();
            RootUri = host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); //port 5001

            // This is a 'fake' server, needed to be returned by the method but will not actually be used.
            return new TestServer(new WebHostBuilder().UseEnvironment("Test").UseStartup<TStartup>());
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
