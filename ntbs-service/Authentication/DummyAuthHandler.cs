using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Authentication
{
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
            id.AddClaim(new Claim(ClaimTypes.Role, adfsOptions.AdGroupsPrefix + adfsOptions.BaseUserGroup, ClaimValueTypes.String));
            // Add role claim for user role - as specified in appsettings.Development.json
            string groupAdmin = adfsOptions.AdGroupsPrefix + (adfsOptions.AdminUserGroup ?? adfsOptions.NationalTeamAdGroup);
            id.AddClaim(new Claim(ClaimTypes.Role, groupAdmin, ClaimValueTypes.String));
            string groupDev = adfsOptions.AdGroupsPrefix + (adfsOptions.DevGroup ?? adfsOptions.NationalTeamAdGroup);
            id.AddClaim(new Claim(ClaimTypes.Role, groupDev, ClaimValueTypes.String));

            id.AddClaim(new Claim(ClaimTypes.Email, "Developer@ntbs.phe.com", ClaimValueTypes.String));
            this.id = new ClaimsPrincipal(id);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(id, Name)));
    }
}
