SeleniumExample - Using Selenium with C#.NET - now updated for 2025 featuring .NET 10 and Selenium 4
===============

This solution demonstrates automated testing web pages with Selenium and C#.NET. It can also be used as a template for new Selenium test projects.

## Current Tests

The tests now run against the **New Balance website** (https://www.newbalance.com/) and include the following:

- **NewBalanceHomepageShouldLoad** - Verifies the New Balance homepage loads successfully
- **ShouldFindSearchBarOnHomepage** - Verifies the search functionality is available
- **ShouldBeAbleToNavigateToMensShoes** - Tests navigation to the Mens section
- **ShouldFindProductsOnSite** - Verifies products are displayed on the site

## Requirements

- .NET 10 SDK or later
- Chrome, Firefox, or Edge browser (default is Chrome)

## Running the Tests

### Via Command Line

```bash
cd GuiTests
dotnet test
```

### Via NUnit Test Runner

```bash
dotnet test --logger "console;verbosity=detailed"
```

### Configuration

The target browser and URL are configured in `GuiTests/App.config`:

```xml
<add key="DriverToUse" value="Chrome" />  <!-- Chrome, Firefox, or Edge -->
<add key="TargetUrl" value="https://www.newbalance.com/" />
```

## Project Structure

The tests are structured according to the [Page Object Pattern](https://selenium.dev/documentation/test_practices/encouraged/page_object_models/):

- **PageObjects** - Contains page object classes that represent web pages
  - `NewBalanceHomepage.cs` - Models the New Balance homepage
  - `NewBalanceMensPage.cs` - Models the New Balance Mens section
- **SeleniumHelpers** - Contains helper classes for Selenium operations
- **Utilities** - Contains utility classes for configuration and validation

## Modern Updates (2025)

- **Framework**: Upgraded from .NET Framework 4.0 to .NET 10.0
- **Selenium**: Updated from 3.141.0 to 4.27.0
- **Project Format**: Converted to modern SDK-style project format
- **Page Objects**: Modernized to use direct element selection instead of deprecated PageFactory attributes
- **Testing**: Updated NUnit from 3.11.0 to 4.2.2
- **Assertions**: Updated FluentAssertions from 3.2.2 to 6.12.1

## Extension Methods

The solution includes the *FindElementByJQuery* extension method for locating elements using JQuery selectors:

```csharp
_driver.FindElementByJQuery("input[name='btnSubmit']")
```