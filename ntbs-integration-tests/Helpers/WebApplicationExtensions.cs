using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_integration_tests.Helpers
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationFactory<Startup> WithMockUserService<T>(this WebApplicationFactory<Startup> factory) where T: class, IUserService
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserService, T>();
                });
            });
        }

        public static WebApplicationFactory<Startup> WithNotificationAndTbServiceConnected(this WebApplicationFactory<Startup> factory,
                                                                                            int notificationId,
                                                                                            string tbServiceCode)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                SetupDatabase(builder, new Tuple<int, string>(notificationId, tbServiceCode), null);
            });
        }

        public static WebApplicationFactory<Startup> WithNotificationAndPostcodeConnected(this WebApplicationFactory<Startup> factory,
                                                                                            int notificationId,
                                                                                            string postcode)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                SetupDatabase(builder, null, new Tuple<int, string>(notificationId, postcode));
            });
        }

        public static HttpClient CreateClientWithoutRedirects(this WebApplicationFactory<Startup> factory)
        {
            return factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private static void SetupDatabase(IWebHostBuilder builder, Tuple<int, string> idToServiceCode, Tuple<int, string> idToPostcode)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices
                        .GetRequiredService<NtbsContext>();

                    if (idToServiceCode != null)
                    {
                        Utilities.SetServiceCodeForNotification(db, idToServiceCode.Item1, idToServiceCode.Item2);
                    }

                    if (idToPostcode != null)
                    {
                        Utilities.SetPostcodeForNotification(db, idToPostcode.Item1, idToPostcode.Item2);
                    }
                }
            });
        }
    }
}