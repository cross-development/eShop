using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Infrastructure.Data;
using Infrastructure.Data.Interfaces;
using Infrastructure.Filters;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Extensions;
using Catalog.Host.Configurations;
using Catalog.Host.Data;
using Catalog.Host.Repositories;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services;
using Catalog.Host.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddControllers(options =>
{
    options.Filters.Add<HttpGlobalExceptionFilter>();
    options.Conventions.Add(new RouteTokenTransformerConvention(new KebabCaseRouteTransformer()));
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "eShop - Catalog HTTP API",
        Version = "v1",
        Description = "The Catalog Service HTTP API"
    });

    var authority = configuration["Authorization:Authority"];

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows()
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                TokenUrl = new Uri($"{authority}/connect/token"),
                Scopes = new Dictionary<string, string>()
                {
                    { AuthScope.WebClient, "Web client full access" },
                    { AuthScope.CatalogApiItems, "Catalog api (items) full access" },
                    { AuthScope.CatalogApiBrands, "Catalog api (brands) full access" },
                    { AuthScope.CatalogApiTypes, "Catalog api (types) full access" },
                }
            }
        }
    });

    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.AddBaseConfiguration();
builder.Services.Configure<ApiConfiguration>(configuration.GetSection("Api"));

builder.Services.AddAuthorization(configuration);

builder.Services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
builder.Services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
builder.Services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();
builder.Services.AddTransient<ICatalogItemService, CatalogItemService>();
builder.Services.AddTransient<ICatalogBrandService, CatalogBrandService>();
builder.Services.AddTransient<ICatalogTypeService, CatalogTypeService>();

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<IApplicationDbContextWrapper<ApplicationDbContext>, ApplicationDbContextWrapper<ApplicationDbContext>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        corsPolicyBuilder => corsPolicyBuilder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
        );
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint($"{configuration["Api:BaseUrl"]}/swagger/v1/swagger.json", "Catalog.API V1");
    options.OAuthClientId("catalog_swagger_ui");
    options.OAuthAppName("Catalog Swagger UI");
});

app.UseRouting();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultControllerRoute();
app.MapControllers();

DbInitializer.Init(app);

app.Run();