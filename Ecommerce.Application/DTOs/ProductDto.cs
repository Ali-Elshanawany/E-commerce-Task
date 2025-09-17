using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string ProductCode => $"P{Id}";
    public byte[] ImageData { get; set; }
    public string Category { get; set; }

    [Range(0, int.MaxValue)]
    public int MinQuantity { get; set; }
    public int Stock { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal Price { get; set; }
}

public class AddProductDto
{

    [JsonIgnore]
    public int Id { get; set; }

    public string Name { get; set; }

    public IFormFile ImageData { get; set; }

    public string ProductCode => $"P{Id}";
    public string Category { get; set; }

    [Range(0, int.MaxValue)]
    public int MinQuantity { get; set; }
    public int Stock { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal Price { get; set; }
}
