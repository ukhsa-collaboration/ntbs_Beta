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

        private async Task AddUser(User user, IEnumerable<TBService> tbServices)
        {
            AddCaseManagerTbServices(user, tbServices);
            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateUser(User existingUser, User newUser, IEnumerable<TBService> tbServices)
        {
            DetachLocalTrackedObject(existingUser);

            existingUser = newUser;
            AddCaseManagerTbServices(existingUser, tbServices);
            _context.Entry(existingUser).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        private void DetachLocalTrackedObject(User existingUser)
        {
            // Not sure why this is necessary/what is tracking the same entity twice, but without kept getting error:
            // "The instance of entity type cannot be tracked because another instance of this type with the same key [Username] is already being tracked"
            // Solution from https://stackoverflow.com/questions/36856073/the-instance-of-entity-type-cannot-be-tracked-because-another-instance-of-this-t
            // TODO: Work out if there is a nicer way.
            var local = _context.Set<User>()
                            .Local
                            .FirstOrDefault(entry => entry.Username.Equals(existingUser.Username));

            if (local != null)
            {
                _context.Entry(local).State = EntityState.Detached;
            }
        }

        private void AddCaseManagerTbServices(User user, IEnumerable<TBService> tbServices)
        {
            if (tbServices.Any())
            {
                user.CaseManagerTbServices = tbServices.Select(tb => new CaseManagerTbService { TbService = tb, CaseManager = user }).ToList();
            }
            else
            {
                user.CaseManagerTbServices = null;
            }
        }
    }
}