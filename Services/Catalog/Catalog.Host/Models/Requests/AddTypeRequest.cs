using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public sealed class AddTypeRequest
{
    [Required]
    [MaxLength(50, ErrorMessage = "The brand type should be 50 characters or less")]
    public string Type { get; set; }
}