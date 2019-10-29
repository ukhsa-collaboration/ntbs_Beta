using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_integration_tests.TestServices
{
    public class NhsUserService : IUserService
    {
        public Task<TBService> GetDefaultTBService(ClaimsPrincipal user)
        {
            return null;
        }

        public Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(new UserPermissionsFilter()
            {
                Type = UserType.NhsUser,
                IncludedTBServiceCodes = new List<string>() { Utilities.PERMITTED_SERVICE_CODE }
            });
        }
    }

    public class PheUserService : IUserService
    {
        public Task<TBService> GetDefaultTBService(ClaimsPrincipal user)
        {
            return null;
        }

        public Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(new UserPermissionsFilter()
            {
                Type = UserType.PheUser,
                IncludedPHECCodes = new List<string>() { Utilities.PERMITTED_PHEC_CODE }
            });
        }
    }
}