﻿using System.ComponentModel.DataAnnotations;

namespace Catalog.Host.Models.Requests;

public sealed class UpdateBrandRequest
{
    [MaxLength(100, ErrorMessage = "The brand name should be 100 characters or less")]
    public string Brand { get; set; }
}