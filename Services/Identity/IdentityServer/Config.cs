using Duende.IdentityServer.Models;
using Infrastructure.Identity;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource(AuthResource.WebClient, "Web client")
            {
                Scopes = { AuthScope.WebClient }
            },
            new ApiResource(AuthResource.CatalogApi, "Catalog API")
            {
                Scopes = { AuthScope.CatalogApi }
            },
            new ApiResource(AuthResource.BasketApi, "Basket API")
            {
                Scopes = { AuthScope.BasketApi }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope(AuthScope.WebClient, "Web client full access"),
            new ApiScope(AuthScope.CatalogApi, "Catalog api full access"),
            new ApiScope(AuthScope.BasketApi, "Basket api full access"),
        };

    public static IEnumerable<Client> Clients(ConfigurationManager configuration) =>
        new Client[]
        {
            new Client
            {
                ClientId = "web_client_pkce",
                ClientName = "Web Client PKCE",
                RequirePkce = true,
                RequireConsent = false,
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = { $"{configuration["Api:WebClientUrl"]}/signin-oidc" },
                PostLogoutRedirectUris = { $"{configuration["Api:WebClientUrl"]}/signout-callback-oidc" },
                AllowedScopes = { AuthScope.OpenId, AuthScope.Profile, AuthScope.WebClient, AuthScope.BasketApi }
            },
            new Client
            {
                ClientId = "catalog_api",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) }
            },
            new Client
            {
                ClientId = "catalog_swagger_ui",
                ClientName = "Catalog Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{configuration["Api:CatalogUrl"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["Api:CatalogUrl"]}/swagger/" },
                AllowedScopes = { AuthScope.WebClient, AuthScope.CatalogApi }
            },
            new Client
            {
                ClientId = "basket_api",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
            },
            new Client
            {
                ClientId = "basket_swagger_ui",
                ClientName = "Basket Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { $"{configuration["Api:BasketUrl"]}/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { $"{configuration["Api:BasketUrl"]}/swagger/" },
                AllowedScopes = { AuthScope.WebClient, AuthScope.BasketApi }
            },
        };
}