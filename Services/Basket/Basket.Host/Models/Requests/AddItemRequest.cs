using System.ComponentModel.DataAnnotations;
using Basket.Host.Models.DTOs;

namespace Basket.Host.Models.Requests;

public sealed class AddItemRequest
{
    [Required]
    public BasketDataDto Data { get; set; }
}