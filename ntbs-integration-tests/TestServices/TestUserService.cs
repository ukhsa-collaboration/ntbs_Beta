using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_integration_tests.TestServices
{

    public static class TestUserServiceExtensions
    {
        public static WebApplicationFactory<Startup> WithUser<T>(this WebApplicationFactory<Startup> factory)
            where T : class, IUserService
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUserService, T>();
                });
            });
        }
    }

    public class NhsUserForAbingdonAndPermitted : IUserService
    {
        private readonly List<string> tbServiceCode = new List<string>
        {
            Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID, Utilities.PERMITTED_SERVICE_CODE
        };
        
        public Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(new UserPermissionsFilter()
            {
                Type = UserType.NhsUser,
                IncludedTBServiceCodes = tbServiceCode
            });
        }

        public Task<TBService> GetDefaultTbService(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(tbServiceCode.Select(x => new TBService { Code = x }));
        }

        public UserType GetUserType(ClaimsPrincipal user)
        {
            return UserType.NhsUser;
        }

        public Task<IEnumerable<string>> GetPhecCodesAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
        
        public Task RecordUserLoginAsync(string username)
        {
            return Task.CompletedTask;
        }
    }
    
    public class NhsUserWithNoTbServices : IUserService
    {
        public async Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return await Task.FromResult(new UserPermissionsFilter
            {
                Type = UserType.NhsUser, 
                IncludedTBServiceCodes = new List<string>()
            });
        }

        public Task<TBService> GetDefaultTbService(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(new List<TBService>() as IEnumerable<TBService>);
        }

        public UserType GetUserType(ClaimsPrincipal user)
        {
            return UserType.NhsUser;
        }

        public Task<IEnumerable<string>> GetPhecCodesAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }
        
        public Task RecordUserLoginAsync(string username)
        {
            return Task.CompletedTask;
        }
    }

    public class PheUserWithPermittedPhecCode : IUserService
    {
        public Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(new UserPermissionsFilter
            {
                Type = UserType.PheUser, IncludedPHECCodes = new List<string> {Utilities.PERMITTED_PHEC_CODE}
            });
        }

        public Task<TBService> GetDefaultTbService(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(Enumerable.Empty<TBService>());
        }

        public UserType GetUserType(ClaimsPrincipal user)
        {
            return UserType.PheUser;
        }

        public Task<IEnumerable<string>> GetPhecCodesAsync(ClaimsPrincipal user)
        {
            var phecCodes = new List<string> {Utilities.PERMITTED_PHEC_CODE};
            return Task.FromResult((IEnumerable<string>)phecCodes);
        }
        
        public Task RecordUserLoginAsync(string username)
        {
            return Task.CompletedTask;
        }
    }
    
    public class NationalTeamUser : IUserService
    {
        public Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return Task.FromResult(new UserPermissionsFilter {Type = UserType.NationalTeam});
        }

        public Task<TBService> GetDefaultTbService(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public UserType GetUserType(ClaimsPrincipal user)
        {
            return UserType.NationalTeam;
        }

        public Task<IEnumerable<string>> GetPhecCodesAsync(ClaimsPrincipal user)
        {
            throw new NotImplementedException();
        }

        public Task RecordUserLoginAsync(string username)
        {
            return Task.CompletedTask;
        }
    }
}
