using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using ntbs_ui_tests.Helpers;
using ntbs_ui_tests.Hooks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TechTalk.SpecFlow;
using Xunit;
using ExpectedConditions = SeleniumExtras.WaitHelpers.ExpectedConditions;

namespace ntbs_ui_tests.Steps
{
    [Binding]
    public class ActSteps
    {
        private readonly IWebDriver Browser;
        private readonly TestConfig Settings;
        private readonly TestContext TestContext;

        public ActSteps(IWebDriver driver, TestConfig settings, TestContext testContext)
        {
            Browser = driver;
            Settings = settings;
            TestContext = testContext;
        }

        #region Navigation and action

        [When(@"I navigate to the url of the current notification")]
        public void WhenINavigateToCurrentNotificationUrl()
        {
            WithErrorLogging(() =>
            {
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/Notifications/{TestContext.AddedNotificationIds.Single()}");
                // Verify that the page has loaded
                Assert.NotNull(Browser.FindElement(By.ClassName("notification-banner-title-text")));
            });
        }

        [When(@"I click '(.*)' on the navigation bar")]
        public void ClickOnTheNavigationBar(string label)
        {
            WithErrorLogging(() =>
            {
                var pageLink = HtmlElementHelper.FindElementByXpath(Browser, $"//*[@id='navigation-side-menu']/li[contains(.,'{label}')]/a");
                pageLink.Click();
            });
        }

        [When(@"I log out")]
        public void WhenILogOut()
        {
            WithErrorLogging(() =>
            {
                Browser.Navigate().GoToUrl($"{Settings.EnvironmentConfig.RootUri}/Logout");
            });
        }

        [When(@"I click on the (.*) link")]
        public void WhenIClickOnTheLink(string linkLabel)
        {
            WithErrorLogging(() =>
            {
                HtmlElementHelper.FindElementByXpath(Browser, $"//a[contains(.,'{linkLabel}')]").Click();
            });
        }

        [When(@"I go to edit the '(.*)' section")]
        public void WhenIGoToEditTheSection(string section)
        {
            WithErrorLogging(() =>
            {
                var sectionId =HtmlElementHelper.GetSectionIdFromSection(section);
                var link = Browser.FindElement(By.Id($"{sectionId}-title"))
                    .FindElement(By.LinkText("Edit"));
                link.Click();
            });
        }

        [When(@"I take action on the alert with title (.*)")]
        public void WhenITakeActionOnAlert(string title)
        {
            WithErrorLogging(() =>
            {
                var alert = HtmlElementHelper.FindElementsByXpath(Browser, "//*[contains(@id, 'alert-')]").Single(a => a.Text.Contains(title));
                alert.FindElement(By.LinkText("Take action")).Click();
            });
        }

        [When(@"I expand manage notification section")]
        public void WhenIExpandNotificationSection()
        {
            WithErrorLogging(() =>
            {
                var button = Browser
                    .FindElement(By.Id("manage-notification"))
                    .FindElement(By.TagName("summary"));
                button.Click();
            });
        }

        [When(@"I expand the '(.*)' section")]
        public void WhenIExpandSection(string sectionId)
        {
            WithErrorLogging(() =>
            {
                var element = HtmlElementHelper.FindElementById(Browser, sectionId).FindElement(By.TagName("summary"));
                element.Click();
            });
        }

        #endregion

        #region Input

        [When(@"I uncheck '(.*)'")]
        public void WhenIDeselectCheckbox(string elementId)
        {
            WithErrorLogging(() =>
            {
                var element = HtmlElementHelper.FindElementById(Browser, elementId);
                if (element.Selected)
                {
                    element.Click();
                }
            });
        }

        [When(@"I check '(.*)'")]
        [When(@"I select radio value '(.*)'")]
        public void WhenISelectRadioOrCheckbox(string elementId)
        {
            WithErrorLogging(() =>
            {
                var element = HtmlElementHelper.FindElementById(Browser, elementId);
                if (!element.Selected)
                {
                    element.Click();
                }
            });
        }

        [When(@"I click on the '(.*)' button")]
        public void WhenIClickOn(string elementId)
        {
            WithErrorLogging(() =>
            {
                var button = HtmlElementHelper.FindElementById(Browser, elementId);
                button.Click();
            });
        }

        [When(@"I select (.*) from input list '(.*)'")]
        public void WhenISelectFromInputList(string value, string inputListId)
        {
            WithErrorLogging(() =>
            {
                var inputList = HtmlElementHelper.FindElementById(Browser, inputListId);
                inputList.Click();
                inputList.SendKeys(Keys.Control + "a");
                inputList.SendKeys(Keys.Delete);
                inputList.SendKeys(value);
                HtmlElementHelper.FindElementById(Browser, inputListId+"__option--0").Click();
                inputList.SendKeys("\t");
            });
        }

