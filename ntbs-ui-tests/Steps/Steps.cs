using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ntbs_service;
using ntbs_service.Models.Validations;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace ntbs_ui_tests.StepDefinitions
{
    public class StepsData
    {
        public int NotificationId;
    }

    [Binding]
    public class Steps
    {
        private readonly IWebDriver Browser;
        private readonly SeleniumServerFactory<Startup> Server;
        private readonly ITestOutputHelper output;
        private readonly StepsData stepsData = new StepsData();

        public Steps(IWebDriver driver, SeleniumServerFactory<Startup> server, ITestOutputHelper output)
        {
            Browser = driver;
            Server = server;
            this.output = output;
        }

        [Given(@"I am on current notification overview page")]
        public void GivenIAmOnNotificationOverviewPage()
        {
            Browser.Navigate().GoToUrl($"{Server.RootUri}/Notifications/{stepsData.NotificationId}");
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

        [When(@"I expand manage notification section")]
        public void WhenIExpandNotificationSection()
        {
            var button = Browser
                .FindElement(By.ClassName("manage-notification"))
                .FindElement(By.TagName("summary"));
            button.Click();
        }

        [When(@"I wait for 1 second")]
        public void WhenIWaitBriefly()
        {
            Thread.Sleep(1000);
        }

        [Then(@"I should see the Notification")]
        public void ThenIShouldSeeTheNotification()
        {
            var urlRegex = new Regex(@".*/Notifications/(\d+)/?");
            var match = urlRegex.Match(Browser.Url);
            var idString = match.Groups[1].Value;
            Assert.True(match.Success);
            stepsData.NotificationId = int.Parse(idString);
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
            Assert.Contains(ValidationMessages.BirthDateIsRequired, summaryText);
            Assert.Contains(ValidationMessages.SexIsRequired, summaryText);
            Assert.Contains(ValidationMessages.PostcodeIsRequired, summaryText);
            Assert.Contains(ValidationMessages.BirthCountryIsRequired, summaryText);
            Assert.Contains(ValidationMessages.GivenNameIsRequired, summaryText);
            Assert.Contains(ValidationMessages.NHSNumberIsRequired, summaryText);
            Assert.Contains(ValidationMessages.FamilyNameIsRequired, summaryText);
            Assert.Contains(ValidationMessages.EthnicGroupIsRequired, summaryText);
            Assert.Contains(ValidationMessages.NotificationDateIsRequired, summaryText);
            Assert.Contains(ValidationMessages.HospitalIsRequired, summaryText);
            Assert.Contains(ValidationMessages.DiseaseSiteIsRequired, summaryText);
            Assert.Contains(ValidationMessages.DiagnosisDateIsRequired, summaryText);
            Assert.Contains(string.Format(ValidationMessages.Mandatory, "Test carried out"), summaryText);
        }

        [Then(@"The notification should be denotified")]
        public void ThenCurrentNotificationShouldBeDenotified()
        {
            var notificationHeading = Browser.FindElement(By.TagName("h1")).Text;
            Assert.Contains("Denotified", notificationHeading);
        }
    }
}
