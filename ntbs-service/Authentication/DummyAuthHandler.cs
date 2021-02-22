using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Authentication
{
    // This class is used to artificially authenticate requests when running tests.
    // Tech debt: we should move this out of production code.
    public class DummyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static readonly string Name = "DummyAuth";

        private readonly ClaimsPrincipal id;

        public DummyAuthHandler(IOptionsMonitor<AdfsOptions> AdfsOptionsMonitor,
                              IOptionsMonitor<AuthenticationSchemeOptions> options,
                              ILoggerFactory logger,
                              UrlEncoder encoder,
                              ISystemClock clock) : base(options, logger, encoder, clock)
        {
            var id = new ClaimsIdentity(Name);
            // Add name claim
            id.AddClaim(new Claim(ClaimTypes.Name, "Developer", ClaimValueTypes.String));
            var adfsOptions = AdfsOptionsMonitor.CurrentValue;

            // Add role claim for base user role
            id.AddClaim(new Claim(ClaimTypes.Role, adfsOptions.BaseUserGroup, ClaimValueTypes.String));

            // Add role claim for user role - as specified in appsettings.Development.json
            string groupDev = adfsOptions.DevGroup ?? adfsOptions.NationalTeamAdGroup;
            id.AddClaim(new Claim(ClaimTypes.Role, groupDev, ClaimValueTypes.String));

            // Add role claim for user role - Admin
            string groupAdmin = adfsOptions.AdminUserGroup;
            id.AddClaim(new Claim(ClaimTypes.Role, groupAdmin, ClaimValueTypes.String));

            id.AddClaim(new Claim(ClaimTypes.Upn, "Developer@ntbs.phe.com", ClaimValueTypes.String));
            this.id = new ClaimsPrincipal(id);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(id, Name)));
    }
}
