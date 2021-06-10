using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace load_test_data_generation.Notifications
{
    internal class PatientDetailsGenerator
    {
        private readonly IContextProvider contextProvider;

        private static readonly DateTime StartOf1920 = new DateTime(1920, 01, 01, 00, 00, 00);
        // The notifications we're generating are all from 2015 onwards, so this ensures no patient
        // was born after they got TB!
        private static readonly DateTime EndOf2014 = new DateTime(2014, 12, 31, 23, 59, 59);

        private List<Country> countries;
        private List<Ethnicity> ethnicities;
        private List<Sex> sexes;
        private Faker<PatientDetails> testPatientDetails;

        public PatientDetailsGenerator(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void Initialise()
        {
            countries = contextProvider.WithContext(context => context.Country.ToList());
            ethnicities = contextProvider.WithContext(context => context.Ethnicity.ToList());
            sexes = contextProvider.WithContext(context => context.Sex.ToList());
            testPatientDetails = new Faker<PatientDetails>()
                .RuleFor(p => p.FamilyName, f => f.Name.LastName())
                .RuleFor(p => p.GivenName, f => f.Name.FirstName())
                .RuleFor(p => p.NhsNumber, f => f.Random.ReplaceNumbers("9#########"))
                .RuleFor(p => p.NhsNumberNotKnown, f => false)
                .RuleFor(p => p.Dob, f => f.Date.Between(StartOf1920, EndOf2014))
                .RuleFor(p => p.CountryId, f => countries.Single(c => c.IsoCode == Countries.UkCode).CountryId)
                .RuleFor(p => p.YearOfUkEntry, f => null)
                // It would be good to use real addresses, but a bit of extra work (there are too many postcodes to choose at random).
                .RuleFor(p => p.NoFixedAbode, f => true)
                .RuleFor(p => p.Address, f => null)
                .RuleFor(p => p.Postcode, f => null)
                .RuleFor(p => p.PostcodeToLookup, f => null)
                .RuleFor(p => p.EthnicityId, f => f.PickRandom(ethnicities).EthnicityId)
                .RuleFor(p => p.SexId, f => f.PickRandom(sexes).SexId);
        }

        public PatientDetails GeneratePatientDetails()
        {
            if (testPatientDetails == null)
            {
                throw new InvalidOperationException("This class must be initialised before patient details can be generated.");
            }
            return testPatientDetails.Generate();
        }
    }
}
