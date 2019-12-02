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

        [Then(@"I should see the Notification")]
        public void ThenIShouldSeeTheNotification()
        {
            var urlArray = Browser.Url.Split('/');
            var numberOfUrlParts = urlArray.Count();
            // Last part should be id, will throw exception if not correct format
            int.Parse(urlArray[numberOfUrlParts - 1]);
            Assert.Equal("Notifications", urlArray[numberOfUrlParts - 2]);
        }

        [Then(@"I should be on the Homepage")]
        public void ThenIShouldBeOnTheHomepage()
        {
            Assert.Equal($"{Server.RootUri}/", Browser.Url);
        }

        [Then(@"I should be on the (.*) page")]
        public void ThenIShouldBeOnPage(string pageName)
        {
            // Remove any query string parameters
            Assert.Equal(pageName, Browser.Url.Split('/').Last().Split('?').First());
        }

        [Then(@"I should see all submission error messages")]
        public void ThenIShouldSeeAllSubmissionErrorMessages()
        {
            var summaryText = Browser.FindElement(By.ClassName("nhsuk-error-summary")).Text;
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.DiseaseSiteIsRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
            Assert.Contains(ValidationMessages.FamilyNameIsRequired, summaryText);
            Assert.Contains(ValidationMessages.FieldRequired, summaryText);
        }
    }
}
