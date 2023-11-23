using ClientApp.Models;

namespace ClientApp.DTOs.Responses;

public sealed class CatalogResponseDto
{
    public int Page { get; init; }

    public int Limit { get; init; }

    public int Count { get; init; }

    public IEnumerable<CatalogItem> Data { get; init; }
}