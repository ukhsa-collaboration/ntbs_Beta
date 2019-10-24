using System;
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
        private readonly SeleniumServerFactory<Startup> Server;
        public IWebDriver Browser;

        public DriverSetup(IObjectContainer objectContainer, SeleniumServerFactory<Startup> server)
        {
            this.objectContainer = objectContainer;
            Server = server;
            Server.CreateClient(); // Not sure why needed, see hanselman link referenced in SeleniumServerFactory
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            var opts = new ChromeOptions();
            opts.AddArgument("--headless"); //Optional, comment this out if you want to SEE the browser window

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