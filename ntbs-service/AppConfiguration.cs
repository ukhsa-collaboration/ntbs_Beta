using System.Globalization;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using ntbs_service.DataMigration;
using ntbs_service.Jobs;
using ntbs_service.Middleware;
using ntbs_service.Properties;
using Serilog;
using Serilog.Events;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

namespace ntbs_service;

public static class AppConfiguration
{
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public static void Configure(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseForwardedHeaders();
            app.UseDeveloperExceptionPage();

            // TODO Find an alternative for using webpack middleware in dotnet 5.0
            // app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
            // {
            //     HotModuleReplacement = true, ConfigFile = "webpack.dev.js"
            // });
            // We only need to turn this on in development, as in production this
            // This behaviour is by default provided by the nginx ingress
            // (see https://kubernetes.github.io/ingress-nginx/user-guide/nginx-configuration/annotations/#server-side-https-enforcement-through-redirect)
            // (also see  HSTS setting below)
            app.UseHttpsRedirection();
        }
        else
        {
            app.UseForwardedHeaders();
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseExceptionHandler("/errors/500");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseStaticFiles();

        if (!app.Environment.IsEnvironment("CI"))
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
                // 400s get thrown e.g. on antiforgery token validation failures. In those cases we don't have
                // an exception logged in Sentry, so we want to log at Warning level to make sure we are able to
                // identify and cure false positives.
                // Otherwise setting to Information to prevent duplicated exceptions in sentry.
                options.GetLevel = (context, _, __) => context.Response.StatusCode == StatusCodes.Status400BadRequest
                    ? LogEventLevel.Warning
                    : LogEventLevel.Information;
            });
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy();
        app.UseSession();

        app.UseMiddleware<MsOfficeLinkPrefetchMiddleware>();

        if (!app.Environment.IsEnvironment("CI"))
        {
            app.UseMiddleware<ActivityDetectionMiddleware>();
        }

        if (app.Configuration.GetValue<bool>(Constants.AuditEnabledConfigValue))
        {
            app.UseMiddleware<AuditGetRequestMiddleWare>();
        }

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapRazorPages();
            endpoints.MapControllers();
        });

        ConfigureHangfire(app);

        var cultureInfo = new CultureInfo("en-GB");
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
    }

    private static void ConfigureHangfire(WebApplication app)
    {
        if (!app.Configuration.GetValue<bool>(Constants.HangfireEnabled))
        {
            return;
        }

        var dashboardOptions = new DashboardOptions
        {
            Authorization =
                new[] { new HangfireAuthorisationFilter(app.Configuration.GetSection("AdOptions")["AdminUserGroup"]) },
            DisplayStorageConnectionString = false,
        };
        app.UseHangfireDashboard("/hangfire", dashboardOptions);
        GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

        var scheduledJobConfig = new ScheduledJobsConfig();
        app.Configuration.GetSection(Constants.ScheduledJobsConfig).Bind(scheduledJobConfig);
        HangfireJobScheduler.ScheduleRecurringJobs(scheduledJobConfig);
    }
}
