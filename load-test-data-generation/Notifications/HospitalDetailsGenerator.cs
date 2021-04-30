using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace load_test_data_generation.Notifications
{
    internal class HospitalDetailsGenerator
    {
        private readonly IContextProvider contextProvider;

        private List<Hospital> hospitals;
        private List<TBService> tbServices;
        private List<User> caseManagers;
        private Faker<HospitalDetails> testHospitalDetails;

        public HospitalDetailsGenerator(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void Initialise()
        {
            hospitals = contextProvider.WithContext(context => context.Hospital.Where(h => !h.IsLegacy).ToList());
            tbServices = contextProvider.WithContext(context => context.TbService.Where(s => !s.IsLegacy).ToList());
            caseManagers = contextProvider.WithContext(context =>
                {
                    return context.User
                        .Include(u => u.CaseManagerTbServices)
                        .Where(u => u.IsCaseManager)
                        .ToList();
                });

            testHospitalDetails = new Faker<HospitalDetails>()
                .RuleFor(h => h.TBServiceCode, f => f.PickRandom(tbServices).Code)
                .RuleFor(h => h.HospitalId, (f, hd) => f.PickRandom(hospitals.Where(h => h.TBServiceCode == hd.TBServiceCode)).HospitalId)
                .RuleFor(h => h.CaseManagerId, (f, hd) => f.PickRandom(caseManagers.Where(cm => cm.CaseManagerTbServices.Any(s => s.TbServiceCode == hd.TBServiceCode))).Id);
        }

        public HospitalDetails GenerateHospitalDetails()
        {
            if (testHospitalDetails == null)
            {
                throw new InvalidOperationException("This class must be initialised before hospital details can be generated.");
            }
            return testHospitalDetails.Generate();
        }
    }
}
