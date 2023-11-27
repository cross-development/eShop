using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public sealed class UpdateTypeRequest
{
    [MaxLength(100, ErrorMessage = "The brand type should be 100 characters or less")]
    public string Type { get; set; }
}