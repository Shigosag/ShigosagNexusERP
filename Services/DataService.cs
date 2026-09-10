using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public class DataService : IDataService
{
    private readonly AppDbContext _db;

    public DataService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        return await _db.Products.Where(p => !p.IsArchived).AsNoTracking().ToListAsync();
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        product.UpdatedAt = DateTime.UtcNow;
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<bool> UpdateProductAsync(Product product)
    {
        var existing = await _db.Products.FindAsync(product.Id);
        if (existing == null) return false;

        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.StockLevel = product.StockLevel;
        existing.Category = product.Category;
        existing.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var existing = await _db.Products.FindAsync(productId);
        if (existing == null) return false;

        existing.IsArchived = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Customer>> GetCustomersAsync()
    {
        return await _db.Customers.AsNoTracking().OrderBy(c => c.Name).ToListAsync();
    }

    public async Task<Customer> AddCustomerAsync(Customer customer)
    {
        customer.CreatedAt = DateTime.UtcNow;
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync();
        return customer;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await _db.Orders.AsNoTracking().OrderByDescending(o => o.OrderDate).ToListAsync();
    }

    public async Task<Order> AddOrderAsync(Order order)
    {
        order.OrderDate = DateTime.UtcNow;
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        return order;
    }

    public async Task<bool> UpdateOrderStatusAsync(int orderId, string status)
    {
        var existing = await _db.Orders.FindAsync(orderId);
        if (existing == null) return false;

        existing.Status = status;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<List<Employee>> GetEmployeesAsync()
    {
        return await _db.Employees.AsNoTracking().OrderBy(e => e.Name).ToListAsync();
    }

    public async Task<Employee> AddEmployeeAsync(Employee employee)
    {
        employee.HireDate = DateTime.UtcNow;
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
        return employee;
    }

    public async Task<bool> UpdateEmployeeStatusAsync(int employeeId, string status)
    {
        var existing = await _db.Employees.FindAsync(employeeId);
        if (existing == null) return false;

        existing.Status = status;
        await _db.SaveChangesAsync();
        return true;
    }
}
