using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Properties;
using ntbs_service_unit_tests.TestHelpers;
using Xunit;

namespace ntbs_service_unit_tests.DataAccess
{
    public class UserRepositoryTests : IClassFixture<RepositoryFixture<UserRepository>>, IDisposable
    {
        private readonly NtbsContext _context;
        private readonly DbContextOptions<NtbsContext> _contextOptions;
        private readonly UserRepository _userRepo;
        private readonly TBService _tbService1;
        private readonly TBService _tbService2;

        public UserRepositoryTests(RepositoryFixture<UserRepository> userRepositoryFixture)
        {
            ContextHelper.DisableAudits();
            _context = userRepositoryFixture.Context;
            _contextOptions = userRepositoryFixture.ContextOptions;

            var optionsMonitor = new Mock<IOptionsMonitor<AdOptions>>();
            optionsMonitor.Setup(om => om.CurrentValue).Returns(new AdOptions { ReadOnlyUserGroup = "ReadOnly" });
            _userRepo = new UserRepository(_context, optionsMonitor.Object);

            var phec = new PHEC { Code = "E45000001", Name = "London" };
            _tbService1 = new TBService { Code = "TBS0001", IsLegacy = false, PHECCode = "E45000001" };
            _tbService2 = new TBService { Code = "TBS0002", IsLegacy = false, PHECCode = "E45000001" };
            _context.PHEC.Add(phec);
            _context.TbService.AddRange(_tbService1, _tbService2);
            _context.SaveChanges();
        }

        [Fact]
        public async Task AddOrUpdateData_AddsCaseManagerTbService_ToExistingUser()
        {
            // Arrange
            await AddUserAndTbServices(CreateUser("user1"), null);

            var updateUser = CreateUser("user1");
            updateUser.IsCaseManager = true;
            var updateTbServices = new[] { _tbService1 };

            // Act
            await _userRepo.AddOrUpdateUser(updateUser, updateTbServices);

            // Assert
            var updatedUser = GetUserUsingNewContext("user1");
            Assert.NotNull(updatedUser);
            Assert.True(updatedUser.IsCaseManager);
            Assert.Collection(updatedUser.CaseManagerTbServices,
                cmtbs => Assert.Equal(_tbService1.Code, cmtbs.TbService.Code));
        }

        [Fact]
        public async Task AddOrUpdateData_AddsAnotherCaseManagerTbService_ToExistingUser()
        {
            // Arrange
            const string username = "user2";
            await AddUserAndTbServices(CreateUser(username), new[] { _tbService1 });

            var updateUser = CreateUser(username);
            updateUser.IsCaseManager = true;
            var updateTbServices = new[] { _tbService1, _tbService2 };

            // Act
            await _userRepo.AddOrUpdateUser(updateUser, updateTbServices);

            // Assert
            var updatedUser = GetUserUsingNewContext(username);
            Assert.NotNull(updatedUser);
            Assert.True(updatedUser.IsCaseManager);
            Assert.Equal(2, updatedUser.CaseManagerTbServices.Count);
            Assert.Contains(updatedUser.CaseManagerTbServices, cmtbs => _tbService1.Code == cmtbs.TbService.Code);
            Assert.Contains(updatedUser.CaseManagerTbServices, cmtbs => _tbService2.Code == cmtbs.TbService.Code);
        }

        [Fact]
        public async Task AddOrUpdateData_RemovesOnlyCaseManagerTbService_FromExistingUser()
        {
            // Arrange
            const string username = "user3";
            await AddUserAndTbServices(CreateUser(username), new[] { _tbService1 });

            var updateUser = CreateUser(username);
            updateUser.IsCaseManager = false;
            var updateTbServices = Enumerable.Empty<TBService>();

            // Act
            await _userRepo.AddOrUpdateUser(updateUser, updateTbServices);

            // Assert
            var updatedUser = GetUserUsingNewContext(username);
            Assert.NotNull(updatedUser);
            Assert.False(updatedUser.IsCaseManager);
            Assert.Empty(updatedUser.CaseManagerTbServices);
        }

        [Fact]
        public async Task AddOrUpdateData_RemovesSingleCaseManagerTbService_FromExistingUser()
        {
            // Arrange
            const string username = "user4";
            await AddUserAndTbServices(CreateUser(username), new[] { _tbService1, _tbService2 });

            var updateUser = CreateUser(username);
            updateUser.IsCaseManager = true;
            var updateTbServices = new[] { _tbService1 };

            // Act
            await _userRepo.AddOrUpdateUser(updateUser, updateTbServices);

            // Assert
            var updatedUser = GetUserUsingNewContext(username);
            Assert.NotNull(updatedUser);
            Assert.True(updatedUser.IsCaseManager);
            Assert.Collection(updatedUser.CaseManagerTbServices,
                cmtbs => Assert.Equal(_tbService1.Code, cmtbs.TbService.Code));
        }

        private User GetUserUsingNewContext(string username)
        {
            using (var newContext = new NtbsContext(_contextOptions))
            {
                var updatedUser = newContext.User
                    .Include(u => u.CaseManagerTbServices)
                    .ThenInclude(cmtbs => cmtbs.TbService)
                    .FirstOrDefault(u => u.Username == username);
                return updatedUser;
            }
        }

        private static User CreateUser(string username) => new User {
            Username = username,
            IsActive = true,
            IsReadOnly = false
        };

        private async Task AddUserAndTbServices(User user, IList<TBService> tbServices)
        {
            user.IsCaseManager = tbServices?.Any() ?? false;
            user.CaseManagerTbServices =
                tbServices?
                    .Select(tbs => new CaseManagerTbService { TbService = tbs, CaseManager = user })
                    .ToList();

            _context.User.Add(user);
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.TbService.RemoveRange(_context.TbService);
            _context.PHEC.RemoveRange(_context.PHEC);
            _context.User.RemoveRange(_context.User);
            _context.SaveChanges();
        }
    }
}
