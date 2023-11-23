using System.Net.Mime;
using System.Text;
using System.Web;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using IdentityModel.Client;
using ClientApp.Configurations;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(
        IHttpClientFactory clientFactory,
        IHttpContextAccessor httpContextAccessor,
        ILogger<HttpClientService> logger)
    {
        _clientFactory = clientFactory;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    private string GetQueryString<TRequest>(TRequest request)
    {
        if (request == null)
        {
            return string.Empty;
        }

        var builder = new UriBuilder();

        var query = HttpUtility.ParseQueryString(builder.Query);

        foreach (var property in request.GetType().GetProperties())
        {
            query[property.Name] = property.GetValue(request)?.ToString();
        }

        builder.Query = query.ToString() ?? string.Empty;

        _logger.LogInformation($"[HttpClientService: GetQueryString] --> BUILDER QUERY: {builder.Query}");

        return builder.Query;
    }

    private StringContent GetStringContent<TContent>(TContent content)
    {
        var serializedContent = JsonConvert.SerializeObject(content);
        var stringContent = new StringContent(serializedContent, Encoding.UTF8, MediaTypeNames.Application.Json);

        _logger.LogInformation($"[HttpClientService: GetStringContent] --> STRING CONTENT: {serializedContent}");

        return stringContent;
    }

    public Task<TResponse> SendAsync<TResponse>(string url, HttpMethod method) =>
        SendAsync<TResponse, object, object>(url, method);

    public Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest request) =>
        SendAsync<TResponse, object, object>(url, method, request);

    public async Task<TResponse> SendAsync<TResponse, TRequest, TContent>(
        string url, HttpMethod method, TRequest request = default, TContent content = default)
    {
        var client = _clientFactory.CreateClient();

        var accessToken = await _httpContextAccessor.HttpContext?.GetTokenAsync("access_token")!;

        _logger.LogInformation($"[HttpClientService: SendAsync] --> ACCESS TOKEN: {accessToken}");

        if (!string.IsNullOrEmpty(accessToken))
        {
            client.SetBearerToken(accessToken);
        }

        var httpMessage = new HttpRequestMessage();

        var queryString = GetQueryString(request);
        var stringContent = GetStringContent(content);

        httpMessage.RequestUri = new Uri($"{url}{queryString}");
        httpMessage.Content = stringContent;
        httpMessage.Method = method;

        var result = await client.SendAsync(httpMessage);

        if (!result.IsSuccessStatusCode)
        {
            return default;
        }

        var resultContent = await result.Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<TResponse>(resultContent);

        return response;
    }
}