        [When(@"I enter (.*) into '(.*)'")]
        public void WhenIEnterValueIntoFieldWithId(string value, string elementId)
        {
            WithErrorLogging(() =>
            {
                var element = HtmlElementHelper.FindElementById(Browser, elementId);
                element.Click();
                element.SendKeys(Keys.Control + "a");
                element.SendKeys(Keys.Delete);
                element.SendKeys(value + "\t");
                if (!Settings.IsHeadless)
                {
                    Thread.Sleep(1000);
                }
            });
        }

        [When(@"I enter '(.*)' into date fields with id '(.*)'")]
        public void WhenIEnterDateIntoFieldWithId(string dateString, string elementId)
        {
            WithErrorLogging(() =>
            {
                var date = DateTime.Parse(dateString, new CultureInfo("en-GB").DateTimeFormat);
                WhenIEnterValueIntoFieldWithId(date.Day.ToString(), elementId + "_Day");
                WhenIEnterValueIntoFieldWithId(date.Month.ToString(), elementId + "_Month");
                WhenIEnterValueIntoFieldWithId(date.Year.ToString(), elementId + "_Year");
                if (!Settings.IsHeadless)
                {
                    Thread.Sleep(1000);
                }
            });
        }

        [When(@"I make selection (.*) from (.*) section for '(.*)'")]
        public void WhenISelectValueFromGroupForFieldWithId(string option, string group, string selectId)
        {
            SelectOptionFromDropdown(option, selectId, group);
        }

        [When(@"I select (.*) for '(.*)'")]
        public void WhenISelectTextFromDropdown(string text, string selectId)
        {
            SelectOptionFromDropdown(text, selectId);
        }

        #endregion

        [When(@"I wait")]
        public void WhenIWait()
        {
            WithErrorLogging(() =>
            {
                Thread.Sleep(3000);
            });
        }

        [When(@"I wait for (.*) to be missing from '(.*)'")]
        public void WhenIWaitForOptionToBeMissingFromDropdown(string text, string selectId)
        {
            WithErrorLogging(() =>
            {
                var wait = new WebDriverWait(Browser, Settings.ImplicitWait);
                wait.Until(ElementDoesNotExistInDropdown(text, selectId));
            });
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

        private void SelectOptionFromDropdown(string text, string dropdownId, string group = null)
        {
            WithErrorLogging(() =>
            {
                var xPath = group != null
                    ? $"//select[@id='{dropdownId}']/optgroup[@label='{group}']/option[normalize-space(text())='{text}']"
                    : $"//select[@id='{dropdownId}']/option[normalize-space(text())='{text}']";
                // In some scenarios the select does not become visible/active until an API call (triggered by previous input) has returned.
                // Consequently we add a wait here on the visibility of the element we will later select.
                var wait = new WebDriverWait(Browser, Settings.ImplicitWait);
                wait.Until(ExpectedConditions.ElementIsVisible(By.XPath(xPath)));
                SelectElementFromDropdownWithRetry(text, dropdownId);
            });
        }

        private void SelectElementFromDropdownWithRetry(string text, string dropdownId)
        {
            try
            {
                SelectElementFromDropdown(text, dropdownId);
            }
            catch (StaleElementReferenceException)
            {
                // In scenarios where the dropdown we're selecting from has just been reloaded (e.g. because of the result
                // of an API call) the options can become stale between finding the dropdown and selecting a value. In this
                // case we can select the value by just trying again.
                SelectElementFromDropdown(text, dropdownId);
            }
        }

        private void SelectElementFromDropdown(string text, string dropdownId)
        {
            new SelectElement(HtmlElementHelper.FindElementById(Browser, dropdownId)).SelectByText(text);
        }

        private static Func<IWebDriver, bool> ElementDoesNotExistInDropdown(string text, string dropdownId)
        {
            return driver =>
            {
                var options = GetDropdownOptionsWithRetry(driver, dropdownId);
                return options.All(opt => opt != text);
            };
        }

        private static List<string> GetDropdownOptionsWithRetry(IWebDriver driver, string dropdownId)
        {
            try
            {
                return GetDropdownOptions(driver, dropdownId);
            }
            catch (StaleElementReferenceException)
            {
                // In scenarios where the dropdown we're looking at has just been reloaded (e.g. because of the result
                // of an API call) the options can become stale between finding the select and reading their text. In this
                // case we can find the new select by just trying again.
                return GetDropdownOptions(driver, dropdownId);
            }
        }

        private static List<string> GetDropdownOptions(IWebDriver driver, string dropdownId)
        {
            return new SelectElement(HtmlElementHelper.FindElementById(driver, dropdownId))
                .Options
                .Select(opt => opt.Text)
                .ToList();
        }
    }
}
