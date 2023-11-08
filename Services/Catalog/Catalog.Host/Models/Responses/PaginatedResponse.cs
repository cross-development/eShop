namespace Catalog.Host.Models.Responses;

public sealed class PaginatedResponse<T>
{
    public int Page { get; init; }

    public int Limit { get; init; }

    public int Count { get; init; }

    public IEnumerable<T> Data { get; init; } = null!;
}