using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using EFAuditer;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ntbs_service.Authentication;
using ntbs_service.Data.Legacy;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Jobs;
using ntbs_service.Middleware;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Properties;
using ntbs_service.Services;
using Serilog;

namespace ntbs_service
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            if (!Env.IsEnvironment("Test"))
            {
                services.AddHangfire(config =>
                {
                    config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(Configuration.GetConnectionString("ntbsContext"),
                            new SqlServerStorageOptions
                            {
                                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                                QueuePollInterval = TimeSpan.Zero,
                                UseRecommendedIsolationLevel = true,
                                UsePageLocksOnDequeue = true,
                                DisableGlobalLocks = true
                            })
                        .UseConsole();
                });
            }

            var adfsConfig = Configuration.GetSection("AdfsOptions");
            var ldapConnectionSettings = Configuration.GetSection("LdapConnectionSettings");
            var setupDummyAuth = adfsConfig.GetValue("UseDummyAuth", false);
            var authSetup = services
                .AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                })
                .AddWsFederation(options =>
                {
                    options.MetadataAddress =
                        adfsConfig["AdfsUrl"] + "/FederationMetadata/2007-06/FederationMetadata.xml";
                    options.Wtrealm = adfsConfig["Wtrealm"];
                    
                    /*
                     * Below event handler is to prevent stale logins from showing a 500 error screen, instead to force
                     * back to the landing page - and cause a re-challenge or continue if already authenticated.
                     * https://community.auth0.com/t/asp-net-core-2-intermittent-correlation-failed-errors/11918/14
                     */
                    options.Events.OnRemoteFailure += context =>
                    {
                        if (context.Failure.Message == "Correlation failed.")
                        {
                            context.HandleResponse();
                            context.Response.Redirect("/");
                        }

                        return Task.CompletedTask;
                    };
                })
                .AddCookie(options =>
                {
                    options.ForwardAuthenticate = setupDummyAuth ? DummyAuthHandler.Name : null;
                });

            if (setupDummyAuth)
            {
                authSetup.AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>(DummyAuthHandler.Name, o => { });
            }

            services.AddMvc(options =>
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .RequireRole(adfsConfig["BaseUserGroup"])
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }).AddRazorPagesOptions(options =>
                {
                    options.Conventions.AllowAnonymousToPage("/Account/AccessDenied");
                    options.Conventions.AllowAnonymousToPage("/Logout");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole(GetAdminRoleName());
                });
            });

            services.AddDbContext<NtbsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ntbsContext"))
            );

            var auditDbConnectionString = Configuration.GetConnectionString("auditContext");

            services.AddDbContext<AuditDatabaseContext>(options =>
                options.UseSqlServer(auditDbConnectionString)
            );

            if (Configuration.GetValue<bool>(Constants.AUDIT_ENABLED_CONFIG_VALUE))
            {
                services.AddEFAuditer(auditDbConnectionString);
            }
            else
            {
                Audit.Core.Configuration.AuditDisabled = true;
            }

            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IReferenceDataRepository, ReferenceDataRepository>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<INotificationMapper, NotificationMapper>();
            services.AddScoped<IImportLogger, ImportLogger>();
            services.AddScoped<INotificationImportService, NotificationImportService>();
            services.AddScoped<INotificationImportRepository, NotificationImportRepository>();
            services.AddScoped<INotificationImportHelper, NotificationImportHelper>();
            services.AddScoped<IMigrationRepository, MigrationRepository>();
            services.AddScoped<IAnnualReportSearchService, AnnualReportSearcher>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostcodeService, PostcodeService>();
            services.AddScoped<Services.IAuthorizationService, AuthorizationService>();
            services.AddScoped<ILegacySearchService, LegacySearchService>();
            services.AddScoped<IItemRepository<ManualTestResult>, TestResultRepository>();
            services.AddScoped<IItemRepository<SocialContextVenue>, SocialContextVenueRepository>();
            services.AddScoped<IItemRepository<SocialContextAddress>, SocialContextAddressRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAdDirectoryServiceFactory, AdDirectoryServiceServiceFactory>();
            services.AddScoped<IAdImportService, AdImportService>();
            services.AddScoped<IItemRepository<TreatmentEvent>, TreatmentEventRepository>();
            services.AddScoped<IHomepageKpiService, HomepageKpiService>();
            services.AddScoped<IDataQualityRepository, DataQualityRepository>();
            services.AddScoped<IDrugResistanceProfileRepository, DrugResistanceProfileRepository>();
            services.AddScoped<IDrugResistanceProfilesService, DrugResistanceProfileService>();
            services.AddScoped<IMdrService, MdrService>();
            services.AddScoped<IFaqRepository, FaqRepository>();

            services.Configure<AdfsOptions>(adfsConfig);
            services.Configure<LdapConnectionSettings>(ldapConnectionSettings);
            services.Configure<MigrationConfig>(Configuration.GetSection("MigrationConfig"));
            
            var referenceLabResultsConfig = Configuration.GetSection("ReferenceLabResultsConfig");
            if (referenceLabResultsConfig.GetValue<bool>("MockOutSpecimenMatching"))
            {
                var notificationId = referenceLabResultsConfig.GetValue<int>("MockedNotificationId");
                var tbServiceCode = referenceLabResultsConfig.GetValue<string>("MockedTbServiceCode");
                var phecCode = referenceLabResultsConfig.GetValue<string>("MockedPhecCode");
                services.AddScoped<ICultureAndResistanceService>(
                    sp => new MockCultureAndResistanceService(notificationId));
                services.AddScoped<ISpecimenService>(
                    sp => new MockSpecimenService(notificationId, tbServiceCode, phecCode));
            }
            else
            {
                services.AddScoped<ICultureAndResistanceService, CultureAndResistanceService>();
                services.AddScoped<ISpecimenService, SpecimenService>();
            }

            var clusterMatchingConfig = Configuration.GetSection("ClusterMatchingConfig");
            if (clusterMatchingConfig.GetValue<bool>("MockOutClusterMatching"))
            {
                var notificationClusterValues = new List<NotificationClusterValue>();
                clusterMatchingConfig.Bind("MockedNotificationClusterValues", notificationClusterValues);

                services.AddScoped<INotificationClusterService>(sp =>
                    new MockNotificationClusterService(notificationClusterValues));
            }
            else
            {
                services.AddScoped<INotificationClusterService, NotificationClusterService>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())	
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                    ConfigFile = "webpack.dev.js"
                });
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/errors/{0}");
                app.UseExceptionHandler("/errors/500");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            if (!Env.IsEnvironment("Test"))
            {
                /*
                Making this conditional is the result of serilog not playing nicely with WebApplicationFactory
                used by the ui tests, see: https://github.com/serilog/serilog-aspnetcore/issues/105
                Using env directly as check is an unsatisfying solution, but configuration values were not picked up consistently correctly here.
                */
                app.UseSerilogRequestLogging(options => // Needs to be before MVC handlers
                {
                    options.MessageTemplate =
                        "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms ({RequestId})";
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseCookiePolicy();
            if (Configuration.GetValue<bool>(Constants.AUDIT_ENABLED_CONFIG_VALUE))
            {
                app.UseMiddleware<AuditGetRequestMiddleWare>();
            }

            app.UseMvc();

            ConfigureHangfire(app);

            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }

        private void ConfigureHangfire(IApplicationBuilder app)
        {
            if (Env.IsEnvironment("Test"))
            {
                return;
            }

            var dashboardOptions = new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorisationFilter(GetAdminRoleName()) }
            };
            app.UseHangfireDashboard("/hangfire", dashboardOptions);
            app.UseHangfireServer(new BackgroundJobServerOptions { WorkerCount = 1 });
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
            
            if (!Env.IsDevelopment())	
            {
                // Most of the time we don't care about recurring jobs in dev mode.
                // Having this exclusion is also useful when connecting to non-dev databases for debugging.
                // as jobs scheduled from (windows) dev machines won't run on linux due to different timezone formats
                // Comment out this check to work with jobs locally.
                HangfireJobScheduler.ScheduleRecurringJobs();
            }
        }

        private string GetAdminRoleName()
        {
            var adfsConfig = Configuration.GetSection("AdfsOptions");
            return adfsConfig["AdminUserGroup"];
        }
    }
}
