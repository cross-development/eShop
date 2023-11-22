using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public sealed class UpdateBrandRequest
{
    [MaxLength(50, ErrorMessage = "The brand name should be 50 characters or less")]
    public string Brand { get; set; }
}