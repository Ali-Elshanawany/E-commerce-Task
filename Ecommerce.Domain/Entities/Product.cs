using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities;

public class Product
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
