using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Data;

public class AppDbContext : DbContext
{
    private readonly IConfiguration? _configuration;

    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options, IConfiguration? configuration = null) 
        : base(options)
    {
        _configuration = configuration;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var config = _configuration ?? new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                .Build();

            var provider = config["DatabaseProvider"] ?? "SQLite";
            var defaultConn = config.GetConnectionString("DefaultConnection");
            var sqliteConn = config.GetConnectionString("FallbackSqlite") ?? "Data Source=shigosag_nexus.db";

            if (provider.Equals("PostgreSQL", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(defaultConn))
            {
                optionsBuilder.UseNpgsql(defaultConn, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(2), errorCodesToAdd: null);
                });
            }
            else
            {
                optionsBuilder.UseSqlite(sqliteConn);
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
        modelBuilder.Entity<Product>().HasIndex(p => p.SKU).IsUnique();
        modelBuilder.Entity<Customer>().HasIndex(c => c.Email);
        modelBuilder.Entity<Order>().HasIndex(o => o.OrderDate);
        modelBuilder.Entity<Employee>().HasIndex(e => e.Department);
    }
}
