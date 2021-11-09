using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;

namespace ntbs_service.DataAccess
{
    public interface IUserRepository
    {
        Task AddOrUpdateUser(User user, IEnumerable<TBService> tbServices, bool allowIsCaseManagerUpdate = true);
        Task AddUserLoginEvent(UserLoginEvent userLoginEvent);
        Task<User> GetUserByUsername(string username);
        Task<User> GetUserById(int id);
        IQueryable<User> GetUserQueryable();
        Task<IList<User>> GetOrderedUsers();
        Task UpdateUserContactDetails(User user);
        Task<Dictionary<string, string>> GetUsernameDictionary();
        Task<Dictionary<string, int>> GetIdDictionary();
    }

    public class UserRepository : IUserRepository
    {
        private readonly NtbsContext _context;
        private readonly AdOptions _adOptions;

        public UserRepository(NtbsContext context, IOptionsMonitor<AdOptions> adOptions)
        {
            _adOptions = adOptions.CurrentValue;
            _context = context;
        }

        public IQueryable<User> GetUserQueryable()
        {
            return this._context.User
                .Include(u => u.CaseManagerTbServices)
                .ThenInclude(c => c.TbService)
                .ThenInclude(tb => tb.PHEC);
        }

        public async Task AddOrUpdateUser(User user, IEnumerable<TBService> tbServices, bool allowIsCaseManagerUpdate = true)
        {
            var existingUser = await GetUserQueryable()
                .SingleOrDefaultAsync(u => u.Username == user.Username);
            user.IsReadOnly = user.AdGroups?.Contains(_adOptions.ReadOnlyUserGroup) ?? false;

            if (existingUser != null)
            {
                await UpdateUserAdDetails(existingUser, user, tbServices, allowIsCaseManagerUpdate);
            }
            else
            {
                await AddUser(user, tbServices);
            }
        }

        public async Task AddUserLoginEvent(UserLoginEvent userLoginEvent)
        {
            _context.AddAuditCustomField(CustomFields.AppUser, userLoginEvent.Username);
            await _context.UserLoginEvent.AddAsync(userLoginEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await GetUserQueryable()
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            var user = await GetUserQueryable()
                .FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<IList<User>> GetOrderedUsers()
        {
            return await GetUserQueryable()
                .OrderBy(u => u.DisplayName)
                .ToListAsync();
        }

        /// <summary>
        /// Our user models consist of values that come from the AD (automatically synced) and others that are supplied
        /// by the user. This method is used for updating the latter, in a way that does not wipe data from the
        /// user-sync fields.
        ///
        /// See also UpdateUserAdDetails method in this class
        /// </summary>
        public async Task UpdateUserContactDetails(User user)
        {
            var existingUser = await _context.User.SingleAsync(u => u.Username == user.Username);
            existingUser.JobTitle = user.JobTitle;
            existingUser.PhoneNumberPrimary = user.PhoneNumberPrimary;
            existingUser.PhoneNumberSecondary = user.PhoneNumberSecondary;
            existingUser.EmailPrimary = user.EmailPrimary;
            existingUser.EmailSecondary = user.EmailSecondary;
            existingUser.Notes = user.Notes;
            existingUser.IsCaseManager = user.IsCaseManager;
            await _context.SaveChangesAsync();
        }

        public Task<Dictionary<string, string>> GetUsernameDictionary()
        {
            return _context.User.ToDictionaryAsync(
                user => user.Username,
                user => user.DisplayName
            );
        }

        public Task<Dictionary<string, int>> GetIdDictionary()
        {
            return _context.User.ToDictionaryAsync(
                user => user.Username,
                user => user.Id
            );
        }

        private async Task AddUser(User user, IEnumerable<TBService> tbServices)
        {
            await _context.User.AddAsync(user);
            SyncCaseManagerTbServices(user, tbServices);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Our user models consist of values that come from the AD (automatically synced) and others that are supplied
        /// by the user. This method is used for updating the former, in a way that does not wipe data from the
        /// user-supplied fields.
        ///
        /// See also UpdateUserContactDetails method in this class
        /// </summary>
        private async Task UpdateUserAdDetails(User existingUser, User newUser, IEnumerable<TBService> tbServices, bool allowIsCaseManagerUpdate = true)
        {
            existingUser.GivenName = newUser.GivenName;
            existingUser.FamilyName = newUser.FamilyName;
            existingUser.DisplayName = newUser.DisplayName;
            existingUser.AdGroups = newUser.AdGroups;
            existingUser.IsActive = newUser.IsActive;
            existingUser.IsReadOnly = newUser.IsReadOnly;
            if (allowIsCaseManagerUpdate)
            {
                existingUser.IsCaseManager = newUser.IsCaseManager;
            }
            SyncCaseManagerTbServices(existingUser, tbServices);
            await _context.SaveChangesAsync();
        }

        private void SyncCaseManagerTbServices(User user, IEnumerable<TBService> tbServices)
        {
            var caseManagerTbServices = tbServices
                .Select(tb => new CaseManagerTbService
                {
                    TbServiceCode = tb.Code,
                    CaseManagerId = user.Id
                })
                .ToList();

            if (user.CaseManagerTbServices == null)
            {
                user.CaseManagerTbServices = caseManagerTbServices;
            }
            else
            {
                RemoveUnmatchedTbServicesFromCaseManager(user.CaseManagerTbServices, caseManagerTbServices);
                AddNewTbServicesToCaseManager(user.CaseManagerTbServices, caseManagerTbServices);
            }
        }

        private void RemoveUnmatchedTbServicesFromCaseManager(
            ICollection<CaseManagerTbService> existingTbServices,
            IList<CaseManagerTbService> newTbServices)
        {
            foreach (var caseManagerTbService in existingTbServices)
            {
                if (!newTbServices.Any(c => c.TbServiceCode == caseManagerTbService.TbServiceCode))
                {
                    _context.Remove(caseManagerTbService);
                }
            }
        }

        private static void AddNewTbServicesToCaseManager(
            ICollection<CaseManagerTbService> existingTbServices,
            IList<CaseManagerTbService> newTbServices)
        {
            foreach (var caseManagerTbService in newTbServices)
            {
                if (!existingTbServices.Any(c => c.TbServiceCode == caseManagerTbService.TbServiceCode))
                {
                    existingTbServices.Add(caseManagerTbService);
                }
            }
        }
    }
}
