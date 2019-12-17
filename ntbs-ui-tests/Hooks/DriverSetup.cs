using BoDi;
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
        public IWebDriver Browser;
        public TestSettings settings;

        public DriverSetup(IObjectContainer objectContainer, SeleniumServerFactory<Startup> server, TestSettings settings)
        {
            this.objectContainer = objectContainer;
            this.settings = settings;
            server.CreateClient(); // Not sure why needed, see hanselman link referenced in SeleniumServerFactory
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var opts = new ChromeOptions();
            opts.AddArgument("--no-sandbox"); // Necessary to avoid `unknown error: DevToolsActivePort file doesn't exist` when running on docker
            if (settings.IsHeadless)
            {
                opts.AddArgument("--headless");
            }

            Browser = new RemoteWebDriver(opts);
            objectContainer.RegisterInstanceAs(Browser);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            Browser.Quit();
        }
    }
}