namespace ClientApp.DTOs.Requests;

public sealed class CatalogRequestDto
{
    public int Page { get; set; }

    public int Limit { get; set; }

    public int? TypeId { get; set; }

    public int? BrandId { get; set; }
}