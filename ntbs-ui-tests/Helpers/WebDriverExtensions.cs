using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ntbs_ui_tests.Helpers
{
    internal static class WebDriverExtensions
    {
        public static void WaitUntilElementIsClickableWithRetry(this IWebDriver webDriver, By by, TimeSpan timeout)
        {
            try
            {
                webDriver.WaitUntilElementIsClickable(by, timeout);
            }
            catch (StaleElementReferenceException)
            {
                // In scenarios where the dropdown we're selecting from has just been reloaded (e.g. because of the result
                // of an API call) the options can become stale between finding the dropdown and selecting a value. In this
                // case we can select the value by just trying again.
                webDriver.WaitUntilElementIsClickable(by, timeout);
            }
        }

        public static void WaitUntilElementIsClickable(this IWebDriver webDriver, By by, TimeSpan timeout)
        {
            var wait = new WebDriverWait(webDriver, timeout);
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }
    }
}
