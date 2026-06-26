using System.ComponentModel.DataAnnotations;

namespace ShigosagNexusERP.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockLevel { get; set; }
    public string Category { get; set; } = "General";
}