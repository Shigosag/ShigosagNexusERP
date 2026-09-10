using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShigosagNexusERP.Models;

public class Employee
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Position { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    public string Department { get; set; } = string.Empty;

    [Required]
    [MaxLength(40)]
    public string Status { get; set; } = "Active"; // Active, On Leave, Terminated

    [Column(TypeName = "decimal(18,2)")]
    public decimal MonthlySalary { get; set; } = 6500.00m;

    public DateTime HireDate { get; set; } = DateTime.UtcNow.AddMonths(-12);
}
