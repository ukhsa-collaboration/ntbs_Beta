using System;
using System.Globalization;
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
            var adConnectionSettings = Configuration.GetSection("AdConnectionSettings");
            var setupDummyAuth = adfsConfig.GetValue("UseDummyAuth", false);
            var authSetup = services.AddAuthentication(sharedOptions =>
                {
                    sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                }).AddWsFederation(options =>
                {
                    options.MetadataAddress =
                        adfsConfig["AdfsUrl"] + "/FederationMetadata/2007-06/FederationMetadata.xml";
                    options.Wtrealm = adfsConfig["Wtrealm"];
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
                        .RequireRole(adfsConfig["AdGroupsPrefix"] + adfsConfig["BaseUserGroup"])
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
            services.AddScoped<IAdDirectoryFactory, AdDirectoryServiceFactory>();
            services.AddScoped<IAdImportService, AdImportService>();
            services.AddScoped<IItemRepository<TreatmentEvent>, TreatmentEventRepository>();

            services.Configure<AdfsOptions>(adfsConfig);
            services.Configure<AdConnectionSettings>(adConnectionSettings);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true, ConfigFile = "webpack.dev.js"
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
                Authorization = new[] {new HangfireAuthorisationFilter(GetAdminRoleName())}
            };
            app.UseHangfireDashboard("/hangfire", dashboardOptions);
            app.UseHangfireServer(new BackgroundJobServerOptions {WorkerCount = 1});
            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute {Attempts = 0});
            HangfireJobScheduler.ScheduleRecurringJobs();
        }

        private string GetAdminRoleName()
        {
            var adfsConfig = Configuration.GetSection("AdfsOptions");
            return adfsConfig["AdGroupsPrefix"] + adfsConfig["AdminUserGroup"];
        }
    }
}
