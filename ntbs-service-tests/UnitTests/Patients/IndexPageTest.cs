using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Data;
using ntbs_service.Models;
using ntbs_service.Pages_Patients;
using Xunit;

namespace ntbs_service_tests
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
            Assert.Equal("Bob", result[0].Forename);
        }

        public IList<Patient> GetSamplePatients()
        {
            var sampleSex = new Sex(){ SexId = 1, Label = "M" };
            var sampleRegion = new Region(){ RegionId = 1, Label = "London" };

            var samplePatient = new Patient{ PatientId = 1, Forename = "Bob", Sex = sampleSex, Region = sampleRegion };

            return new List<Patient> { samplePatient };
        }
    }
}
