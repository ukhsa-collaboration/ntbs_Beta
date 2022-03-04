using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EFAuditer;
using Hangfire;
using Hangfire.Console;
using Hangfire.SqlServer;
using Microsoft.ApplicationInsights.AspNetCore;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ntbs_service.Authentication;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Properties;
using ntbs_service.Services;
using Serilog;
using ZNetCS.AspNetCore.Authentication.Basic;
using ZNetCS.AspNetCore.Authentication.Basic.Events;

namespace ntbs_service;

public static class AppServiceConfiguration
{
    public static void ConfigureServices(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        // This was helpful for identifying issues with ADFS login - but shouldn't be on usually
        // IdentityModelEventSource.ShowPII = true;
        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders = ForwardedHeaders.All;
            options.KnownNetworks.Clear();
            options.KnownProxies.Clear();
        });

        // Configuration
        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });

        services.AddHsts(options =>
        {
            options.MaxAge = TimeSpan.FromSeconds(63072000); // Two years
        });

        var adConfig = builder.Configuration.GetSection("AdOptions");
        var adfsConfig = builder.Configuration.GetSection("AdfsOptions");
        var azureAdConfig = builder.Configuration.GetSection("AzureAdOptions");
        var applicationInsightsConfig = builder.Configuration.GetSection("ApplicationInsightsOptions");

        var adOptions = new AdOptions();
        var adfsOptions = new AdfsOptions();
        var azureAdOptions = new AzureAdOptions();
        var applicationInsightsOptions = new ApplicationInsightsOptions();

        adConfig.Bind(adOptions);
        adfsConfig.Bind(adfsOptions);
        azureAdConfig.Bind(azureAdOptions);
        applicationInsightsConfig.Bind(applicationInsightsOptions);

        services.Configure<AdOptions>(adConfig);
        services.Configure<AdfsOptions>(adfsConfig);
        services.Configure<AzureAdOptions>(azureAdConfig);
        services.Configure<LdapSettings>(builder.Configuration.GetSection("LdapSettings"));
        services.Configure<MigrationConfig>(builder.Configuration.GetSection("MigrationConfig"));

        // Plugin services
        if (builder.Environment.IsEnvironment("CI"))
        {
            services.AddDistributedMemoryCache();
        }
        else
        {
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = builder.Configuration.GetConnectionString("ntbsContext");
                options.SchemaName = "dbo";
                options.TableName = "SessionState";
            });
        }

        services.AddSession(options =>
        {
            options.Cookie.IsEssential = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        // select authentication method
        var httpBasicAuthConfig = builder.Configuration.GetSection("HttpBasicAuth");
        var basicAuthEnabled = httpBasicAuthConfig.GetValue("Enabled", false);
        var azureAdAuthEnabled = azureAdOptions.Enabled;

        Log.Information($"Basic Auth Enabled: {basicAuthEnabled}");
        Log.Information($"Azure Ad Auth Enabled: {azureAdAuthEnabled}");

        var baseUserGroupRole = adOptions.BaseUserGroup;

        if (adOptions.UseDummyAuth)
        {
            UseDummyAuth(services);
        }
        else if (basicAuthEnabled)
        {
            UseHttpBasicAuth(services, httpBasicAuthConfig, adOptions);
        }
        else if (azureAdAuthEnabled)
        {
            UseAzureAdAuthentication(services, adOptions, azureAdOptions);
        }
        else
        {
            UseAdfsAuthentication(services, adOptions, adfsOptions);
        }

        services.AddControllersWithViews(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(baseUserGroupRole)
                .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
        services.AddRazorPages(options =>
        {
            options.Conventions.AllowAnonymousToPage("/Account/AccessDenied");
            options.Conventions.AllowAnonymousToPage("/Logout");
            options.Conventions.AllowAnonymousToPage("/PostLogout");
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy =>
            {
                policy.RequireRole(builder.Configuration.GetSection("AdOptions")["AdminUserGroup"]);
            });
        });
        SetupHangfire(builder);

        var auditDbConnectionString = builder.Configuration.GetConnectionString("auditContext");
        if (!builder.Environment.IsEnvironment("CI"))
        {
            // DB Contexts
            services.AddDbContext<NtbsContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ntbsContext"))
            );

            services.AddSingleton<NtbsContextDesignTimeFactory>();

            services.AddDbContext<AuditDatabaseContext>(options =>
                options.UseSqlServer(auditDbConnectionString)
            );

            // Add a DbContext for Data Protection key storage
            services.AddDbContext<KeysContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("keysContext")));
            services.AddDataProtection().PersistKeysToDbContext<KeysContext>();
        }

        // Repositories
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IReferenceDataRepository, ReferenceDataRepository>();
        services.AddScoped<IAlertRepository, AlertRepository>();
        services.AddScoped<INotificationImportRepository, NotificationImportRepository>();
        services.AddScoped<IMigratedNotificationsMarker, MigratedNotificationsMarker>();
        services.AddScoped<INotificationImportHelper, NotificationImportHelper>();
        services.AddScoped<IMigrationRepository, MigrationRepository>();
        services.AddScoped<IItemRepository<ManualTestResult>, TestResultRepository>();
        services.AddScoped<IItemRepository<SocialContextVenue>, SocialContextVenueRepository>();
        services.AddScoped<IItemRepository<SocialContextAddress>, SocialContextAddressRepository>();
        services.AddScoped<ITreatmentEventRepository, TreatmentEventRepository>();
        services.AddScoped<IItemRepository<MBovisExposureToKnownCase>, MBovisExposureToKnownCaseRepository>();
        services
            .AddScoped<IItemRepository<MBovisUnpasteurisedMilkConsumption>,
                MBovisUnpasteurisedMilkConsumptionRepository>();
        services.AddScoped<IItemRepository<MBovisOccupationExposure>, MBovisOccupationExposureRepository>();
        services.AddScoped<IItemRepository<MBovisAnimalExposure>, MBovisAnimalExposureRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDataQualityRepository, DataQualityRepository>();
        services.AddScoped<IDrugResistanceProfileRepository, DrugResistanceProfileRepository>();

        // Services
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAlertService, AlertService>();
        services.AddScoped<INotificationMapper, NotificationMapper>();
        services.AddScoped<IImportLogger, ImportLogger>();
        services.AddScoped<INotificationImportService, NotificationImportService>();
        services.AddScoped<IImportValidator, ImportValidator>();
        services.AddScoped<ISearchService, SearchService>();
        services.AddScoped<IAuditService, AuditService>();
        services.AddScoped<INotificationChangesService, NotificationChangesService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPostcodeService, PostcodeService>();
        services.AddScoped<ntbs_service.Services.IAuthorizationService, AuthorizationService>();
        services.AddScoped<ILegacySearchService, LegacySearchService>();
        services.AddScoped<IAdDirectoryServiceFactory, AdDirectoryServiceFactory>();
        services.AddScoped<IEnhancedSurveillanceAlertsService, EnhancedSurveillanceAlertsService>();
        services.AddScoped<IUserSearchService, UserSearchService>();
        services.AddScoped<IServiceDirectoryService, ServiceDirectoryService>();
        services.AddScoped<IClusterImportService, ClusterImportService>();
        services.AddScoped<ISpecimenImportService, SpecimenImportService>();
        services.AddScoped<ICaseManagerImportService, CaseManagerImportService>();
        services.AddScoped<IAdUserService, AdUserService>();
        services.AddScoped<IExternalLinksService, ExternalLinksService>();
        services.AddScoped<ITreatmentEventMapper, TreatmentEventMapper>();
        services.AddScoped<IUserHelper, UserHelper>();
        services.AddScoped<ILogService, LogService>();
        services.AddScoped<ITableCountsRepository, TableCountsRepository>();
        services.AddScoped<IExternalStoredProcedureRepository, ExternalStoredProcedureRepository>();

        AddAuditService(builder, auditDbConnectionString);
        AddReferenceLabResultServices(builder);
        AddNotificationClusterRepository(builder);
        AddReportingServices(builder);
        AddMicrosoftGraphServices(services, azureAdOptions);
        AddAdImportService(builder, azureAdOptions);
        AddApplicationInsightsMonitoring(services, applicationInsightsOptions);
    }

    private static void SetupHangfire(WebApplicationBuilder builder)
    {
        var services = builder.Services;
        if (builder.Configuration.GetValue<bool>(Constants.HangfireEnabled))
        {
            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(builder.Configuration.GetConnectionString("ntbsContext"),
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

            services.AddHangfireServer(options =>
            {
                options.WorkerCount = builder.Configuration.GetValue<int>(Constants.HangfireWorkerCount);
            });
        }
    }

    private static void UseDummyAuth(IServiceCollection services)
    {
        var authSetup = services.AddAuthentication(DummyAuthHandler.Name);
        authSetup.AddScheme<AuthenticationSchemeOptions, DummyAuthHandler>(DummyAuthHandler.Name, o => { });
    }

    private static  void UseAdfsAuthentication(IServiceCollection services, AdOptions adOptions, AdfsOptions adfsOptions)
    {
        var authSetup = services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
            })
            .AddWsFederation(options =>
            {
                options.CallbackPath = "/Index";
                options.SkipUnrecognizedRequests = true;
                options.MetadataAddress = adfsOptions.AdfsUrl + "/FederationMetadata/2007-06/FederationMetadata.xml";
                options.Wtrealm = adfsOptions.Wtrealm;
                options.CorrelationCookie.SameSite = SameSiteMode.None;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
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

                options.Events.OnSecurityTokenValidated += async context =>
                {
                    var username = context.Principal.Username();
                    if (username != null)
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        await userService.RecordUserLoginAsync(username);
                    }
                };
            })
            .AddCookie(options =>
            {
                options.ForwardAuthenticate = null;
                options.SlidingExpiration = false;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(adOptions.MaxSessionCookieLifetimeInMinutes);
            });
    }

    private static void UseAzureAdAuthentication(IServiceCollection services, AdOptions adOptions,
        AzureAdOptions azureAdOptions)
    {
        var authSetup = services
            .AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                sharedOptions.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.ForwardAuthenticate = null;
                options.SlidingExpiration = false;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(adOptions.MaxSessionCookieLifetimeInMinutes);
            })
            .AddOpenIdConnect(options =>
            {
                options.ClientId = azureAdOptions.ClientId;
                options.ClientSecret = azureAdOptions.ClientSecret;
                options.Authority = azureAdOptions.Authority;
                options.CallbackPath = azureAdOptions.CallbackPath;
                options.CorrelationCookie.SameSite = SameSiteMode.None;
                options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                options.NonceCookie.SecurePolicy = CookieSecurePolicy.Always;

                options.Events = new OpenIdConnectEvents();
                options.Events.OnTokenValidated += async context =>
                {
                    var username = context.Principal.Username();
                    if (username == null)
                    {
                        username = context.Principal.FindFirstValue(ClaimTypes.Email);
                    }

                    // add group claims.
                    if (username != null)
                    {
                        var claims = new List<Claim>();
                        var azureAdFactory = context.HttpContext.RequestServices
                            .GetRequiredService<IAzureAdDirectoryServiceFactory>();
                        var azureAdDirectoryService = azureAdFactory.Create();

                        var groupClaims = await azureAdDirectoryService.BuildRoleClaimsForUser(username);
                        claims.AddRange(groupClaims);

                        var upnClaim = new Claim(ClaimTypes.Upn, username);
                        claims.Add(upnClaim);

                        var applicationClaimsIdentity = new ClaimsIdentity(claims);
                        context.Principal.AddIdentity(applicationClaimsIdentity);
                    }

                    if (username != null)
                    {
                        var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
                        await userService.RecordUserLoginAsync(username);
                    }
                };

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
            });
    }

    private static void UseHttpBasicAuth(IServiceCollection services,
        IConfigurationSection httpBasicAuthConfig,
        AdOptions adOptions)
    {
        services
            .AddAuthentication(BasicAuthenticationDefaults.AuthenticationScheme)
            .AddBasicAuthentication(
                options =>
                {
                    options.Realm = "Basic Realm";
                    options.Events = new BasicAuthenticationEvents
                    {
                        OnValidatePrincipal = context =>
                        {
                            if ((context.UserName.ToLower() == httpBasicAuthConfig["Username"].ToLower())
                                && (context.Password == httpBasicAuthConfig["Password"]))
                            {
                                var groupAdmin = adOptions.AdminUserGroup;
                                var groupDev = adOptions.DevGroup ?? adOptions.NationalTeamAdGroup;
                                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name,
                                        context.UserName,
                                        context.Options.ClaimsIssuer),
                                    new Claim(ClaimTypes.Role, adOptions.BaseUserGroup, ClaimValueTypes.String),
                                    new Claim(ClaimTypes.Role, groupAdmin, ClaimValueTypes.String),
                                    new Claim(ClaimTypes.Role, groupDev, ClaimValueTypes.String)
                                };

                                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(
                                    claims,
                                    BasicAuthenticationDefaults.AuthenticationScheme));
                                var ticket = new AuthenticationTicket(
                                    claimsPrincipal,
                                    new AuthenticationProperties(),
                                    BasicAuthenticationDefaults.AuthenticationScheme);

                                context.Principal = claimsPrincipal;

                                // Returning the ticket, though it doesn't seem to be used (at least in the initial
                                // logic) - hence setting the Principal above, too.
                                return Task.FromResult(AuthenticateResult.Success(ticket));
                            }

                            return Task.FromResult(AuthenticateResult.Fail("Authentication  really failed."));
                        }
                    };
                });
    }

    private static void AddAuditService(WebApplicationBuilder builder, string auditDbConnectionString)
    {
        if (builder.Configuration.GetValue<bool>(Constants.AuditEnabledConfigValue))
        {
            builder.Services.AddEFAuditer(auditDbConnectionString);
        }
        else
        {
            Audit.Core.Configuration.AuditDisabled = true;
        }
    }

    private static void AddAdImportService(WebApplicationBuilder builder, AzureAdOptions azureAdOptions)
    {
        if (azureAdOptions.Enabled)
        {
            builder.Services.AddScoped<IAdImportService, AzureAdImportService>();
        }
        else
        {
            builder.Services.AddScoped<IAdImportService, AdImportService>();
        }
    }

    private static void AddApplicationInsightsMonitoring(IServiceCollection services,
        ApplicationInsightsOptions applicationInsightsOptions)
    {
        if (applicationInsightsOptions.Enabled == true)
        {
            services.AddApplicationInsightsTelemetry(new ApplicationInsightsServiceOptions
            {
                ConnectionString = applicationInsightsOptions.ConnectionString
            });

            services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
            {
                module.EnableSqlCommandTextInstrumentation =
                    applicationInsightsOptions.EnableSqlCommandTextInstrumentation ?? false;
            });

            if (applicationInsightsOptions.EnableProfiler == true)
            {
                services.AddServiceProfiler(options =>
                {
                    options.RandomProfilingOverhead = applicationInsightsOptions.RandomProfilingOverhead ?? 0.05F;
                    options.Duration =
                        TimeSpan.FromSeconds(applicationInsightsOptions.ProfilerDurationSeconds ?? 120);
                });
            }
        }
        else
        {
            services.AddSingleton<IJavaScriptSnippet, BlankJavaScriptSnippet>();
        }
    }

    private static void AddNotificationClusterRepository(WebApplicationBuilder builder)
    {
        var clusterMatchingConfig = builder.Configuration.GetSection(Constants.ClusterMatchingConfig);
        if (clusterMatchingConfig.GetValue<bool>(Constants.ClusterMatchingConfigMockOut))
        {
            var notificationClusterValues = new List<NotificationClusterValue>();
            clusterMatchingConfig.Bind("MockedNotificationClusterValues", notificationClusterValues);

            builder.Services.AddScoped<INotificationClusterRepository>(sp =>
                new MockNotificationClusterRepository(notificationClusterValues));
        }
        else
        {
            builder.Services.AddScoped<INotificationClusterRepository, NotificationClusterRepository>();
        }
    }

    private static void AddReferenceLabResultServices(WebApplicationBuilder builder)
    {
        var referenceLabResultsConfig = builder.Configuration.GetSection(Constants.ReferenceLabResultsConfig);
        if (referenceLabResultsConfig.GetValue<bool>(Constants.ReferenceLabResultsConfigMockOut))
        {
            var notificationId = referenceLabResultsConfig.GetValue<int>("MockedNotificationId");
            var tbServiceCode = referenceLabResultsConfig.GetValue<string>("MockedTbServiceCode");
            var phecCode = referenceLabResultsConfig.GetValue<string>("MockedPhecCode");
            builder.Services.AddScoped<ICultureAndResistanceService>(
                sp => new MockCultureAndResistanceService(notificationId));
            builder.Services.AddScoped<ISpecimenService>(
                sp => new MockSpecimenService(notificationId, tbServiceCode, phecCode));
        }
        else
        {
            builder.Services.AddScoped<ICultureAndResistanceService, CultureAndResistanceService>();
            builder.Services.AddScoped<ISpecimenService, SpecimenService>();
        }
    }

    private static void AddReportingServices(WebApplicationBuilder builder)
    {
        if (string.IsNullOrEmpty(builder.Configuration.GetConnectionString(Constants.DbConnectionStringReporting)))
        {
            builder.Services.AddScoped<IHomepageKpiService, MockHomepageKpiService>();
            builder.Services.AddScoped<IDrugResistanceProfileService, MockDrugResistanceProfileService>();
        }
        else
        {
            builder.Services.AddScoped<IHomepageKpiService, HomepageKpiService>();
            builder.Services.AddScoped<IDrugResistanceProfileService, DrugResistanceProfileService>();
        }
    }

    private static void AddMicrosoftGraphServices(IServiceCollection services, AzureAdOptions azureAdOptions)
    {
        if (azureAdOptions.Enabled)
        {
            services.AddScoped<IAzureAdDirectoryServiceFactory, AzureAdDirectoryServiceFactory>();
            services.AddScoped<IAzureAdDirectoryService, AzureAdDirectoryService>();
        }
    }
}
