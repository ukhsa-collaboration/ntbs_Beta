using System.Data.SqlClient;
using System.Threading.Tasks;
using BoDi;
using Dapper;
using ntbs_service;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using TechTalk.SpecFlow;

namespace ntbs_ui_tests.Hooks
{
    [Binding]
    public class DriverSetup
    {
        private readonly IObjectContainer objectContainer;
        public readonly TestContext testContext;
        public IWebDriver Browser;
        public TestConfig settings;

        public DriverSetup(
            IObjectContainer objectContainer,
            TestConfig settings,
            TestContext testContext)
        {
            this.objectContainer = objectContainer;
            this.settings = settings;
            this.testContext = testContext;
        }

        [BeforeScenario]
        public async void BeforeScenario()
        {
            var opts = new ChromeOptions();
            opts.AddArgument("--no-sandbox"); // Necessary to avoid `unknown error: DevToolsActivePort file doesn't exist` when running on docker
            if (settings.IsHeadless)
            {
                opts.AddArgument("--headless");
            }

            Browser = new RemoteWebDriver(opts);
            Browser.Manage().Timeouts().ImplicitWait = settings.ImplicitWait;
            objectContainer.RegisterInstanceAs(Browser);
            await CleanUpMigratedNotification();
        }

        [AfterScenario]
        public async Task AfterScenario()
        {
            using (var connection = new SqlConnection(settings.EnvironmentConfig.ConnectionString))
            {
                connection.Open();
                var deleteAlerts = "DELETE FROM Alert WHERE NotificationId IN @ids";
                var deleteNotifications = "DELETE FROM Notification WHERE NotificationId IN @ids";
                await connection.ExecuteAsync(deleteAlerts, new { ids = testContext.AddedNotificationIds.ToArray() });
                await connection.ExecuteAsync(deleteNotifications, new { ids = testContext.AddedNotificationIds.ToArray() });
            }
            Browser.Quit();
            await CleanUpMigratedNotification();
        }

        private async Task CleanUpMigratedNotification()
        {
            using (var connection = new SqlConnection(settings.EnvironmentConfig.ConnectionString))
            {
                connection.Open();
                var deleteNotification = "DELETE FROM Notification WHERE ETSID = '189045'";
                await connection.ExecuteAsync(deleteNotification);
                connection.Close();
            }
            using (var connection = new SqlConnection(settings.EnvironmentConfig.MigrationConnectionString))
            {
                var importedNotificationTableName = GetImportedNotificationTableName();
                connection.Open();
                var deleteImportedNotification = $"DELETE FROM {importedNotificationTableName} WHERE LegacyId = '189045'";
                await connection.ExecuteAsync(deleteImportedNotification);
            }
        }

        private string GetImportedNotificationTableName()
        {
            var importedTablePrefix = settings.EnvironmentUnderTest == "local"
                ? "Dev"
                : char.ToUpper(settings.EnvironmentUnderTest[0]) + settings.EnvironmentUnderTest.Substring(1);
            return $"{importedTablePrefix}ImportedNotifications";
        }
    }
}
