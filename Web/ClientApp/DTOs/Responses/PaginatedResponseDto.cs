namespace ClientApp.DTOs.Responses;

public sealed class PaginatedResponseDto<T>
    where T : class
{
    public int Page { get; init; }

    public int Limit { get; init; }

    public int Count { get; init; }

    public IEnumerable<T> Data { get; init; }
}