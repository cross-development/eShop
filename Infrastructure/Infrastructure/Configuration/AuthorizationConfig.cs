namespace Infrastructure.Configuration;

public sealed class AuthorizationConfig
{
    public string Authority { get; set; }

    public string SiteAudience { get; set; }
}