using System;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;
using Structura.GuiTests.Utilities;

namespace Structura.GuiTests.SeleniumHelpers
{
    public enum DriverToUse
    {
        InternetExplorer,
        Chrome,
        Firefox
    }

    public class DriverFactory
    {
        private static FirefoxOptions FirefoxOptions
        {
            get
            {
                var firefoxProfile = new FirefoxOptions();
                firefoxProfile.SetPreference("network.automatic-ntlm-auth.trusted-uris", "http://localhost");
                return firefoxProfile;
            }
        }

        /// <summary>
        /// Create a standard Selenium WebDriver.
        /// </summary>
        public IWebDriver Create()
        {
            return CreateInternal(enableHealing: false);
        }

        /// <summary>
        /// Create a self-healing WebDriver with custom locator recovery.
        /// </summary>
        public IWebDriver CreateWithHealing()
        {
            return CreateInternal(enableHealing: true);
        }

        private IWebDriver CreateInternal(bool enableHealing)
        {
            IWebDriver driver;
            var driverToUse = ConfigurationHelper.Get<DriverToUse>("DriverToUse");
            var headlessMode = ConfigurationHelper.Get<bool>("HeadlessMode");

            switch (driverToUse)
            {
                case DriverToUse.InternetExplorer:
                    driver = new InternetExplorerDriver(AppDomain.CurrentDomain.BaseDirectory, new InternetExplorerOptions(), TimeSpan.FromMinutes(5));
                    break;
                case DriverToUse.Firefox:
                    var firefoxProfile = FirefoxOptions;
                    if (headlessMode)
                    {
                        firefoxProfile.AddArgument("--headless");
                    }
                    driver = new FirefoxDriver(firefoxProfile);
                    driver.Manage().Window.Maximize();
                    break;
                case DriverToUse.Chrome:
                    var chromeOptions = new ChromeOptions();
                    if (headlessMode)
                    {
                        chromeOptions.AddArgument("--headless");
                    }
                    // Don't pass a path - let Selenium Manager handle driver automatically
                    driver = new ChromeDriver(chromeOptions);
                    driver.Manage().Window.Maximize();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Apply self-healing wrapper if enabled
            if (enableHealing)
            {
                driver = new SelfHealingDriver(driver);
            }

            driver.Manage().Window.Maximize();
            var timeouts = driver.Manage().Timeouts();

            timeouts.ImplicitWait = TimeSpan.FromSeconds(ConfigurationHelper.Get<int>("ImplicitlyWait"));
            timeouts.PageLoad = TimeSpan.FromSeconds(ConfigurationHelper.Get<int>("PageLoadTimeout"));

            // Suppress the onbeforeunload event first. This prevents the application hanging on a dialog box that does not close.
            ((IJavaScriptExecutor)driver).ExecuteScript("window.onbeforeunload = function(e){};");
            return driver;
        }
    }
}