namespace Catalog.Host.Models.DTOs;

public sealed class CatalogItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public decimal Price { get; set; }

    public string PictureUrl { get; set; }

    public CatalogTypeDto CatalogType { get; set; }

    public CatalogBrandDto CatalogBrand { get; set; }

    public int AvailableStock { get; set; }
}