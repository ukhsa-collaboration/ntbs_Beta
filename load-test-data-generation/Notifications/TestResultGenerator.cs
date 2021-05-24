using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace load_test_data_generation.Notifications
{
    internal class TestResultGenerator
    {
        private readonly IContextProvider contextProvider;

        private static readonly DateTime StartOf2015 = new DateTime(2014, 01, 01, 00, 00, 00);
        private static readonly DateTime EndOf2020 = new DateTime(2020, 12, 31, 23, 59, 59);

        private List<ManualTestType> testTypes;
        private Faker<TestData> testData;

        public TestResultGenerator(IContextProvider contextProvider)
        {
            this.contextProvider = contextProvider;
        }

        public void Initialise()
        {
            testTypes = contextProvider.WithContext(context =>
            {
                return context.ManualTestType
                    .Include(tt => tt.ManualTestTypeSampleTypes)
                    .ToList();
            });

            testData = new Faker<TestData>()
                .RuleFor(tr => tr.HasTestCarriedOut, f => f.Random.Bool(0.8f))
                .RuleFor(tr => tr.ManualTestResults, GenerateResults);
        }

        public TestData GenerateTestData()
        {
            return testData.Generate();
        }

        private List<ManualTestResult> GenerateResults(Faker faker, TestData testData)
        {
            var testResultGenerator = new Faker<ManualTestResult>()
                .RuleFor(tr => tr.TestDate, f => f.Date.Between(StartOf2015, EndOf2020))
                .RuleFor(tr => tr.ManualTestTypeId, f => f.PickRandom(testTypes).ManualTestTypeId)
                .RuleFor(tr => tr.SampleTypeId, GenerateSampleTypeId)
                .RuleFor(tr => tr.Result, GenerateResult);

            return testData.HasTestCarriedOut == true
                ? Enumerable.Range(0, faker.Random.Int(1, 3)).Select(_ => testResultGenerator.Generate()).ToList()
                : new List<ManualTestResult>();
        }

        private int? GenerateSampleTypeId(Faker faker, ManualTestResult testResult)
        {
            var possibleSampleTypes = testTypes.Single(tt => tt.ManualTestTypeId == testResult.ManualTestTypeId).ManualTestTypeSampleTypes;
            return possibleSampleTypes.Any()
                ? faker.PickRandom(possibleSampleTypes.Select(ttst => ttst.SampleTypeId))
                : null;
        }

        private static Result? GenerateResult(Faker faker, ManualTestResult testResult)
        {
            var possibleResults = ((Result[])Enum.GetValues(typeof(Result)))
                .Where(result => result.IsValidForTestType(testResult.ManualTestTypeId.Value));
            return faker.PickRandom(possibleResults);
        }
    }
}
