using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Infrastructure.Identity;
using Infrastructure.Extensions;
using ClientApp.Configurations;
using ClientApp.Models;
using ClientApp.Services;
using ClientApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(configuration.GetValue("SessionCookieLifetimeMinutes", 60));
    })
    .AddOpenIdConnect(options =>
    {
        options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.Authority = configuration["Api:IdentityUrl"];
        options.Events.OnRedirectToIdentityProvider = async (context) =>
        {
            context.ProtocolMessage.RedirectUri = configuration["Api:RedirectUri"];
            await Task.FromResult(0);
        };
        options.Events.OnRemoteFailure = async (context) =>
        {
            context.Response.Redirect(configuration["Api:CallbackUrl"]!);
            context.HandleResponse();

            await Task.FromResult(0);
        };
        options.SignedOutRedirectUri = configuration["Api:CallbackUrl"]!;
        options.ClientId = "web_client_pkce";
        options.ClientSecret = "secret";
        options.ResponseType = "code";
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;
        options.RequireHttpsMetadata = false;
        options.UsePkce = true;
        options.Scope.Add(AuthScope.OpenId);
        options.Scope.Add(AuthScope.Profile);
        options.Scope.Add(AuthScope.WebClient);
        options.Scope.Add(AuthScope.BasketApi);
        options.Scope.Add(AuthScope.OrderApi);
    });

builder.AddBaseConfiguration();
builder.Services.Configure<ApiConfiguration>(configuration.GetSection("Api"));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IHttpClientService, HttpClientService>();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IIdentityParser<ApplicationUser>, IdentityParser>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Lax
});
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(name: "defaultError", pattern: "{controller=Error}/{action=Error}");
app.MapControllers();

app.Run();