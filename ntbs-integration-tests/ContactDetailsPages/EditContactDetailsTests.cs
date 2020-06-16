using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.ContactDetailsPages
{
    public class EditContactDetailsTests : TestRunnerBase
    {
        public EditContactDetailsTests(NtbsWebApplicationFactory<Startup> factory) 
            : base(factory) { }
        
        private const string PageRoute = "/ContactDetails/Edit";

        [Fact]
        public async Task EditDetails_ValidFields_Success()
        {
            // Arrange
            var initialDocument = await GetDocumentForUrlAsync(PageRoute);

            var formData = new Dictionary<string, string>
            {
                ["ContactDetails.JobTitle"] = "Teacher", 
                ["ContactDetails.PhoneNumberPrimary"] = "0888192311", 
                ["ContactDetails.PhoneNumberSecondary"] = "0123871623", 
                ["ContactDetails.EmailPrimary"] = "primary@email" ,
                ["ContactDetails.EmailSecondary"] = "secondary@email", 
                ["ContactDetails.Notes"] = "Notes" 
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, PageRoute);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        }
        
        [Fact]
        public async Task EditDetails_InvalidFields_DisplayErrors()
        {
            // Arrange
            var initialDocument = await GetDocumentForUrlAsync(PageRoute);

            var formData = new Dictionary<string, string>
            {
                ["ContactDetails.JobTitle"] = "¬Teacher", 
                ["ContactDetails.PhoneNumberPrimary"] = "¬0888192311", 
                ["ContactDetails.PhoneNumberSecondary"] = "¬0123871623", 
                ["ContactDetails.EmailPrimary"] = "¬primary@email" ,
                ["ContactDetails.EmailSecondary"] = "¬secondary@email", 
                ["ContactDetails.Notes"] = "¬Notes" 
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, PageRoute);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorMessage("job-title",
                String.Format(ValidationMessages.InvalidCharacter, "Job Title"));
            resultDocument.AssertErrorMessage("phone-primary",
                String.Format(ValidationMessages.InvalidCharacter, "Phone number #1"));
            resultDocument.AssertErrorMessage("phone-secondary",
                String.Format(ValidationMessages.InvalidCharacter, "Phone number #2"));
            resultDocument.AssertErrorMessage("email-primary",
                String.Format(ValidationMessages.InvalidCharacter, "Email #1"));
            resultDocument.AssertErrorMessage("email-secondary",
                String.Format(ValidationMessages.InvalidCharacter, "Email #2"));            
            resultDocument.AssertErrorMessage("notes",
                String.Format(ValidationMessages.InvalidCharacter, "Notes"));
        }
    }
}
