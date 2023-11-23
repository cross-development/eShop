using ClientApp.Models;

namespace ClientApp.DTOs.Responses;

public sealed class TypeResponseDto
{
    public IEnumerable<CatalogType> Data { get; set; }
}