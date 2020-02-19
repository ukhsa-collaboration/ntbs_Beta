using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.DataAccess
{
    public interface IUserRepository
    {
        Task AddOrUpdateUser(User user, IEnumerable<TBService> tbServices);
        Task AddUserLoginEvent(UserLoginEvent userLoginEvent);
    }

    public class UserRepository : IUserRepository
    {
        private readonly NtbsContext _context;

        public UserRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task AddOrUpdateUser(User user, IEnumerable<TBService> tbServices)
        {
            var existingUser = await _context.User.Include(u => u.CaseManagerTbServices)
                .SingleOrDefaultAsync(u => u.Username == user.Username);

            if (existingUser != null)
            {
                await UpdateUser(existingUser, user, tbServices);
            }
            else
            {
                await AddUser(user, tbServices);
            }
        }

        public async Task AddUserLoginEvent(UserLoginEvent userLoginEvent)
        {
            _context.UserLoginEvent.Add(userLoginEvent);
            await _context.SaveChangesAsync();
        }

        private async Task AddUser(User user, IEnumerable<TBService> tbServices)
        {
            _context.User.Add(user);
            SyncCaseManagerTbServices(user, tbServices);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateUser(User existingUser, User newUser, IEnumerable<TBService> tbServices)
        {
            // This seemingly un-needed line is actually a hack around the fact that sql server performs
            // case insensitive string comparison.
            newUser.Username = existingUser.Username;
            _context.Entry(existingUser).CurrentValues.SetValues(newUser);
            SyncCaseManagerTbServices(existingUser, tbServices);
            await _context.SaveChangesAsync();
        }

        private static void SyncCaseManagerTbServices(User user, IEnumerable<TBService> tbServices)
        {
            var caseManagerTbServices = tbServices
                .Select(tb => new CaseManagerTbService
                {
                    TbServiceCode = tb.Code,
                    CaseManagerUsername = user.Username
                })
                .ToList();

            if (user.CaseManagerTbServices == null)
            {
                user.CaseManagerTbServices = caseManagerTbServices;
            }
            else
            {
                RemoveUnmatchedCaseManagerTbServices(user.CaseManagerTbServices, caseManagerTbServices);

                foreach (var caseManagerTbService in caseManagerTbServices)
                {
                    if (!user.CaseManagerTbServices.Any(c => c.Equals(caseManagerTbService)))
                    {
                        user.CaseManagerTbServices.Add(caseManagerTbService);
                    }
                }
            }
        }

        private static void RemoveUnmatchedCaseManagerTbServices(
            ICollection<CaseManagerTbService> userCaseManagerTbServices,
            IList<CaseManagerTbService> adCaseManagerTbServices)
        {
            var caseManagerTbServicesToRemove = new List<CaseManagerTbService>();
            foreach (var caseManagerTbService in userCaseManagerTbServices)
            {
                if (!adCaseManagerTbServices.Any(c => caseManagerTbService.Equals(c)))
                {
                    caseManagerTbServicesToRemove.Add(caseManagerTbService);
                }
            }
            foreach (var caseManagerTbService in caseManagerTbServicesToRemove)
            {
                userCaseManagerTbServices.Remove(caseManagerTbService);
            }
        }
    }
}
