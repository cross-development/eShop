using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public sealed class PaginatedItemRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The page should be equal to or greater than 1")]
    public int Page { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The limit should be equal to or greater than 1")]
    public int Limit { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The catalog type id should be equal to greater than 1")]
    public int? TypeId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The catalog brand id should be equal to greater than 1")]
    public int? BrandId { get; set; }
}