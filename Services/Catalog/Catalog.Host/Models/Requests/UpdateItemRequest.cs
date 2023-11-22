using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public sealed class UpdateItemRequest
{
    [MaxLength(100, ErrorMessage = "The product name should be 100 characters or less")]
    public string Name { get; set; }

    [MaxLength(200, ErrorMessage = "The product description should be 200 characters or less")]
    public string Description { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The product price should be greater than 1")]
    public decimal? Price { get; set; }

    public string PictureFileName { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "The amount of products on stock should not be less than 0")]
    public int? AvailableStock { get; set; }

    public int? CatalogTypeId { get; set; }

    public int? CatalogBrandId { get; set; }
}