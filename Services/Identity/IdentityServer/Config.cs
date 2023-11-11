using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalog_api", "Catalog api full access"),
            new ApiScope("basket_api", "Basket api full access"),
            new ApiScope("web_client", "Web client full access"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "web_client_pkce",
                ClientName = "Web Client PKCE",
                AllowedGrantTypes = GrantTypes.Code,
                ClientSecrets = { new Secret("secret".Sha256()) },
                RedirectUris = { "http://localhost:5000/signin-oidc" },
                AllowedScopes = { "openid", "profile", "web_client" },
                RequirePkce = true,
                RequireConsent = false
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
                RedirectUris = { "http://localhost:5001/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { "http://localhost:5001/swagger/" },
                AllowedScopes = { "web_client", "catalog_api" }
            },
            new Client
            {
                ClientId = "basket_swagger_ui",
                ClientName = "Basket Swagger UI",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowAccessTokensViaBrowser = true,
                RedirectUris = { "http://localhost:5002/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { "http://localhost:5002/swagger/" },
                AllowedScopes = { "web_client", "basket_api" }
            },
        };
}