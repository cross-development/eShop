using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Infrastructure.Configuration;

namespace Infrastructure.Extensions;

public static class ConfigurationExtensions
{
    public static void AddBaseConfiguration(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;

        builder.Services.Configure<ClientConfig>(configuration.GetSection("Client"));
        builder.Services.Configure<AuthorizationConfig>(configuration.GetSection("Authorization"));
    }
}