using EFAuditer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service.DataAccess;
using ntbs_service.Services;

namespace ntbs_integration_tests
{
    public class NtbsWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
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
                    options.UseInMemoryDatabase("Ntbs_Test_Db");
                    options.UseInternalServiceProvider(serviceProvider);
                });

                services.AddDbContext<AuditDatabaseContext>(options =>
                {
                    options.UseInMemoryDatabase("Ntbs_Audit_Test_Db");
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
                    sp => new MockSpecimenService(Utilities.NOTIFIED_ID));
                services.AddScoped<IHomepageKpiService>(sp => new MockHomepageKpiService());
            });
        }
    }
}
