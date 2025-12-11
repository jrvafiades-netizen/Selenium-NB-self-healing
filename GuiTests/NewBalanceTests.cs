using System;
using System.Text;
using System.Threading;
using Xunit;
using OpenQA.Selenium;
using Structura.GuiTests.PageObjects;
using Structura.GuiTests.SeleniumHelpers;
using Structura.GuiTests.Utilities;

namespace Structura.GuiTests
{
    public class NewBalanceTests : IDisposable
    {
        private IWebDriver _driver;
        private string _baseUrl;

        public NewBalanceTests()
        {
            var enableHealing = ConfigurationHelper.Get<bool>("EnableHealenium");
            _driver = enableHealing 
                ? new DriverFactory().CreateWithHealing() 
                : new DriverFactory().Create();
            _baseUrl = ConfigurationHelper.Get<string>("TargetUrl");
            Console.WriteLine("Setup: Driver created");
        }

        public void Dispose()
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

        [Fact]
        public void NewBalanceHomepageShouldLoad()
        {
            Console.WriteLine("TEST: NewBalanceHomepageShouldLoad");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);

            // Act
            homepage.Navigate(_baseUrl);

            // Assert
            Assert.True(homepage.IsLoaded(), "New Balance homepage should load successfully");
        }

        [Fact]
        public void ShouldFindSearchBarOnHomepage()
        {
            Console.WriteLine("TEST: ShouldFindSearchBarOnHomepage");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var searchElement = homepage.GetSearchBar();

            // Assert
            Assert.True(searchElement.Displayed, "Search bar should be visible on the homepage");
        }

        [Fact]
        public void ShouldBeAbleToNavigateToMensShoes()
        {
            Console.WriteLine("TEST: ShouldBeAbleToNavigateToMensShoes");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var mensPage = homepage.NavigateToMens();

            // Assert
            Assert.True(mensPage.IsLoaded(), "Mens page should load successfully");
        }

        [Fact]
        public void ShouldFindProductsOnSite()
        {
            Console.WriteLine("TEST: ShouldFindProductsOnSite");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var productsVisible = homepage.AreProductsVisible();

            // Assert
            Assert.True(productsVisible, "Products should be visible on the New Balance site");
        }
    }
}


