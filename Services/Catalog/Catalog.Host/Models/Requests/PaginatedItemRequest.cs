namespace Catalog.Host.Models.Requests;

public sealed class PaginatedItemRequest
{
    public int Page { get; set; }

    public int Limit { get; set; }

    public int? TypeId { get; set; }

    public int? BrandId { get; set; }
}