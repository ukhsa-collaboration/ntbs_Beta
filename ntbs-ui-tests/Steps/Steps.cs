using System.Linq;
using System.Threading;
using ntbs_service;
using ntbs_service.Models.Validations;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Xunit;

namespace ntbs_ui_tests.StepDefinitions
{
    [Binding]
    public class Steps
    {
        private readonly IWebDriver Browser;
        private readonly SeleniumServerFactory<Startup> Server;

        public Steps(IWebDriver driver, SeleniumServerFactory<Startup> server)
        {
            Browser = driver;
            Server = server;
        }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnPage(string pageName)
        {
            Browser.Navigate().GoToUrl($"{Server.RootUri}/{pageName}");
        }

        [When(@"I enter (.*) into '(.*)'")]
        public void WhenIEnterValueIntoFieldWithId(string value, string elementId)
        {
            FindById(elementId).SendKeys(value);
        }

        private IWebElement FindById(string elementId)
        {
            return Browser.FindElement(By.Id(elementId));
        }

        [When(@"I check '(.*)'")]
        [When(@"I select radio value '(.*)'")]
        [When(@"I click on '(.*)'")]
        public void WhenIClickOn(string elementId)
        {
            FindById(elementId).Click();
        }

        [When(@"I select (.*) for '(.*)'")]
        public void WhenISelectValueFromDropdown(string value, string selectId)
        {
            new SelectElement(FindById(selectId)).SelectByValue(value);
        }

        [When(@"I wait for 1 second")]
        public void WhenIWaitBriefly()
        {
            Thread.Sleep(1000);
        }

        [Then(@"I should be on the (.*) page")]
        public void ThenIShouldBeOnPage(string pageName)
        {
            Assert.Equal(pageName, Browser.Url.Split('/').Last().Split('?').First());
        }

        [Then(@"I should see all submission error messages")]
        public void ThenIShouldSeeAllSubmissionErrorMessages()
        {
            var summaryText = Browser.FindElement(By.ClassName("nhsuk-error-summary")).Text;
            Assert.Contains(ValidationMessages.NotificationDateIsRequired, summaryText);
            Assert.Contains(ValidationMessages.HospitalIsRequired, summaryText);
            Assert.Contains(ValidationMessages.TBServiceIsRequired, summaryText);
            Assert.Contains(ValidationMessages.DiseaseSiteIsRequired, summaryText);
            Assert.Contains(ValidationMessages.DiagnosisDateIsRequired, summaryText);
            Assert.Contains(ValidationMessages.BirthDateIsRequired, summaryText);
            Assert.Contains(ValidationMessages.SexIsRequired, summaryText);
            Assert.Contains(ValidationMessages.PostcodeIsRequired, summaryText);
            Assert.Contains(ValidationMessages.BirthCountryIsRequired, summaryText);
            Assert.Contains(ValidationMessages.GivenNameIsRequired, summaryText);
            Assert.Contains(ValidationMessages.NHSNumberIsRequired, summaryText);
            Assert.Contains(ValidationMessages.FamilyNameIsRequired, summaryText);
            Assert.Contains(ValidationMessages.EthnicGroupIsRequired, summaryText);
        }
    }
}
