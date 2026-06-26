using Microsoft.EntityFrameworkCore;
using ShigosagNexusERP.Data;
using ShigosagNexusERP.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShigosagNexusERP.Services;

public class DataService(AppDbContext db) : IDataService
{
    public async Task<List<Product>> GetProductsAsync() => await db.Products.ToListAsync();
    
    public async Task AddProductAsync(Product product)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();
    }
}