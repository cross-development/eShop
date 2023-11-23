using ClientApp.Models;

namespace ClientApp.DTOs.Responses;

public sealed class BrandResponseDto
{
    public IEnumerable<CatalogBrand> Data { get; set; }
}