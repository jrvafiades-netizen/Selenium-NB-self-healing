using System;
using System.Text;
using System.Threading;
using FluentAssertions;
using NUnit.Framework;
using OpenQA.Selenium;
using Structura.GuiTests.PageObjects;
using Structura.GuiTests.SeleniumHelpers;
using Structura.GuiTests.Utilities;

namespace Structura.GuiTests
{
    [TestFixture]
    public class NewBalanceTests
    {
        private IWebDriver _driver;
        private string _baseUrl;

        [SetUp]
        public void SetupTest()
        {
            var enableHealing = ConfigurationHelper.Get<bool>("EnableHealenium");
            _driver = enableHealing 
                ? new DriverFactory().CreateWithHealing() 
                : new DriverFactory().Create();
            _baseUrl = ConfigurationHelper.Get<string>("TargetUrl");
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                _driver.Quit();
                _driver.Close();
            }
            catch (Exception)
            {
                // Ignore errors if we are unable to close the browser
            }
        }

        [Test]
        public void NewBalanceHomepageShouldLoad()
        {
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);

            // Act
            homepage.Navigate(_baseUrl);

            // Assert
            homepage.IsLoaded().Should().BeTrue("New Balance homepage should load successfully");
        }

        [Test]
        public void ShouldFindSearchBarOnHomepage()
        {
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var searchElement = homepage.GetSearchBar();

            // Assert
            searchElement.Displayed.Should().BeTrue("Search bar should be visible on the homepage");
        }

        [Test]
        public void ShouldBeAbleToNavigateToMensShoes()
        {
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var mensPage = homepage.NavigateToMens();

            // Assert
            mensPage.IsLoaded().Should().BeTrue("Mens page should load successfully");
        }

        [Test]
        public void ShouldFindProductsOnSite()
        {
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var productsVisible = homepage.AreProductsVisible();

            // Assert
            productsVisible.Should().BeTrue("Products should be visible on the New Balance site");
        }
    }
}


