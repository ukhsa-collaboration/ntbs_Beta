using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Properties;

namespace ntbs_integration_tests.TestServices
{
    public static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<Startup> WithUserAuth(this WebApplicationFactory<Startup> factory, ITestUser user)
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped(_ => user);
                    services.AddAuthentication(UserAuthentication.SchemeName)
                        .AddScheme<AuthenticationSchemeOptions, UserAuthentication>(UserAuthentication.SchemeName, o => { });
                });
            });
        }
    }

    public class UserAuthentication : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string SchemeName = "TestAuth";

        private readonly ITestUser user;
        private readonly AdOptions adOptions;

        public UserAuthentication(
            ITestUser user,
            IOptionsMonitor<AdOptions> adOptions,
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
            this.user = user;
            this.adOptions = adOptions.CurrentValue;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var identity = new ClaimsIdentity(SchemeName);
            identity.AddClaim(new Claim(ClaimTypes.Upn, user.Username));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.DisplayName));
            identity.AddClaim(new Claim(ClaimTypes.Role, adOptions.BaseUserGroup));
            foreach (var adGroup in user.AdGroups)
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, adGroup));
            }
            var claimsPrincipal = new ClaimsPrincipal(identity);
            return Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, SchemeName)));
        }
    }
}
