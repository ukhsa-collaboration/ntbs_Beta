using System;

namespace ntbs_ui_tests.Hooks
{
    public class TestSettings
    {
        // set this to false if you want to see the browser window (locally only)
        public bool IsHeadless { get; set; } = true;

        // The amount of time we will wait when failing to get an element before failing the test
        // This allows JS adding an element into a page to do so before Selenium claims it is not there; it reduces
        // the probability of a false-failure due to Selenium and JS on the site racing each other.
        // If you are getting unexpected "element not found" errors, try increasing this timespan.
        public TimeSpan ImplicitWait { get; set; } = TimeSpan.FromSeconds(5);
    }
}
