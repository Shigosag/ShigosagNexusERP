using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShigosagNexusERP.Models;

public class Order
{
    [Key]
    public int Id { get; set; }

    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public int CustomerId { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = "Pending"; // Completed, Pending, Processing, Cancelled

    [MaxLength(200)]
    public string Notes { get; set; } = string.Empty;
}
