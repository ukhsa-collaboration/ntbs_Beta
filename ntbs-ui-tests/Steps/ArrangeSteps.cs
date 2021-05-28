using System;
using System.Linq;
using System.Threading;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_ui_tests.Helpers;
using ntbs_ui_tests.Hooks;
using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace ntbs_ui_tests.Steps
{
    [Binding]
    public class ArrangeSteps
    {
        private readonly IWebDriver Browser;
        private readonly TestConfig Settings;
        private readonly TestContext TestContext;

        public ArrangeSteps(IWebDriver driver, TestConfig settings, TestContext testContext)
        {
            Browser = driver;
            Settings = settings;
            TestContext = testContext;
        }

        #region Navigation

        [Given(@"I navigate to the app")]
        public void GivenINavigateToApp()
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}");
        }

        [Given(@"I am on the (.*) page")]
        public void GivenIAmOnPage(string pageName)
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/{pageName}");
        }

        [Given(@"I am on the Homepage")]
        public void GivenIAmOnTheHomepage()
        {
            Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}");
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

        #region Login

        [Given(@"I have logged in as (.*)")]
        public void GivenIHaveLoggedIn(string userId)
        {
            var user = Settings.Users[userId];
            user.UserId = GetUserIdFromUsername(user.Username);
            Browser.FindElement(By.CssSelector("input[type=email]")).SendKeys(user.Username);
            Browser.FindElement(By.CssSelector("input[type=submit][value=Next]")).Click();
            Browser.FindElement(By.CssSelector("input[type=password]")).SendKeys(user.Password);
            Browser.FindElement(By.CssSelector("input[type=submit][value='Sign in']")).Click();
            TestContext.LoggedInUser = user;
        }

        [Given(@"I choose to log in with a different account")]
        public void GivenIChooseToLogInWithDifferentAccount()
        {
            HtmlElementHelper.FindElementById(Browser, "otherTile").Click();
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

        #endregion

        private int GetUserIdFromUsername(string username)
        {
            var options = new DbContextOptionsBuilder<NtbsContext>();
            options.UseSqlServer(Settings.EnvironmentConfig.ConnectionString);
            using (var context = new NtbsContext(options.Options))
            {
                return context.User.Single(u => u.Username.ToLower() == username.ToLower()).Id;
            }
        }
    }
}
