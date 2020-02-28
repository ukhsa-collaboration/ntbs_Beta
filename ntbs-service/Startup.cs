using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
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
            // Configuration
            var adfsConfig = Configuration.GetSection("AdfsOptions");
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<AdfsOptions>(adfsConfig);
            services.Configure<LdapConnectionSettings>(Configuration.GetSection("LdapConnectionSettings"));
            services.Configure<MigrationConfig>(Configuration.GetSection("MigrationConfig"));
            
            // Plugin services
            SetupAuthentication(services, adfsConfig);

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
                    options.Conventions.AllowAnonymousToPage("/Health");
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.RequireRole(GetAdminRoleName());
                });
            });
            SetupHangfire(services);

            // DB Contexts
            services.AddDbContext<NtbsContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("ntbsContext"))
            );

            var auditDbConnectionString = Configuration.GetConnectionString("auditContext");

            services.AddDbContext<AuditDatabaseContext>(options =>
                options.UseSqlServer(auditDbConnectionString)
            );

            // Repositories
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IReferenceDataRepository, ReferenceDataRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<INotificationImportRepository, NotificationImportRepository>();
            services.AddScoped<INotificationImportHelper, NotificationImportHelper>();
            services.AddScoped<IMigrationRepository, MigrationRepository>();
            services.AddScoped<IItemRepository<ManualTestResult>, TestResultRepository>();
            services.AddScoped<IItemRepository<SocialContextVenue>, SocialContextVenueRepository>();
            services.AddScoped<IItemRepository<SocialContextAddress>, SocialContextAddressRepository>();
            services.AddScoped<IItemRepository<TreatmentEvent>, TreatmentEventRepository>();
            services.AddScoped<IItemRepository<MBovisExposureToKnownCase>, MBovisExposureToKnownCaseRepository>();
            services.AddScoped<IItemRepository<MBovisUnpasteurisedMilkConsumption>, MBovisUnpasteurisedMilkConsumptionRepository>();
            services.AddScoped<IItemRepository<MBovisOccupationExposure>, MBovisOccupationExposureRepository>();
            services.AddScoped<IItemRepository<MBovisAnimalExposure>, MBovisAnimalExposureRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IDataQualityRepository, DataQualityRepository>();
            services.AddScoped<IDrugResistanceProfileRepository, DrugResistanceProfileRepository>();
            services.AddScoped<IFaqRepository, FaqRepository>();
            
            // Services
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IAlertService, AlertService>();
            services.AddScoped<INotificationMapper, NotificationMapper>();
            services.AddScoped<IImportLogger, ImportLogger>();
            services.AddScoped<INotificationImportService, NotificationImportService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostcodeService, PostcodeService>();
            services.AddScoped<Services.IAuthorizationService, AuthorizationService>();
            services.AddScoped<ILegacySearchService, LegacySearchService>();
            services.AddScoped<IAdDirectoryServiceFactory, AdDirectoryServiceServiceFactory>();
            services.AddScoped<IAdImportService, AdImportService>();
            services.AddScoped<IEnhancedSurveillanceAlertsService, EnhancedSurveillanceAlertsService>();
            services.AddScoped<ITreatmentOutcomeService, TreatmentOutcomeService>();
            AddAuditService(services, auditDbConnectionString);
            AddReferenceLabResultServices(services);
            AddClusterService(services);
            AddReportingServices(services);
        }

        private void SetupHangfire(IServiceCollection services)
        {
            if (!Env.IsEnvironment("CI"))
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
        }

        private static void SetupAuthentication(IServiceCollection services, IConfigurationSection adfsConfig)
        {
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

                    options.Events.OnSecurityTokenValidated += context =>
                    {
                        var username = context.Principal.FindFirstValue(ClaimTypes.Email);
                        if (username != null)
                        {
                            var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                            userService.RecordUserLogin(username);
                        }

                        return Task.CompletedTask;
                    };
                })
                .AddCookie(options => { options.ForwardAuthenticate = setupDummyAuth ? DummyAuthHandler.Name : null; });

            if (setupDummyAuth)
            {
                authSetup.AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>(DummyAuthHandler.Name, o => { });
            }
        }

        private void AddAuditService(IServiceCollection services, string auditDbConnectionString)
        {
            if (Configuration.GetValue<bool>(Constants.AUDIT_ENABLED_CONFIG_VALUE))
            {
                services.AddEFAuditer(auditDbConnectionString);
            }
            else
            {
                Audit.Core.Configuration.AuditDisabled = true;
            }
        }
        
        private void AddClusterService(IServiceCollection services)
        {
            var clusterMatchingConfig = Configuration.GetSection(Constants.CLUSTER_MATCHING_CONFIG);
            if (clusterMatchingConfig.GetValue<bool>(Constants.CLUSTER_MATCHING_CONFIG__MOCKOUT))
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

        private void AddReferenceLabResultServices(IServiceCollection services)
        {
            var referenceLabResultsConfig = Configuration.GetSection(Constants.REFERENCE_LAB_RESULTS_CONFIG);
            if (referenceLabResultsConfig.GetValue<bool>(Constants.REFERENCE_LAB_RESULTS_CONFIG__MOCKOUT))
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
        }

        private void AddReportingServices(IServiceCollection services)
        {
            if (string.IsNullOrEmpty(Configuration.GetConnectionString(Constants.DB_CONNECTIONSTRING_REPORTING)))
            {
                services.AddScoped<IHomepageKpiService, MockHomepageKpiService>();
                services.AddScoped<IDrugResistanceProfilesService, MockDrugResistanceProfilesService>();                
            }
            else
            {

                services.AddScoped<IHomepageKpiService, HomepageKpiService>();
                services.AddScoped<IDrugResistanceProfilesService, DrugResistanceProfileService>();                
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

            if (!Env.IsEnvironment("CI"))
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
            if (Env.IsEnvironment("CI"))
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
