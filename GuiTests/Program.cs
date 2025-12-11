using System;
using System.Threading.Tasks;
using Structura.GuiTests;

Console.WriteLine("Starting Selenium Tests...\n");

// Run tests with a timeout of 5 minutes
var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromMinutes(5));
var testTask = RunAllTests(cts.Token);

try
{
    testTask.Wait(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("\n✗ Tests timed out after 5 minutes");
    Environment.Exit(1);
}

Environment.Exit(0);

static async Task RunAllTests(System.Threading.CancellationToken cancellationToken)
{
    try
    {
        // Test 1: NewBalanceHomepageShouldLoad
        Console.WriteLine("=== Test 1: NewBalanceHomepageShouldLoad ===");
        var test1 = new NewBalanceTests();
        test1.NewBalanceHomepageShouldLoad();
        test1.Dispose();
        Console.WriteLine("✓ PASSED\n");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✗ FAILED: {ex.Message}\n");
    }

try
{
    // Test 2: ShouldFindSearchBarOnHomepage
    Console.WriteLine("=== Test 2: ShouldFindSearchBarOnHomepage ===");
    var test2 = new NewBalanceTests();
    test2.ShouldFindSearchBarOnHomepage();
    test2.Dispose();
    Console.WriteLine("✓ PASSED\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ FAILED: {ex.Message}\n");
}

try
{
    // Test 3: ShouldBeAbleToNavigateToMensShoes
    Console.WriteLine("=== Test 3: ShouldBeAbleToNavigateToMensShoes ===");
    var test3 = new NewBalanceTests();
    test3.ShouldBeAbleToNavigateToMensShoes();
    test3.Dispose();
    Console.WriteLine("✓ PASSED\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ FAILED: {ex.Message}\n");
}

try
{
    // Test 4: ShouldFindProductsOnSite
    Console.WriteLine("=== Test 4: ShouldFindProductsOnSite ===");
    var test4 = new NewBalanceTests();
    test4.ShouldFindProductsOnSite();
    test4.Dispose();
    Console.WriteLine("✓ PASSED\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ FAILED: {ex.Message}\n");
}

try
{
    // Test 5: DemonstrateSelfHealingCapabilities
    Console.WriteLine("=== Test 5: DemonstrateSelfHealingCapabilities ===");
    var test5 = new SelfHealingTests();
    test5.DemonstrateSelfHealingCapabilities();
    test5.Dispose();
    Console.WriteLine("✓ PASSED\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ FAILED: {ex.Message}\n");
}

try
{
    // Test 6: SelfHealingDriverCanRecoverFromBrokenLocators
    Console.WriteLine("=== Test 6: SelfHealingDriverCanRecoverFromBrokenLocators ===");
    var test6 = new SelfHealingTests();
    test6.SelfHealingDriverCanRecoverFromBrokenLocators();
    test6.Dispose();
    Console.WriteLine("✓ PASSED\n");
}
catch (Exception ex)
{
    Console.WriteLine($"✗ FAILED: {ex.Message}\n");
}

Console.WriteLine("All tests completed!");
}
