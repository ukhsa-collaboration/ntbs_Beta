using System.Collections.Generic;
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
            AddCaseManagerTbServices(user, tbServices);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateUser(User existingUser, User newUser, IEnumerable<TBService> tbServices)
        {
            // This seemingly un-needed line is actually a hack around the fact that sql server performs
            // case insensitive string comparison.
            newUser.Username = existingUser.Username;
            _context.Entry(existingUser).CurrentValues.SetValues(newUser);
            AddCaseManagerTbServices(existingUser, tbServices);
            await _context.SaveChangesAsync();
        }

        private static void AddCaseManagerTbServices(User user, IEnumerable<TBService> tbServices)
        {
            var caseManagerTbServices = tbServices
                .Select(tb => new CaseManagerTbService {TbService = tb, CaseManager = user})
                .ToList();
            user.CaseManagerTbServices = caseManagerTbServices.Any() ? caseManagerTbServices : null;
        }
    }
}
