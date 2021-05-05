using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;
using ntbs_ui_tests.Helpers;
using ntbs_ui_tests.Hooks;
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
        private readonly TestConfig Settings;
        private readonly TestContext TestContext;

        public Steps(IWebDriver driver, TestConfig settings, TestContext testContext)
        {
            Browser = driver;
            Settings = settings;
            TestContext = testContext;
        }

        [Given(@"I have logged in as (.*)")]
        public void GivenIHaveLoggedIn(string userId)
        {
            try
            {
                var user = Settings.Users[userId];
                user.UserId = GetUserIdFromUsername(user.Username);
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}");
                Browser.FindElement(By.CssSelector("input[type=email]")).SendKeys(user.Username);
                Browser.FindElement(By.CssSelector("input[type=submit][value=Next]")).Click();
                Browser.FindElement(By.CssSelector("input[type=password]")).SendKeys(user.Password);
                Browser.FindElement(By.CssSelector("input[type=submit][value='Sign in']")).Click();
                Browser.FindElement(By.CssSelector("input[type=submit][value=Yes]"));
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
        
        [When(@"I make selection (.*) from (.*) section for '(.*)'")]
        public void WhenISelectValueFromGroupForFieldWithId(string value, string group, string elementId)
        {
            var selection = Browser.FindElement(By.XPath($"//select[@id='{elementId}']/optgroup[@label='{group}']/option[@value='{value}']"));
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
        public void WhenISelectTextFromDropdown(string text, string selectId)
        {
            new SelectElement(FindById(selectId)).SelectByText(text);
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
        public void WhenIGoToEditTheSection(string overviewSectionId)
        {
            var link = Browser.FindElement(By.Id(overviewSectionId))
                .FindElement(By.LinkText("Edit"));
            link.Click();
        }

        [When(@"I select (.*) from input list '(.*)'")]
        public void WhenISelectFromInputList(string value, string inputListId)
        {
            FindById(inputListId).Click();
            FindById(inputListId).SendKeys(value);
            FindById(inputListId+"__option--0").Click();
            FindById(inputListId).SendKeys("\t");
        }

        [Then(@"I should see the Notification")]
        public void ThenIShouldSeeTheNotification()
        {
            var urlRegex = new Regex(@".*/Notifications/(\d+)/?(#.+)?$");
            var match = urlRegex.Match(Browser.Url);
            Assert.True(match.Success, $"Url I am on instead: {Browser.Url}");
        }

        [Then(@"I can see the starting event '(.*)` dated `(.*)'")]
        public void ThenIShouldSeeTheNotification(string eventType, string dateString)
        {
            var episodesOverview = Browser
                .FindElement(By.Id("overview-episodes"))
                .FindElement(By.XPath(".."));
            Assert.Contains(eventType, episodesOverview.Text);
            Assert.Contains(dateString, episodesOverview.Text);
        }

        [Then(@"The value '(.*)' for the field '(.*)' in section '(.*)' is in the database")]
        public void ThenDatabaseValueShouldEqual(string value, string field, string section)
        {
            CheckDatabaseValues(value, field, section);
        }

        [Then(@"The date '(.*)' for the field '(.*)' in section '(.*)' is in the database")]
        public void ThenDatabaseDateValueShouldEqual(string date, string field, string section)
        {
            CheckDatabaseValues(DateTime.Parse(date, new CultureInfo("en-GB")), field, section);
        }

        [Then(@"The status '(.*)' for the field '(.*)' in section '(.*)' is in the database")]
        public void ThenDatabaseStatusValueShouldEqual(Status status, string field, string section)
        {
            CheckDatabaseValues(status, field, section);
        }
        
        [Then(@"The value for the field '(.*)' in section '(.*)' in the database is (.*)")]
        public void ThenDatabaseBoolValueShouldEqual(string field, string section, bool value)
        {
            CheckDatabaseValues(value, field, section);
        }
        
        [Then(@"The number '(.*)' for the field '(.*)' in section '(.*)' is in the database")]
        public void ThenDatabaseIntValueShouldEqual(int value, string field, string section)
        {
            CheckDatabaseValues(value, field, section);
        }

        [Then(@"I should be on the Homepage")]
        public void ThenIShouldBeOnTheHomepage()
        {
            Assert.Equal($"{Settings.EnvironmentConfig.RootUri}/", Browser.Url);
        }

        [Then("A new notification should have been created")]
        public void ThenNotificationCreated()
        {
            var urlRegex = new Regex(@".*/Notifications/(\d+)/.*$");
            var match = urlRegex.Match(Browser.Url);
            var idString = match.Groups[1].Value;
            Assert.True(match.Success, $"Url I am on instead: {Browser.Url}");
            TestContext.AddedNotificationIds.Add(int.Parse(idString));
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

        private void CheckDatabaseValues(object value, string field, string section)
        {
            var currentNotificationId = GetNotificationIdFromUrl();
            var notificationProperty = MapSectionToNotificationProperty(section);
            var sectionProperty = MapFieldToSectionProperty(field);
            var options = new DbContextOptionsBuilder<NtbsContext>();
            options.UseSqlServer(Settings.EnvironmentConfig.ConnectionString);
            using var context = new NtbsContext(options.Options);
            var currentNotification = context.Notification.Single(n => n.NotificationId == currentNotificationId);
            if (!string.IsNullOrEmpty(section))
            {
                var currentNotificationProperty = currentNotification.GetType().GetProperty(notificationProperty)
                    .GetValue(currentNotification, null);
                Assert.Equal(
                    currentNotificationProperty.GetType().GetProperty(sectionProperty).GetValue(currentNotificationProperty, null),
                    value);
            }
            else
            {
                Assert.Equal(
                    currentNotification.GetType().GetProperty(sectionProperty).GetValue(currentNotification, null),
                    value);
            }
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

        private int GetNotificationIdFromUrl()
        {
            var splitUrl = Browser.Url.Split("/").ToList();
            return int.Parse(splitUrl[splitUrl.IndexOf("Notifications") + 1].Split("#")[0]);
        }

        private string MapSectionToNotificationProperty(string section)
        {
            return section switch
            {
                "Patient details" => "PatientDetails",
                "Hospital details" => "HospitalDetails",
                "Clinical details" => "ClinicalDetails",
                "Test results" => "TestData",
                "Contact tracing" => "ContactTracing",
                "Social risk factors" => "SocialRiskFactors",
                "Visitor details" => "VisitorDetails",
                "Comorbidities" => "ComorbidityDetails",
                "Immunosuppression" => "ImmunosuppressionDetails",
                "Social context addresses" => "SocialContextAddress",
                "Social context venues" => "SocialContextVenue",
                "Previous history" => "PreviousTbHistory",
                _ => null
            };
        }

        private string MapFieldToSectionProperty(string field)
        {
            return field switch
            {
                "Given name" => "GivenName",
                "Occupation other" => "OccupationOther",
                "Notification date" => "NotificationDate",
                "BCG vaccination" => "BCGVaccinationState",
                "Home visit" => "HomeVisitCarriedOut",
                "Home visit date" => "FirstHomeVisitDate",
                "Test carried out" => "HasTestCarriedOut",
                "Adults identified" => "AdultsIdentified",
                "Adults screened" => "AdultsScreened",
                "Children latent TB" => "ChildrenLatentTB",
                "Children identified" => "ChildrenIdentified",
                "Alcohol misuse" => "AlcoholMisuseStatus",
                "Homelessness" => "RiskFactorHomelessness.Status",
                "Asylum seeker" => "AsylumSeekerStatus",
                "Has visitor" => "HasVisitor",
                "Total number of countries" => "TotalNumberOfCountries",
                "Stay length for country 1" => "StayLengthInMonths1",
                "Immunosuppression status" => "Status",
                "Immunosuppression other" => "OtherDescription",
                _ => field
            };
        }
    }
}
