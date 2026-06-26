using Microsoft.EntityFrameworkCore;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Employee> Employees => Set<Employee>(); // HR Module

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=shigosag_nexus.db");
    }
}