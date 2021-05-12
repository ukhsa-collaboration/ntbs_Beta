using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using ntbs_ui_tests.Helpers;
using ntbs_ui_tests.Hooks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Xunit;

namespace ntbs_ui_tests.Steps
{
    [Binding]
    public class Steps
    {
        private readonly IWebDriver Browser;
        private readonly TestConfig Settings;
        private readonly TestContext TestContext;

        public Steps(IWebDriver driver, TestConfig settings, TestContext testContext)
        {
            Browser = driver;
            Settings = settings;
            TestContext = testContext;
        }

        #region Arrange

        [Given(@"I navigate to the app")]
        public void GivenINavigateToApp()
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}");
        }
        
        [Given(@"I have logged in as (.*)")]
        public void GivenIHaveLoggedIn(string userId)
        {
            try
            {
                var user = Settings.Users[userId];
                user.UserId = GetUserIdFromUsername(user.Username);
                Browser.FindElement(By.CssSelector("input[type=email]")).SendKeys(user.Username);
                Browser.FindElement(By.CssSelector("input[type=submit][value=Next]")).Click();
                Browser.FindElement(By.CssSelector("input[type=password]")).SendKeys(user.Password);
                Browser.FindElement(By.CssSelector("input[type=submit][value='Sign in']")).Click();
                TestContext.LoggedInUser = user;
            }
            catch
            {
                // TODO:Remove this code
                // This is temporary debugging to help determine why the login is sometimes failing
                var webElement = (string)((IJavaScriptExecutor)Browser).ExecuteScript("return document.body.innerHTML;");
                Console.WriteLine(webElement);
                throw;
            }
        }

        [Given(@"I choose to log in with a different account")]
        public void GivenIChooseToLogInWithDifferentAccount()
        {
            FindElementById("otherTile").Click();
        }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnPage(string pageName)
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/{pageName}");
        }

        [Given(@"I am on seeded '(.*)' notification overview page")]
        public void GivenIAmOnANotificationPage(string notificationName)
        {
            var notification = Utilities.GetNotificationForUser(notificationName, TestContext.LoggedInUser);
            SaveNotificationInDatabase(notification);

            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/Notifications/{notification.NotificationId}");

            if (!Settings.IsHeadless)
            {
                Thread.Sleep(4000);
            }
        }

        #endregion

        #region Act

        [When(@"I navigate to the url of the current notification")]
        public void WhenINavigateToCurrentNotificationUrl()
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/Notifications/{TestContext.AddedNotificationIds.Single()}");
        }
        
        [When(@"I click '(.*)' on the navigation bar")]
        public void ClickOnTheNavigationBar(string label)
        {
            var pageLink = FindElementByXpath($"//*[@id='navigation-side-menu']/li[contains(.,'{label}')]/a");
            pageLink.Click();
        }

        [When(@"I click on the (.*) link")]
        public void WhenIClickOnTheLink(string linkLabel)
        {
            FindElementByXpath($"//a[contains(.,'{linkLabel}')]").Click();
        }

        [When(@"I enter (.*) into '(.*)'")]
        public void WhenIEnterValueIntoFieldWithId(string value, string elementId)
        {
            var element = FindElementById(elementId);
            element.Click();
            element.SendKeys(Keys.Control + "a");
            element.SendKeys(Keys.Delete);
            element.SendKeys(value + "\t");
            if (!Settings.IsHeadless)
            {
                Thread.Sleep(1000);
            }
        }
        
        [When(@"I make selection (.*) from (.*) section for '(.*)'")]
        public void WhenISelectValueFromGroupForFieldWithId(string value, string group, string elementId)
        {
            var selection = FindElementByXpath($"//select[@id='{elementId}']/optgroup[@label='{group}']/option[@value='{value}']");
            selection.Click();
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

        [When(@"I uncheck '(.*)'")]
        public void WhenIDeselectCheckbox(string elementId)
        {
            var element = FindElementById(elementId);
            if (element.Selected)
            {
                element.Click();
            }
        }

        [When(@"I check '(.*)'")]
        [When(@"I select radio value '(.*)'")]
        public void WhenISelectRadioOrCheckbox(string elementId)
        {
            var element = FindElementById(elementId);
            if (!element.Selected)
            {
                element.Click();
            }
        }

        [When(@"I click on the '(.*)' button")]
        public void WhenIClickOn(string elementId)
        {
            FindElementById(elementId).Click();
        }

        [When(@"I select (.*) for '(.*)'")]
        public void WhenISelectTextFromDropdown(string text, string selectId)
        {
            new SelectElement(FindElementById(selectId)).SelectByText(text);
        }

        [When(@"I expand manage notification section")]
        public void WhenIExpandNotificationSection()
        {
            var button = Browser
                .FindElement(By.Id("manage-notification"))
                .FindElement(By.TagName("summary"));
            button.Click();
        }

        [When(@"I go to edit the '(.*)' section")]
        public void WhenIGoToEditTheSection(string section)
        {
            var sectionId = GetSectionIdFromSection(section);
            var link = Browser.FindElement(By.Id($"{sectionId}-title"))
                .FindElement(By.LinkText("Edit"));
            link.Click();
        }

        [When(@"I expand the '(.*)' section")]
        public void WhenIExpandSection(string sectionId)
        {
            var element = FindElementById(sectionId).FindElement(By.TagName("summary"));
            element.Click();
        }

        [When(@"I select (.*) from input list '(.*)'")]
        public void WhenISelectFromInputList(string value, string inputListId)
        {
            var inputList = FindElementById(inputListId);
            inputList.Click();
            inputList.SendKeys(Keys.Control + "a");
            inputList.SendKeys(Keys.Delete);
            inputList.SendKeys(value);
            FindElementById(inputListId+"__option--0").Click();
            inputList.SendKeys("\t");
        }

        [When(@"I take action on the alert with title (.*)")]
        public void WhenITakeActionOnAlert(string title)
        {
            var alert = FindElementsByXpath("//*[contains(@id, 'alert-')]").Single(a => a.Text.Contains(title));
            alert.FindElement(By.LinkText("Take action")).Click();
        }

        [When(@"I log out")]
        public void WhenILogOut()
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/Logout");
        }

        #endregion

        #region Assert

        [Then(@"I can see the value (.*) for element with id '(.*)'")]
        public void ThenICanSeeValueForId(string value, string elementId)
        {
            Assert.Contains(value, FindElementById(elementId).Text);
        }

        [Then(@"I should see the Notification")]
        public void ThenIShouldSeeTheNotification()
        {
            var urlRegex = new Regex(@".*/Notifications/(\d+)/?(#.+)?$");
            var match = urlRegex.Match(Browser.Url);
            Assert.True(match.Success, $"Url I am on instead: {Browser.Url}");
        }

        [Then(@"I should be on the Homepage")]
        public void ThenIShouldBeOnTheHomepage()
        {
            Assert.Equal($"{Settings.EnvironmentConfig.RootUri}/", Browser.Url);
        }

        [Then("A new notification should have been created")]
        public void ThenNotificationCreated()
        {
            var notificationId = GetNotificationIdAndAssertMatchFromUrl();
            Assert.DoesNotContain(notificationId, TestContext.AddedNotificationIds);
            TestContext.AddedNotificationIds.Add(notificationId);
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

        [Then(@"I can see the value '(.*)' for the field '(.*)' in the '(.*)' overview section")]
        public void ThenICanSeeValueForFieldInTheOverviewSection(string value, string field, string section)
        {
            var sectionId = GetSectionIdFromSection(section);
            var htmlId = $"{sectionId}-{field}";
            Assert.Contains(value, FindElementById(htmlId).Text);
        }

        [Then(@"I can see the value '(.*)' in the '(.*)' table overview section")]
        public void ThenICanSeeValueInTheOverviewSection(string value, string section)
        {
            var sectionId = GetSectionIdFromSection(section);
            Assert.Contains(value, FindElementById(sectionId).Text);
        }

        [Then(@"I can see the error '(.*)'")]
        public void ThenICanSeeTheError(string errorMessage)
        {
            var errorSection = FindElementByXpath("//*[@class='nhsuk-error-summary']");
            Assert.Contains(errorMessage, errorSection.Text);
        }

        [Then(@"I see the warning '(.*)' for '(.*)'")]
        public void ThenICanSeeTheWarningForId(string warningMessage, string warningId)
        {
            var warningElement = FindElementById(warningId);
            Assert.Contains(warningMessage, warningElement.Text);
        }

        [Then(@"The element with id '(.*)' is not present")]
        public void ThenICannotSeeTheElement(string elementId)
        {
            var elements = FindElementsById(elementId);
            Assert.False(elements.Any());
        }

        [Then(@"A (.*) alert is present on the notification")]
        public void ThenAlertIsPresent(string alertTitle)
        {
            var alerts = FindElementsByXpath("//*[contains(@id, 'alert-')]");
            Assert.NotNull(alerts.SingleOrDefault(a => a.Text.Contains(alertTitle)));
        }

        [Then(@"I can see '(.*)' as the rejection note")]
        public void ThenICanSeeRejectionNote(string rejectionNote)
        {
            var noteOnPage = Browser.FindElement(By.ClassName("rejection-note"));
            Assert.Contains(rejectionNote, noteOnPage.Text);
        }

        [Then(@"I can see the value (.*) for '(.*)' transfer information")]
        public void ThenICanSeeTransferInformationValue(string value, string title)
        {
            var transferInformationElements = Browser.FindElements(By.ClassName("transfer-request-information"));
            Assert.Contains($"{title}:\r\n{value}", transferInformationElements.Select(t => t.Text));
        }

        #endregion

        private IWebElement FindElementById(string elementId)
        {
            return Browser.FindElement(By.Id(elementId));
        }

        private ReadOnlyCollection<IWebElement> FindElementsById(string elementId)
        {
            return Browser.FindElements(By.Id(elementId));
        }

        private IWebElement FindElementByXpath(string xpath)
        {
            return Browser.FindElement(By.XPath(xpath));
        }

        private ReadOnlyCollection<IWebElement> FindElementsByXpath(string xpath)
        {
            return Browser.FindElements(By.XPath(xpath));
        }

        private void SaveNotificationInDatabase(Notification notification)
        {
            var options = new DbContextOptionsBuilder<NtbsContext>();
            options.UseSqlServer(Settings.EnvironmentConfig.ConnectionString);
            using (var context = new NtbsContext(options.Options))
            {
                context.Add(notification);
                context.SaveChanges();
            }
            TestContext.AddedNotificationIds.Add(notification.NotificationId);
        }

        private int GetUserIdFromUsername(string username)
        {
            var options = new DbContextOptionsBuilder<NtbsContext>();
            options.UseSqlServer(Settings.EnvironmentConfig.ConnectionString);
            using (var context = new NtbsContext(options.Options))
            {
                return context.User.Single(u => u.Username.ToLower() == username.ToLower()).Id;
            }
        }

        private string GetSectionIdFromSection(string section)
        {
            return OverviewSubPathToAnchorMap.GetOverviewAnchorId(
                (string)typeof(NotificationSubPaths).GetProperty($"Edit{section}").GetValue(null, null));
        }

        private int GetNotificationIdAndAssertMatchFromUrl()
        {
            var urlRegex = new Regex(@".*/Notifications/(\d+)/.*$");
            var match = urlRegex.Match(Browser.Url);
            var idString = match.Groups[1].Value;
            Assert.True(match.Success, $"Url I am on instead: {Browser.Url}");
            return int.Parse(idString);
        }
    }
}
