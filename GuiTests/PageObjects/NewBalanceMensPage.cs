using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Structura.GuiTests.PageObjects
{
    public class NewBalanceMensPage
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public NewBalanceMensPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
        }

        public bool IsLoaded()
        {
            try
            {
                _wait.Until(d => d.FindElement(By.TagName("main")).Displayed);
                return true;
            }
            catch (TimeoutException)
            {
                return false;
            }
        }

        public IList<IWebElement> GetProducts()
        {
            try
            {
                return _driver.FindElements(By.CssSelector("[data-product], [class*='product-item'], [class*='shoe-tile']"));
            }
            catch (NoSuchElementException)
            {
                return new List<IWebElement>();
            }
        }

        public bool HasProducts()
        {
            return GetProducts().Count > 0;
        }
    }
}
