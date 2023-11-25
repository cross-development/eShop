namespace Infrastructure.Identity;

public static class AuthScope
{
    // Base scopes
    public const string OpenId = "openid";

    public const string Profile = "profile";

    // Custom scopes
    public const string WebClient = "web_client";

    public const string CatalogApiItems = "catalog_api_items";

    public const string CatalogApiBrands = "catalog_api_brands";

    public const string CatalogApiTypes = "catalog_api_types";

    public const string BasketApi = "basket_api";

    public const string OrderApi = "order_api";
}