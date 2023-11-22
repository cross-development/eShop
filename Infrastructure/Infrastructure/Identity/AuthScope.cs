namespace Infrastructure.Identity;

public static class AuthScope
{
    // Base scopes
    public const string OpenId = "openid";

    public const string Profile = "profile";

    // Custom scopes
    public const string CatalogApi = "catalog_api";

    public const string BasketApi = "basket_api";

    public const string WebClient = "web_client";
}