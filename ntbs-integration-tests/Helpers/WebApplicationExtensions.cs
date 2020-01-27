using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service;
using ntbs_service.DataAccess;
using ntbs_service.Services;

namespace ntbs_integration_tests.Helpers
{
    public static class WebApplicationExtensions
    {

        public static WebApplicationFactory<Startup> WithNotificationAndTbServiceConnected(this WebApplicationFactory<Startup> factory,
                                                                                            int notificationId,
                                                                                            string tbServiceCode)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                UpdateDatabase(builder, (db) => Utilities.SetServiceCodeForNotification(db, notificationId, tbServiceCode));
            });
        }

        public static WebApplicationFactory<Startup> WithNotificationAndPostcodeConnected(this WebApplicationFactory<Startup> factory,
                                                                                            int notificationId,
                                                                                            string postcode)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                UpdateDatabase(builder, (db) => Utilities.SetPostcodeForNotification(db, notificationId, postcode));
            });
        }

        public static HttpClient CreateClientWithoutRedirects(this WebApplicationFactory<Startup> factory)
        {
            return factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private static void UpdateDatabase(IWebHostBuilder builder, Action<NtbsContext> updateMethod)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices
                        .GetRequiredService<NtbsContext>();

                    updateMethod(db);
                }
            });
        }
    }
}
