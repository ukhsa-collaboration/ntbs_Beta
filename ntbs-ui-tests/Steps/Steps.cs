using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using ntbs_service;
using ntbs_service.Models.Validations;
using ntbs_ui_tests.Hooks;
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
        private readonly TestSettings Settings;

        private readonly StepsData stepsData = new StepsData();

        public Steps(IWebDriver driver, SeleniumServerFactory<Startup> server, ITestOutputHelper output, TestSettings settings)
        {
            Browser = driver;
            Server = server;
            this.output = output;
            Settings = settings;
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
            FindById(elementId).Click();
            FindById(elementId).SendKeys(value + "\t");
            if (!Settings.IsHeadless)
            {
                Thread.Sleep(1000);
            }
        }

        [When(@"I wait")]
        public void WhenIWait()
        {
            Thread.Sleep(1000);
        }

        private IWebElement FindById(string elementId)
        {
            return Browser.FindElement(By.Id(elementId));
        }

        [When(@"I check '(.*)'")]
        [When(@"I select radio value '(.*)'")]
        public void WhenISelectRadioOrCheckbox(string elementId)
        {
            FindById(elementId).Click();
        }

        [When(@"I click on the '(.*)' button")]
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
                .FindElement(By.Id("manage-notification"))
                .FindElement(By.TagName("summary"));
            button.Click();
        }

        [Then(@"I should see the Notification")]
        public void ThenIShouldSeeTheNotification()
        {
            var urlRegex = new Regex(@".*/Notifications/(\d+)/?$");
            var match = urlRegex.Match(Browser.Url);
            var idString = match.Groups[1].Value;
            Assert.True(match.Success, $"Url I am on instead: {Browser.Url}");
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
            // Wait for everything to load
            if (!Settings.IsHeadless)
            {
                Thread.Sleep(2000);
            }
        }

        [Then(@"I should see all submission error messages")]
        public void ThenIShouldSeeAllSubmissionErrorMessages()
        {
            var summaryText = Browser.FindElement(By.ClassName("nhsuk-error-summary")).Text;
            Assert.Contains("Notification date is a mandatory field", summaryText);
            Assert.Contains("Hospital is a mandatory field", summaryText);
            Assert.Contains(ValidationMessages.DiseaseSiteIsRequired, summaryText);
            Assert.Contains("Diagnosis date is a mandatory field", summaryText);
            Assert.Contains("Date of birth is a mandatory field", summaryText);
            Assert.Contains("Sex is a mandatory field", summaryText);
            Assert.Contains("Postcode is a mandatory field", summaryText);
            Assert.Contains("Birth country is a mandatory field", summaryText);
            Assert.Contains("Given name is a mandatory field", summaryText);
            Assert.Contains("NHS number is a mandatory field", summaryText);
            Assert.Contains("Family name is a mandatory field", summaryText);
            Assert.Contains("Ethnic group is a mandatory field", summaryText);
        }

        [Then(@"The notification should be denotified")]
        public void ThenCurrentNotificationShouldBeDenotified()
        {
            var notificationHeading = Browser.FindElement(By.TagName("h1")).Text;
            Assert.Contains("Denotified", notificationHeading);
        }
    }
}
