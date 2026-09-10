using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        try
        {
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User 
                    { 
                        Username = "admin", 
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), 
                        Role = "Admin",
                        FullName = "System Administrator",
                        CreatedAt = DateTime.UtcNow
                    },
                    new User 
                    { 
                        Username = "manager", 
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"), 
                        Role = "Manager",
                        FullName = "Operations Lead",
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new Product { Name = "Quantum Server Rack", SKU = "NX-SRV-01", Price = 15400.00m, StockLevel = 4, Category = "Hardware" },
                    new Product { Name = "Nexus Workstation", SKU = "NX-WS-05", Price = 2500.00m, StockLevel = 12, Category = "Hardware" },
                    new Product { Name = "ERP Enterprise License", SKU = "SW-NX-ENT", Price = 999.00m, StockLevel = 999, Category = "Software" },
                    new Product { Name = "Pink Fiber Cable", SKU = "CBL-P-10", Price = 45.99m, StockLevel = 2, Category = "Accessories" },
                    new Product { Name = "Core Uplink Module", SKU = "NX-MOD-09", Price = 450.00m, StockLevel = 1, Category = "Hardware" },
                    new Product { Name = "Optic Signal Transceiver", SKU = "NX-OPT-22", Price = 820.00m, StockLevel = 18, Category = "Accessories" }
                );
            }

            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee { Name = "Alice Johnson", Position = "CTO", Department = "Executive", Status = "Active", MonthlySalary = 12500m },
                    new Employee { Name = "Bob Smith", Position = "Sales Lead", Department = "Sales", Status = "Active", MonthlySalary = 7200m },
                    new Employee { Name = "Shigosag", Position = "Principal Architect", Department = "IT", Status = "Active", MonthlySalary = 11000m },
                    new Employee { Name = "Segun Arulogun Gabriel", Position = "Quality Operations Lead", Department = "QA", Status = "Active", MonthlySalary = 8900m },
                    new Employee { Name = "Charlie Rose", Position = "Data Analyst", Department = "IT", Status = "On Leave", MonthlySalary = 6200m },
                    new Employee { Name = "Diana Prince", Position = "HR Director", Department = "HR", Status = "Active", MonthlySalary = 8500m }
                );
            }

            if (!context.Orders.Any())
            {
                context.Orders.AddRange(
                    new Order { TotalAmount = 5400.50m, Status = "Completed", OrderDate = DateTime.UtcNow.AddDays(-28), Notes = "Cleared Q3 Inflow" },
                    new Order { TotalAmount = 8900.00m, Status = "Completed", OrderDate = DateTime.UtcNow.AddDays(-15), Notes = "Hardware Deployment" },
                    new Order { TotalAmount = 1200.00m, Status = "Pending", OrderDate = DateTime.UtcNow.AddDays(-7), Notes = "Maintenance Renewal" },
                    new Order { TotalAmount = 15400.00m, Status = "Processing", OrderDate = DateTime.UtcNow.AddDays(-2), Notes = "Core Uplink Infrastructure" },
                    new Order { TotalAmount = 3150.00m, Status = "Pending", OrderDate = DateTime.UtcNow, Notes = "Terminal Upgrades" }
                );
            }

            if (!context.Customers.Any())
            {
                context.Customers.AddRange(
                    new Customer { Name = "Tech Corp Solutions", Email = "contact@techcorp.com", Phone = "+1 800 555 0199", Company = "Tech Corp", Status = "Active" },
                    new Customer { Name = "Global Logistics Ltd", Email = "billing@globallog.com", Phone = "+44 20 7946 0000", Company = "Global Logistics", Status = "Active" },
                    new Customer { Name = "Nexus Innovations", Email = "hello@nexus.io", Phone = "+1 415 555 9876", Company = "Nexus Labs", Status = "Active" },
                    new Customer { Name = "Pink Cloud Systems", Email = "info@pinkcloud.net", Phone = "+1 212 555 0123", Company = "Pink Cloud", Status = "Active" },
                    new Customer { Name = "Cyberdyne Systems", Email = "admin@cyberdyne.com", Phone = "+1 310 555 1984", Company = "Cyberdyne", Status = "Active" }
                );
            }

            context.SaveChanges();
        }
        catch (Exception)
        {
            // Seed operations should fail gracefully without terminating initialization
        }
    }
}
