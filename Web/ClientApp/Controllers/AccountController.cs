using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using ClientApp.Models;
using ClientApp.Services.Interfaces;

namespace ClientApp.Controllers;

[Authorize]
public sealed class AccountController : Controller
{
    private readonly ILogger<AccountController> _logger;
    private readonly IIdentityParser<ApplicationUser> _identityParser;

    public AccountController(
        ILogger<AccountController> logger,
        IIdentityParser<ApplicationUser> identityParser)
    {
        _logger = logger;
        _identityParser = identityParser;
    }

    public IActionResult SignIn()
    {
        var user = _identityParser.Parse(User);

        _logger.LogInformation($"[AccountController: SignIn] ==> USER ID {user.Id}\n");

        return RedirectToAction(nameof(HomeController.Index), "Home");
    }

    public async Task<IActionResult> Signout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme);

        var homeUrl = Url.Action(nameof(HomeController.Index), "Home");

        _logger.LogInformation($"[AccountController: Signout] ==> SIGN OUT REDIRECT URL {homeUrl}\n");

        return new SignOutResult(OpenIdConnectDefaults.AuthenticationScheme,
            new AuthenticationProperties { RedirectUri = homeUrl });
    }
}