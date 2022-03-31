using System;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_ui_tests.Helpers;
using ntbs_ui_tests.Hooks;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.UnitTestProvider;

namespace ntbs_ui_tests.Steps
{
    [Binding]
    public class ArrangeSteps
    {
        private readonly IWebDriver Browser;
        private readonly TestConfig Settings;
        private readonly TestContext TestContext;
        private readonly IUnitTestRuntimeProvider RuntimeProvider;

        public ArrangeSteps(IWebDriver driver, TestConfig settings, TestContext testContext, IUnitTestRuntimeProvider unitTestRuntimeProvider)
        {
            Browser = driver;
            Settings = settings;
            TestContext = testContext;
            RuntimeProvider = unitTestRuntimeProvider;
        }

        #region Navigation

        [Given(@"I navigate to the app")]
        public void GivenINavigateToApp()
        {
            WithErrorLogging(() =>
            {
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUriString}");
            });
        }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnPage(string pageName)
        {
            WithErrorLogging(() =>
            {
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUriString}/{pageName}");
            });
        }

        [Given(@"I am on the Homepage")]
        public void GivenIAmOnTheHomepage()
        {
            WithErrorLogging(() =>
            {
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUriString}");
            });
        }

        [Given(@"I am on seeded '(.*)' notification overview page")]
        public void GivenIAmOnANotificationPage(string notificationName)
        {
            WithErrorLogging(() =>
            {
                var notification = Utilities.GetNotificationForUser(notificationName, TestContext.LoggedInUser);
                SaveNotificationInDatabase(notification);

                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUriString}/Notifications/{notification.NotificationId}");

                if (!Settings.IsHeadless)
                {
                    Thread.Sleep(4000);
                }
            });
        }

        #endregion

        #region Login

        [Given(@"I have logged in as (.*)")]
        public void GivenIHaveLoggedIn(string userId)
        {
            WithErrorLogging(() =>
            {
                if (Settings.UseCookieOverride)
                {
                    var user = Settings.Users["ManuallyLoggedInUser"];
                    user.UserId = GetUserIdFromUsername(user.Username);
                    SimulateLoginByAddingCookiesToBrowser();
                    TestContext.LoggedInUser = user;
                }
                else
                {
                    var user = Settings.Users[userId];
                    user.UserId = GetUserIdFromUsername(user.Username);
                    Login(user);
                    TestContext.LoggedInUser = user;
                }
            });
        }

        [Given(@"I choose to log in with a different account")]
        public void GivenIChooseToLogInWithDifferentAccount()
        {
            WithErrorLogging(() =>
            {
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUriString}");
                HtmlElementHelper.FindElementById(Browser, "otherTile").Click();
            });
        }

        private void Login(UserConfig user)
        {
            const string nextButtonSelector = "input[type=submit][value=Next]";
            Browser.WaitUntilElementIsClickable(By.CssSelector(nextButtonSelector), Settings.ImplicitWait);
            Browser.FindElement(By.CssSelector("input[type=email]")).SendKeys(user.Username);
            Browser.FindElement(By.CssSelector(nextButtonSelector)).Click();
            const string signInButtonSelector = "input[type=submit][value='Sign in']";
            Browser.WaitUntilElementIsClickable(By.CssSelector(signInButtonSelector), Settings.ImplicitWait);
            Browser.FindElement(By.CssSelector("input[type=password]")).SendKeys(user.Password);
            Browser.FindElement(By.CssSelector(signInButtonSelector)).Click();
        }

        private void SimulateLoginByAddingCookiesToBrowser()
        {
            // First, go to NTBS so that cookies are added to the right domain
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUriString}/PostLogout");
            Browser.FindElement(By.Id("maincontent")); // Ensure page has loaded
            // Then, add the cookies provided in the config file
            var cookieContainer = new CookieContainer();
            cookieContainer.SetCookies(Settings.EnvironmentConfig.RootUri, Settings.AuthenticatedCookieHeader.Replace(';', ','));
            foreach (System.Net.Cookie cookie in cookieContainer.GetCookies(Settings.EnvironmentConfig.RootUri))
            {
                Browser.Manage().Cookies.AddCookie(new OpenQA.Selenium.Cookie(cookie.Name, cookie.Value));
            }
        }

        #endregion

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

        private void WithErrorLogging(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                // This is temporary debugging to help determine why certain tests sometimes fail
                var webElement = (string)((IJavaScriptExecutor)Browser).ExecuteScript("return document.body.innerHTML;");
                Console.WriteLine(webElement);
                throw;
            }
        }
    }
}
