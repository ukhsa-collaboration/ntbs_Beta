using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using Serilog;

namespace ntbs_service.Services
{
    public interface IAdUserService
    {
        Task AddAndUpdateUsers(IList<(User user, IList<TBService>)> usersInAd);
    }
    public class AdUserService : IAdUserService
    {
        private readonly IUserRepository _userRepository;

        public AdUserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task AddAndUpdateUsers(IList<(User user, IList<TBService>)> usersInAd)
        {
            foreach (var (user, tbServicesMatchingGroups) in usersInAd)
            {
                Log.Information($"Updating user {user.Username}");
                await _userRepository.AddOrUpdateUser(user, tbServicesMatchingGroups);
            }

            var ntbsUsersNotInAd = this._userRepository.GetUserQueryable()
                .Where(user => !usersInAd.Select(u => u.user.Username)
                    .Contains(user.Username)).ToList();
            foreach (var user in ntbsUsersNotInAd)
            {
                Log.Information($"Updating user {user.Username}");
                user.IsActive = false;
                user.AdGroups = null;
                await _userRepository.AddOrUpdateUser(user, user.CaseManagerTbServices.Select(cmtb => cmtb.TbService));
            }
        }
    }
}
