using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Structura.GuiTests.PageObjects
{
    public class NewBalanceHomepage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public NewBalanceHomepage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public bool IsLoaded()
        {
            try
            {
                _wait.Until(d => d.FindElement(By.TagName("body")).Displayed);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public IWebElement GetSearchBar()
        {
            try
            {
                // Try multiple common search selectors
                return _driver.FindElement(By.CssSelector("[placeholder*='search' i]")) ??
                       _driver.FindElement(By.CssSelector("input[type='search']")) ??
                       _driver.FindElement(By.CssSelector("[role='searchbox']"));
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException("Search bar not found on New Balance homepage");
            }
        }

        public NewBalanceMensPage NavigateToMens()
        {
            try
            {
                // Try to find and click the Mens or Shoes navigation link
                var mensLink = _driver.FindElement(By.XPath("//a[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'mens')]")) ??
                              _driver.FindElement(By.XPath("//a[contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), 'shoes')]"));
                
                mensLink.Click();
                Thread.Sleep(2000); // Wait for page to load
                return new NewBalanceMensPage(_driver);
            }
            catch (NoSuchElementException)
            {
                throw new NoSuchElementException("Unable to find Mens or Shoes navigation link");
            }
        }

        public bool AreProductsVisible()
        {
            try
            {
                // Look for common product indicators
                var productElements = _driver.FindElements(By.CssSelector("[data-product], [class*='product'], img[alt*='shoe' i]"));
                return productElements.Count > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
