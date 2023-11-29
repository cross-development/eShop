using System.ComponentModel.DataAnnotations;

namespace Order.Host.Models.Requests;

public sealed class PaginatedRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The page should be equal to or greater than 1")]
    public int Page { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "The limit should be equal to or greater than 1")]
    public int Limit { get; set; }
}