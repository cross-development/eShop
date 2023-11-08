namespace Catalog.Host.Models.Responses;

public sealed class DataResponse<T>
{
    public IEnumerable<T> Data { get; init; } = null!;
}