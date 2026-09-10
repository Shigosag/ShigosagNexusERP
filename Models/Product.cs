using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShigosagNexusERP.Models;

public class Product
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string SKU { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    public int StockLevel { get; set; }

    [MaxLength(60)]
    public string Category { get; set; } = "General";

    public bool IsArchived { get; set; } = false;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
