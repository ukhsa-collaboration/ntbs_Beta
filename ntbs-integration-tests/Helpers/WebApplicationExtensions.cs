using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_integration_tests.Helpers
{
    public static class WebApplicationExtensions
    {
        public static WebApplicationFactory<Startup> WithNhsUserBuilder(this WebApplicationFactory<Startup> factory, IDictionary<int, string> idServiceCodeMapping)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                SetupDatabase(builder, idServiceCodeMapping);
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserService, NhsUserService>();
                });
            });
        }

        public static WebApplicationFactory<Startup> WithPheUserBuilder(this WebApplicationFactory<Startup> factory,
                                                                        IDictionary<int, string> idServiceCodeMapping,
                                                                        IDictionary<int, string> idPostcodeMapping)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                SetupDatabase(builder, idServiceCodeMapping, idPostcodeMapping);
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserService, PheUserService>();
                });
            });
        }

        public static HttpClient WithoutRedirects(this WebApplicationFactory<Startup> factory)
        {
            return factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        private static void SetupDatabase(IWebHostBuilder builder, IDictionary<int, string> idServiceCodePairs, IDictionary<int, string> idPostcodePairs = null)
        {
            builder.ConfigureServices(services =>
            {
                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices
                        .GetRequiredService<NtbsContext>();

                    if (idServiceCodePairs != null)
                    {
                        foreach (var idCodePair in idServiceCodePairs)
                        {
                            Utilities.AddServiceCodeToNotification(db, idCodePair.Key, idCodePair.Value);
                        }
                    }

                     if (idPostcodePairs != null)
                    {
                        foreach (var idCodePair in idPostcodePairs)
                        {
                            Utilities.AddPostcodeToNotification(db, idCodePair.Key, idCodePair.Value);
                        }
                    }
                }
            });
        }
    }
}