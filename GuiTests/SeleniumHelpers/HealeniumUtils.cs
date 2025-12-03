using OpenQA.Selenium;
using System;
using System.Text;

namespace Structura.GuiTests.SeleniumHelpers
{
    /// <summary>
    /// Self-healing utilities for element locators.
    /// Provides methods to track and report on healed locators.
    /// </summary>
    public static class HealeniumUtils
    {
        /// <summary>
        /// Check if the driver is self-healing enabled.
        /// </summary>
        public static bool IsHealingDriver(IWebDriver driver)
        {
            return driver is SelfHealingDriver;
        }

        /// <summary>
        /// Get the healing report from a self-healing driver.
        /// </summary>
        public static HealingReport GetHealingReport(IWebDriver driver)
        {
            if (driver is SelfHealingDriver healingDriver)
            {
                return healingDriver.GetHealingReport();
            }
            throw new InvalidOperationException("Driver is not a self-healing driver");
        }

        /// <summary>
        /// Generate a summary of all healed locators.
        /// </summary>
        public static string GetHealingSummary(IWebDriver driver)
        {
            if (!IsHealingDriver(driver))
            {
                return "Driver is not self-healing enabled";
            }

            var report = GetHealingReport(driver);
            var summary = new StringBuilder();
            
            summary.AppendLine("=== Self-Healing Report ===");
            summary.AppendLine($"Total Healed Locators: {report.TotalHealedLocators}");
            
            if (report.HealedLocators.Count > 0)
            {
                summary.AppendLine("\nHealed Elements:");
                foreach (var element in report.HealedLocators)
                {
                    summary.AppendLine($"  - Original: {element.Key}");
                    summary.AppendLine($"    Healed: {element.Value}");
                    var attempts = report.HealingAttempts.ContainsKey(element.Key) ? report.HealingAttempts[element.Key] : 0;
                    summary.AppendLine($"    Healing Attempts: {attempts}");
                }
            }

            return summary.ToString();
        }

        /// <summary>
        /// Log healing report to console.
        /// </summary>
        public static void LogHealingReport(IWebDriver driver)
        {
            Console.WriteLine(GetHealingSummary(driver));
        }
    }
}
