using ShigosagNexusERP.Models;
using System;
using System.Linq;

namespace ShigosagNexusERP.Data;

/// <summary>
/// Enterprise Data Seeding Engine.
/// Synchronized with Dashboard Telemetry and Semantic UI Triggers.
/// Author: Shigosag
/// </summary>
public static class DbInitializer
{
    public static void Seed(AppDbContext context)
    {
        // Ensure Database Schema exists
        context.Database.EnsureCreated();

        // 1. IDENTITY MANAGEMENT
        if (!context.Users.Any())
        {
            context.Users.Add(new User 
            { 
                Username = "admin", 
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), 
                Role = "Admin" 
            });
        }

        // 2. WAREHOUSE & INVENTORY (Triggers Low Stock Alerts)
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product { Name = "Quantum Server Rack", SKU = "NX-SRV-01", Price = 15400.00m, StockLevel = 4, Category = "Hardware" },
                new Product { Name = "Nexus Workstation", SKU = "NX-WS-05", Price = 2500.00m, StockLevel = 12, Category = "Hardware" },
                new Product { Name = "ERP Enterprise License", SKU = "SW-NX-ENT", Price = 999.00m, StockLevel = 999, Category = "Software" },
                new Product { Name = "Pink Fiber Cable", SKU = "CBL-P-10", Price = 45.99m, StockLevel = 2, Category = "Accessories" },
                new Product { Name = "Core Uplink Module", SKU = "NX-MOD-09", Price = 450.00m, StockLevel = 1, Category = "Hardware" }
            );
        }

        // 3. HUMAN CAPITAL (Triggers Status Color Styles)
        if (!context.Employees.Any())
        {
            context.Employees.AddRange(
                new Employee { Name = "Alice Johnson", Position = "CTO", Department = "Executive", Status = "Active" },
                new Employee { Name = "Bob Smith", Position = "Sales Lead", Department = "Sales", Status = "Active" },
                new Employee { Name = "Shigosag", Position = "Software Developer", Department = "IT", Status = "Active" },
                new Employee { Name = "Segun Arulogun Gabriel", Position = "Customers Experience Auditor", Department = "QA", Status = "Active" },
                new Employee { Name = "Charlie Rose", Position = "Data Analyst", Department = "IT", Status = "On Leave" },
                new Employee { Name = "Diana Prince", Position = "HR Director", Department = "HR", Status = "Active" }
            );
        }

        // 4. FINANCIAL LEDGER (Triggers KPI Card Calculations)
        if (!context.Orders.Any())
        {
            context.Orders.AddRange(
                // Cleared Inflow
                new Order { TotalAmount = 5400.50m, Status = "Completed", OrderDate = DateTime.Now.AddDays(-10) },
                new Order { TotalAmount = 8900.00m, Status = "Completed", OrderDate = DateTime.Now.AddDays(-8) },
                
                // Pipeline Bookings
                new Order { TotalAmount = 1200.00m, Status = "Pending", OrderDate = DateTime.Now.AddDays(-5) },
                new Order { TotalAmount = 15400.00m, Status = "Processing", OrderDate = DateTime.Now.AddDays(-2) },
                new Order { TotalAmount = 3150.00m, Status = "Pending", OrderDate = DateTime.Now }
            );
        }

        // 5. CLIENT RELATIONS (CRM Module)
        if (!context.Customers.Any())
        {
            context.Customers.AddRange(
                new Customer { Name = "Tech Corp Solutions", Email = "contact@techcorp.com", Phone = "+1 800 555 0199" },
                new Customer { Name = "Global Logistics Ltd", Email = "billing@globallog.com", Phone = "+44 20 7946 0000" },
                new Customer { Name = "Nexus Innovations", Email = "hello@nexus.io", Phone = "+1 415 555 9876" },
                new Customer { Name = "Pink Cloud Systems", Email = "info@pinkcloud.net", Phone = "+1 212 555 0123" },
                new Customer { Name = "Cyberdyne Systems", Email = "admin@cyberdyne.com", Phone = "+1 310 555 1984" }
            );
        }

        context.SaveChanges();
    }
}