using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Pages_Patients;
using Xunit;

namespace ntbs_service_tests.UnitTests.Patients
{
    public class IndexPageTest
    {
        private Mock<IPatientRepository> mockPatientRepository;
        public IndexPageTest() 
        {
            mockPatientRepository = new Mock<IPatientRepository>();
        }

        [Fact]
        public async Task OnGetAsync_PopulatesPageModel_WithPatients()
        {
            // Arrange
            mockPatientRepository.Setup(rep => rep.GetPatientsAsync())
                                 .Returns(Task.FromResult(GetSamplePatients()));

            var pageModel = new IndexModel(mockPatientRepository.Object);

            // Act
            await pageModel.OnGetAsync();
            var result = Assert.IsAssignableFrom<List<Patient>>(pageModel.Patients);
            Assert.True(result.Count == 1);
            Assert.Equal("Bob", result[0].GivenName);
        }

        public IList<Patient> GetSamplePatients()
        {
            var sampleSex = new Sex(){ SexId = 1, Label = "M" };

            var samplePatient = new Patient{ PatientId = 1, GivenName = "Bob", Sex = sampleSex };

            return new List<Patient> { samplePatient };
        }
    }
}
