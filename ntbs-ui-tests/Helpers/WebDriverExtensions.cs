using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ntbs_ui_tests.Helpers
{
    internal static class WebDriverExtensions
    {
        public static void WaitUntilElementIsClickable(this IWebDriver webDriver, By by, TimeSpan timeout)
        {
            var wait = new WebDriverWait(webDriver, timeout);
            wait.Until(ExpectedConditions.ElementToBeClickable(by));
        }
    }
}
