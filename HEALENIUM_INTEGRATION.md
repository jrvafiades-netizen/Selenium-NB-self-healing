# Healenium Integration Guide

## Overview

This project now includes **custom self-healing capabilities** inspired by Healenium. The self-healing driver automatically attempts to recover from broken element locators by trying alternative detection strategies.

## What is Self-Healing?

Self-healing in test automation means that when an element locator fails (e.g., due to DOM changes), the framework attempts to find the element using alternative strategies instead of failing immediately. This reduces test maintenance burden.

## Features

### 1. **Automatic Locator Recovery**
When an element cannot be found using the primary locator, the self-healing driver tries:
- Partial ID matching using CSS selectors
- Class name matching
- Tag name matching
- XPath fallbacks

### 2. **Healing Reports**
Track which locators were healed and how many times each was attempted:
```
=== Self-Healing Report ===
Total Healed Locators: 3

Healed Elements:
  - Original: By.Id: searchInput
    Healed: By.CssSelector: [id*='searchInput']
    Healing Attempts: 1
```

### 3. **Zero External Dependencies**
Unlike the official Healenium package, this implementation:
- Uses only Selenium WebDriver
- No external service required
- Works offline
- No licensing needed

## Usage

### Enable Self-Healing

**Option 1: In Code**
```csharp
// Use self-healing driver
var driver = new DriverFactory().CreateWithHealing();
```

**Option 2: In Configuration**
Update `App.config`:
```xml
<add key="EnableHealenium" value="true" />
```

Then the test setup will automatically use the healing driver.

### Getting Healing Reports

```csharp
[TearDown]
public void TeardownTest()
{
    if (HealeniumUtils.IsHealingDriver(_driver))
    {
        HealeniumUtils.LogHealingReport(_driver);
    }
    
    _driver.Quit();
}
```

### Using in Tests

```csharp
[Test]
public void MyTest()
{
    var homepage = new NewBalanceHomepage(_driver);
    homepage.Navigate(_baseUrl);
    
    // If element isn't found with the first locator,
    // the self-healing driver will attempt recovery
    var searchBar = homepage.GetSearchBar();
    searchBar.Displayed.Should().BeTrue();
}
```

## Implementation Details

### SelfHealingDriver Class
Located in `SeleniumHelpers/HealeniumDriver.cs`

**Key Methods:**
- `FindElement(By)` - Finds a single element with healing
- `FindElements(By)` - Finds multiple elements with healing
- `GetHealingReport()` - Returns healing statistics

**Healing Strategies:**
1. **Partial ID Match** - If `id="searchInput"` fails, tries `[id*='searchInput']`
2. **Class Name Match** - Attempts to find by class name
3. **Tag Name Match** - Falls back to tag name search
4. **XPath Patterns** - Tries alternative XPath expressions

### HealeniumUtils Class
Located in `SeleniumHelpers/HealeniumUtils.cs`

**Utility Methods:**
- `IsHealingDriver(IWebDriver)` - Check if driver has healing enabled
- `GetHealingReport(IWebDriver)` - Get healing statistics
- `GetHealingSummary(IWebDriver)` - Get formatted report
- `LogHealingReport(IWebDriver)` - Print report to console

### DriverFactory Updates
Updated to support healing creation:
- `Create()` - Standard Selenium driver (no healing)
- `CreateWithHealing()` - Self-healing enabled driver

## Configuration

### App.config Settings

```xml
<!-- Enable/Disable self-healing in tests -->
<add key="EnableHealenium" value="false" />
```

**Values:**
- `true` - Enable self-healing for all tests
- `false` - Use standard Selenium driver (default)

## Example Tests

See `SelfHealingTests.cs` for example tests demonstrating:
- Basic self-healing usage
- Healing report generation
- Recovery from locator failures

## Benefits

✅ **Reduced Test Maintenance** - Tests recover from minor DOM changes
✅ **Better Stability** - Fewer false negatives due to locator issues
✅ **Healing Visibility** - Track which locators are breaking
✅ **No External Service** - Self-contained implementation
✅ **Easy Debugging** - Healing reports show what failed and how it was recovered

## Limitations

- **Not AI-powered** - Uses pattern matching, not machine learning
- **Simple Strategies** - Works best for common locator patterns
- **Performance** - May add slight overhead when healing occurs
- **Not a Silver Bullet** - Fundamentally broken locators still fail

## When to Use

✅ Use self-healing when:
- Testing dynamic web applications
- Managing legacy test suites with brittle locators
- Need to reduce test maintenance
- Want visibility into locator failures

❌ Don't use self-healing when:
- Locators are well-maintained and stable
- Need maximum performance
- Prefer explicit failure on locator changes
- Testing with very complex selectors

## Migration from Standard Driver

To convert existing tests to use self-healing:

1. Update `App.config`:
   ```xml
   <add key="EnableHealenium" value="true" />
   ```

2. Or in code:
   ```csharp
   // Before
   _driver = new DriverFactory().Create();
   
   // After
   _driver = new DriverFactory().CreateWithHealing();
   ```

3. No other changes needed - self-healing is transparent to existing code!

## Troubleshooting

### Healing Not Working?

1. Check if driver is healing-enabled:
   ```csharp
   Assert.IsTrue(HealeniumUtils.IsHealingDriver(_driver));
   ```

2. Check healing report for details:
   ```csharp
   HealeniumUtils.LogHealingReport(_driver);
   ```

3. Ensure `App.config` setting is correct

### Performance Issues?

- Consider disabling healing for stable locators
- Use `CreateWithHealing()` only when needed
- Monitor healing report to optimize locators

## Future Enhancements

Possible improvements:
- ML-based locator matching
- Visual element recognition
- Shadow DOM support
- Cross-browser healing strategies
- Healing metrics and analytics

## References

- [Healenium Project](https://healenium.io/)
- [Selenium Documentation](https://selenium.dev/)
- [Test Automation Best Practices](https://github.com/atosorigin/SeleniumExample)
