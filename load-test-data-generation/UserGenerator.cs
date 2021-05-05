using System.Collections.Generic;
using System.Linq;
using Bogus;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace load_test_data_generation
{
    internal class UserGenerator
    {
        private readonly IContextProvider contextProvider;

        private readonly Faker<User> testUsers = new Faker<User>()
               .RuleFor(u => u.GivenName, f => f.Name.FirstName())
               .RuleFor(u => u.FamilyName, f => f.Name.LastName())
               .RuleFor(u => u.Username, (f, u) => f.Internet.Email(u.GivenName, u.FamilyName))
               .RuleFor(u => u.DisplayName, (f, u) => $"{u.GivenName} {u.FamilyName}")
               .RuleFor(u => u.IsActive, f => true)
               .RuleFor(u => u.IsCaseManager, f => true)
               .RuleFor(u => u.PhoneNumberPrimary, f => f.Phone.PhoneNumber("0#### ######"))
               .RuleFor(u => u.PhoneNumberSecondary, f => f.Phone.PhoneNumber("0#### ######"))
               .RuleFor(u => u.JobTitle, f => f.Name.JobTitle())
               .RuleFor(u => u.EmailPrimary, (f, u) => u.Username)
               .RuleFor(u => u.EmailSecondary, (f, u) => f.Internet.Email())
               .RuleFor(u => u.Notes, f => f.Lorem.Sentence());

        public UserGenerator(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void GenerateUsers()
        {
            var users = GetTbServices().Select(CreateUserForTbService);
            SaveUsers(users);
        }

        private User CreateUserForTbService(TBService tbService)
        {
            var user = testUsers.Generate();
            user.AdGroups = tbService.ServiceAdGroup;
            user.CaseManagerTbServices = new List<CaseManagerTbService>
            {
                new CaseManagerTbService { TbServiceCode = tbService.Code }
            };
            return user;
        }

        private List<TBService> GetTbServices()
        {
            return contextProvider.WithContext(context =>
            {
                return context.TbService
                   .Where(tbService => !tbService.IsLegacy)
                   .ToList();
            });
        }

        private void SaveUsers(IEnumerable<User> users)
        {
            contextProvider.WithContext(context =>
            {
                context.AddRange(users);
                context.SaveChanges();
            });
        }
    }
}
