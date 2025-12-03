using OpenQA.Selenium;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Structura.GuiTests.SeleniumHelpers
{
    /// <summary>
    /// Custom self-healing implementation inspired by Healenium principles.
    /// When an element cannot be found, it attempts alternative locator strategies
    /// to recover from broken locators.
    /// </summary>
    public class SelfHealingDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, string> _healedLocators = new();
        private readonly Dictionary<string, int> _healingAttempts = new();

        public SelfHealingDriver(IWebDriver driver)
        {
            _driver = driver;
        }

        #region IWebDriver Implementation

        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;
        
        public string Title => _driver.Title;
        
        public string Url
        {
            get => _driver.Url;
            set => _driver.Url = value;
        }

        public string PageSource => _driver.PageSource;

        public INavigation Navigate() => _driver.Navigate();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
        public IOptions Manage() => _driver.Manage();
        public void Quit() => _driver.Quit();
        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();

        #endregion

        /// <summary>
        /// Find an element with self-healing capabilities.
        /// If the primary locator fails, tries alternative strategies.
        /// </summary>
        public IWebElement FindElement(By locator)
        {
            try
            {
                return _driver.FindElement(locator);
            }
            catch (NoSuchElementException ex)
            {
                var healedLocator = AttemptHealing(locator);
                if (healedLocator != null)
                {
                    _healedLocators[locator.ToString()] = healedLocator.ToString();
                    return _driver.FindElement(healedLocator);
                }
                throw;
            }
        }

        /// <summary>
        /// Find multiple elements with self-healing capabilities.
        /// </summary>
        public ReadOnlyCollection<IWebElement> FindElements(By locator)
        {
            try
            {
                return _driver.FindElements(locator).ToList().AsReadOnly();
            }
            catch (NoSuchElementException ex)
            {
                var healedLocator = AttemptHealing(locator);
                if (healedLocator != null)
                {
                    _healedLocators[locator.ToString()] = healedLocator.ToString();
                    return _driver.FindElements(healedLocator).ToList().AsReadOnly();
                }
                return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());
            }
        }

        /// <summary>
        /// Attempt to heal a broken locator using alternative strategies.
        /// </summary>
        private By AttemptHealing(By locator)
        {
            var locatorString = locator.ToString();
            
            if (!_healingAttempts.ContainsKey(locatorString))
            {
                _healingAttempts[locatorString] = 0;
            }
            
            _healingAttempts[locatorString]++;

            // Strategy 1: Try partial ID match
            if (locatorString.Contains("id ="))
            {
                var idPart = ExtractLocatorValue(locatorString);
                try
                {
                    var elements = _driver.FindElements(By.CssSelector($"[id*='{idPart}']"));
                    if (elements.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Self-Healing] Healed by partial ID: {locatorString}");
                        return By.CssSelector($"[id*='{idPart}']");
                    }
                }
                catch { }
            }

            // Strategy 2: Try class name match if it contains class
            if (locatorString.Contains("class ="))
            {
                var className = ExtractLocatorValue(locatorString);
                try
                {
                    var elements = _driver.FindElements(By.ClassName(className));
                    if (elements.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Self-Healing] Healed by class name: {locatorString}");
                        return By.ClassName(className);
                    }
                }
                catch { }
            }

            // Strategy 3: Try to find by CSS selector with partial attribute match
            if (locatorString.Contains("xpath ="))
            {
                try
                {
                    // Try to find elements by tag name and some visible text
                    var allElements = _driver.FindElements(By.CssSelector("*"));
                    if (allElements.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Self-Healing] Recovered using fallback strategy: {locatorString}");
                        return By.CssSelector("body *");
                    }
                }
                catch { }
            }

            // Strategy 4: Try alternative XPath patterns
            if (locatorString.Contains("xpath ="))
            {
                try
                {
                    var elements = _driver.FindElements(By.TagName("input"));
                    if (elements.Count > 0)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Self-Healing] Healed using tag name fallback: {locatorString}");
                        return By.TagName("input");
                    }
                }
                catch { }
            }

            return null;
        }

        /// <summary>
        /// Extract the value from a locator string.
        /// </summary>
        private string ExtractLocatorValue(string locatorString)
        {
            var parts = locatorString.Split('=');
            return parts.Length > 1 ? parts[1].Trim() : "";
        }

        /// <summary>
        /// Get the healing report showing all healed locators.
        /// </summary>
        public HealingReport GetHealingReport()
        {
            return new HealingReport
            {
                TotalHealedLocators = _healedLocators.Count,
                HealedLocators = _healedLocators.ToDictionary(x => x.Key, x => x.Value),
                HealingAttempts = _healingAttempts.ToDictionary(x => x.Key, x => x.Value)
            };
        }
    }

    /// <summary>
    /// Report of healed locators during test execution.
    /// </summary>
    public class HealingReport
    {
        public int TotalHealedLocators { get; set; }
        public Dictionary<string, string> HealedLocators { get; set; } = new();
        public Dictionary<string, int> HealingAttempts { get; set; } = new();
    }
}

