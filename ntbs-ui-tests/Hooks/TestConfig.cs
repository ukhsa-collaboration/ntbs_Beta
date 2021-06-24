using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ntbs_ui_tests.Helpers;

namespace ntbs_ui_tests.Hooks
{
    public class TestConfig
    {
        public TestConfig()
        {
            ConfigHelper.GetConfigurationRoot().Bind(this);
        }

        // set this to false if you want to see the browser window (locally only)
        public bool IsHeadless => true;

        // The amount of time we will wait when failing to get an element before failing the test
        // This allows JS adding an element into a page to do so before Selenium claims it is not there; it reduces
        // the probability of a false-failure due to Selenium and JS on the site racing each other.
        // If you are getting unexpected "element not found" errors, try increasing this timespan.
        public TimeSpan ImplicitWait => TimeSpan.FromSeconds(30);

        public IDictionary<string, UserConfig> Users { get; set; }

        public IDictionary<string, EnvironmentConfig> Environments { get; set; }

        public string EnvironmentUnderTest { get; set; }

        public EnvironmentConfig EnvironmentConfig => Environments[EnvironmentUnderTest];
    }

    public class EnvironmentConfig
    {
        public string ConnectionString { get; set; }
        public string MigrationConnectionString { get; set; }
        public string SpecimenConnectionString { get; set; }
        public string RootUri { get; set; }
        public string ImportedNotificationNtbsEnvironment { get; set; }
    }

    public class UserConfig
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string TbServiceCode { get; set; }
    }
}
