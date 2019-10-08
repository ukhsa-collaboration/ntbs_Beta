using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ntbs_service.Authentication {
    public class DevAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public static readonly string DevAuthName = "Dev";

        private readonly ClaimsPrincipal id;

        public DevAuthHandler(IOptionsMonitor<AdfsOptions> AdfsOptionsMonitor,
                              IOptionsMonitor<AuthenticationSchemeOptions> options,
                              ILoggerFactory logger,
                              UrlEncoder encoder,
                              ISystemClock clock) : base(options, logger, encoder, clock)
        {
            var id = new ClaimsIdentity(DevAuthName);
            id.AddClaim(new Claim(ClaimTypes.Name, "Devloper", ClaimValueTypes.String));
            var adfsOptions = AdfsOptionsMonitor.CurrentValue;
            id.AddClaim(new Claim(ClaimTypes.Role, adfsOptions.AdGroupsPrefix + adfsOptions.BaseUserGroup, ClaimValueTypes.String));
            string group = adfsOptions.AdGroupsPrefix + 
                (adfsOptions.DevGroup == null ? adfsOptions.DevGroup : adfsOptions.NationalTeamAdGroup);
            id.AddClaim(new Claim(ClaimTypes.Role, group, ClaimValueTypes.String));
            this.id = new ClaimsPrincipal(id);
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            => Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(id, DevAuthName)));
    }
}