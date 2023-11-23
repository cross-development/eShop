namespace ClientApp.Services.Interfaces;

public interface IHttpClientService
{
    Task<TResponse> SendAsync<TResponse>(string url, HttpMethod method);

    Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest request);

    Task<TResponse> SendAsync<TResponse, TRequest, TContent>(string url, HttpMethod method, TRequest request, TContent content);
}