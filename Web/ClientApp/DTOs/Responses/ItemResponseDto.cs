using ClientApp.Models;

namespace ClientApp.DTOs.Responses;

public class ItemResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string PictureUrl { get; set; }

    public CatalogType CatalogType { get; set; }

    public CatalogBrand CatalogBrand { get; set; }

    public int AvailableStock { get; set; }
}