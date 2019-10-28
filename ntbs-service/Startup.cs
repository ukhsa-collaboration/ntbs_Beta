using Microsoft.AspNetCore.Authentication.Cookies;
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
using ntbs_service.Data.Legacy;
using ntbs_service.Models;
using ntbs_service.Services;
using System.Globalization;
using EFAuditer;
using ntbs_service.Middleware;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Authentication;

namespace ntbs_service
{
  public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


            var setupDummyAuth = Env.IsDevelopment() || Env.IsEnvironment("Test");
            IConfigurationSection adfsConfig = Configuration.GetSection("AdfsOptions");
            var authSetup = services.AddAuthentication(sharedOptions =>
                    {
                        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                    }).AddWsFederation(options =>
                    {
                        options.MetadataAddress = adfsConfig["AdfsUrl"] + "/FederationMetadata/2007-06/FederationMetadata.xml";
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
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

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
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<ISearchServiceLegacy, SearchServiceLegacy>();
            services.AddScoped<IETSSearchService, ETSSearcher>();
            services.AddScoped<ILTBRSearchService, LTBRSearcher>();
            services.AddScoped<IAnnualReportSearchService, AnnualReportSearcher>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IAuditService, AuditService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IPostcodeService, PostcodeService>();
            services.AddScoped<Services.IAuthorizationService, AuthorizationService>();

            services.Configure<AdfsOptions>(adfsConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (Env.IsEnvironment("Test"))
            {
                app.UseDeveloperExceptionPage();
            }
            else if (env.IsDevelopment())
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            if (Configuration.GetValue<bool>(Constants.AUDIT_ENABLED_CONFIG_VALUE))
            {
                app.UseMiddleware<AuditGetRequestMiddleWare>();
            }

            app.UseMvc();

            var cultureInfo = new CultureInfo("en-GB");
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        }
    }
}
