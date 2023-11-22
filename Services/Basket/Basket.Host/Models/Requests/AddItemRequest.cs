using System.ComponentModel.DataAnnotations;

namespace Basket.Host.Models.Requests;

public sealed class AddItemRequest
{
    [Required]
    public string Data { get; set; }
}