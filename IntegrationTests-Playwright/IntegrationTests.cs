using Microsoft.Playwright;

namespace IntegrationTests_Playwright;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class Tests : PageTest
{
    [Test]
    public async Task LaunchingDeviKick_GivenURL_WhenLoggingIn_ThenReturnLoggedUserHomepage()
    {
        await Page.GotoAsync("http://localhost:4200/home");
        await Page.GotoAsync("http://localhost:4200/login");
        await Page.GetByPlaceholder("Enter your email").FillAsync("shaunmbolompo426@gmail.com");
        await Page.GetByPlaceholder("Enter your password").ClickAsync();
        await Page.GetByPlaceholder("Enter your password").FillAsync("pass");
        await Page.GetByRole(AriaRole.Button, new() { Name = "LET'S GO" }).ClickAsync();
    }

    [Test]
    public async Task LaunchDevKick_GivenUserSearch_WhenOnHomepae_ThenReturnSearchResults()
    {
        await Page.GotoAsync("http://localhost:4200/home");
        await Page.GetByPlaceholder("Type to search").ClickAsync();
        await Page.GetByPlaceholder("Type to search").PressAsync("CapsLock");
        await Page.GetByPlaceholder("Type to search").FillAsync("Open");
        await Page.GetByPlaceholder("Type to search").PressAsync("CapsLock");
        await Page.GetByPlaceholder("Type to search").FillAsync("OpenTelemetry");
        await Page.GetByPlaceholder("Type to search").PressAsync("Enter");
        await Page.GetByRole(AriaRole.Button, new() { Name = "Enter title" }).ClickAsync();
    }
}