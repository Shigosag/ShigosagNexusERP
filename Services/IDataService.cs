using System.Collections.Generic;
using System.Threading.Tasks;
using ShigosagNexusERP.Models;

namespace ShigosagNexusERP.Services;

public interface IDataService
{
    // Product operations
    Task<List<Product>> GetProductsAsync();
    Task<Product> AddProductAsync(Product product);
    Task<bool> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int productId);

    // Customer operations
    Task<List<Customer>> GetCustomersAsync();
    Task<Customer> AddCustomerAsync(Customer customer);

    // Order operations
    Task<List<Order>> GetOrdersAsync();
    Task<Order> AddOrderAsync(Order order);
    Task<bool> UpdateOrderStatusAsync(int orderId, string status);

    // Employee operations
    Task<List<Employee>> GetEmployeesAsync();
    Task<Employee> AddEmployeeAsync(Employee employee);
    Task<bool> UpdateEmployeeStatusAsync(int employeeId, string status);
}
