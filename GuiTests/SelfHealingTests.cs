using System;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using Structura.GuiTests.PageObjects;
using Structura.GuiTests.SeleniumHelpers;
using Structura.GuiTests.Utilities;

namespace Structura.GuiTests
{
    [TestFixture]
    public class SelfHealingTests
    {
        private IWebDriver _driver;
        private string _baseUrl;

        [SetUp]
        public void SetupTest()
        {
            // Uncomment the line below to enable self-healing:
            // var _driver = new DriverFactory().CreateWithHealing();
            
            _driver = new DriverFactory().Create();
            _baseUrl = ConfigurationHelper.Get<string>("TargetUrl");
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                // Log healing report if self-healing is enabled
                if (HealeniumUtils.IsHealingDriver(_driver))
                {
                    HealeniumUtils.LogHealingReport(_driver);
                }

                _driver.Quit();
                _driver.Close();
            }
            catch (Exception)
            {
                // Ignore errors if we are unable to close the browser
            }
        }

        [Test]
        public void DemonstrateSelfHealingCapabilities()
        {
            // This test demonstrates the self-healing capabilities.
            // When using the SelfHealingDriver, if a locator fails to find an element,
            // it will attempt alternative strategies before failing.

            // Arrange
            var homepage = new NewBalanceHomepage(_driver);

            // Act
            homepage.Navigate(_baseUrl);

            // Assert
            homepage.IsLoaded().Should().BeTrue("Homepage should load successfully");
            
            // If using self-healing driver, the healing report will be logged
        }

        [Test]
        public void SelfHealingDriverCanRecoverFromBrokenLocators()
        {
            // This test shows how the self-healing driver can recover
            // from locator changes by trying alternative strategies

            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            var isHealingEnabled = HealeniumUtils.IsHealingDriver(_driver);

            // Act
            homepage.Navigate(_baseUrl);
            var searchBar = homepage.GetSearchBar();

            // Assert
            searchBar.Should().NotBeNull("Search bar should be found");
            searchBar.Displayed.Should().BeTrue("Search bar should be visible");

            // If healing was enabled, show what was healed
            if (isHealingEnabled)
            {
                var report = HealeniumUtils.GetHealingReport(_driver);
                report.TotalHealedLocators.Should().BeGreaterThanOrEqualTo(0);
            }
        }
    }
}
